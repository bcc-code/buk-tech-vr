using System;
using Buk.PhysicsLogic.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Buk.PhysicsLogic.Implementation
{
  public class PhysicsGun : MonoBehaviour, IGun
  {
    public GameObject bulletType;
    public InputAction trigger;
    public bool inputEnabled;
    // How fast the bullet is launched
    public float maxMuzzleVelocity = 10.0f;
    // Seconds before you can shoot a new bullet.
    public float coolDown = .25f;
    // At what time was the last bullet shot.
    private float lastShotTime = 0.0f;

    private Rigidbody shooterBody;
    public bool CanShoot { get => inputEnabled && Time.fixedTime - lastShotTime >= coolDown; }

    protected virtual void TriggerPressed(InputAction.CallbackContext _)
    {
      if (CanShoot) {
        Shoot(maxMuzzleVelocity);
      }
    }

    protected virtual void TriggerReleased(InputAction.CallbackContext _)
    {
      // Do nothing
    }

    public void Awake()
    {

      if (bulletType == null)
      {
        throw new Exception("You must add a bullet type!");
      }

      if (bulletType.GetComponentInChildren<Rigidbody>() == null)
      {
        throw new Exception("Your bullet must have a Rigidbody to use it with this gun!");
      }
      shooterBody = GetComponentInParent<Rigidbody>();
      InputSetup();
    }

    public virtual void InputSetup() {
      if (trigger != null)
      {
        trigger.started += TriggerPressed;
        trigger.canceled += TriggerReleased;
        trigger.Enable();
      }
    }

    public void Shoot(float velocity)
    {
      lastShotTime = Time.fixedTime;
      // Create a new copy of bulletType using the gun's position and rotation.
      var bulletBody = Instantiate(bulletType, transform.position, transform.rotation)
        // Get the Rigidbody of that bullet, so that we can apply physics to it.
        .GetComponentInChildren<Rigidbody>();
        // If possible
        if (shooterBody) {
          // Make the bullet start moving just as fast as the shooter.
          // This makes the behaviour more realistic
          bulletBody.velocity = shooterBody.velocity;
        }
        // Apply velocity to the bullet's body, relative to its current position and rotation
        bulletBody.AddRelativeForce(0f, velocity, 0f, ForceMode.VelocityChange);
    }

    public void OnDestroy()
    {
      if (trigger != null)
      {
        trigger.started -= TriggerPressed;
        trigger.performed -= TriggerReleased;
      }
    }
  }
}
