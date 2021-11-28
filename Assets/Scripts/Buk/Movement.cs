using UnityEngine;
using UnityEngine.InputSystem;

namespace Buk
{
  public class Movement : MonoBehaviour
  {
    // Two axis movement forward/backward and left/right
    [Header("Movement")]
    public InputAction move;
    // Single button to jump
    [Header("Jump")]
    public InputAction jump;
    // Single axis rotation left/right
    [Header("Rotate")]
    public InputAction rotate;
    [Header("Speed and acceleration")]
    public float moveAcceleration = 5f;
    public float strafeAcceleration = 3f;
    public float rotateVelocity = 1f;
    public float jumpVelocity = 3f;
    public float maxVelocity = 15f;

    private new CapsuleCollider collider;
    private Rigidbody body;
    private bool onGround = false;

    public void Awake()
    {
      collider = GetComponent<CapsuleCollider>();
      body = collider.attachedRigidbody;
      if (jump != null) {
        jump.performed += (_) => Jump();
      }
      move?.Enable();
      jump?.Enable();
      rotate?.Enable();
    }

    public void Jump()
    {
      // If there's anything below the player.
      if (onGround)
      {
        body.velocity += new Vector3(0f, jumpVelocity, 0f);
      }
    }
    public void Update()
    {
      if (Physics.Raycast(transform.position, Vector3.down, out var _, collider.bounds.size.y))
      {
        onGround = true;
      }
      else
      {
        onGround = false;
      }
      var rotation = rotate?.ReadValue<float>() ?? 0;
      var movement = move?.ReadValue<Vector2>() ?? Vector2.zero;
      // Must be on the ground AND
      if (onGround)
      {
        // Rotate the player, not the RigidBody (which is rotation locked relative to the player)
        gameObject.transform.localRotation *= Quaternion.AngleAxis((rotation) * rotateVelocity, Vector3.up);
        // Either not at maximum velocity
        if (body.velocity.magnitude < maxVelocity
        // Or trying to decrease velocity
        || (body.velocity + new Vector3(movement.x, 0, movement.y)).magnitude < body.velocity.magnitude)
        {
          var force = new Vector3(movement.x * strafeAcceleration, 0, movement.y * moveAcceleration);
          body.AddRelativeForce(force, ForceMode.Acceleration);
        }
      }
    }
  }
}
