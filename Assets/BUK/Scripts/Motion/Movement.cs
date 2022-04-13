using UnityEngine;
using UnityEngine.InputSystem;

namespace Buk.Motion
{
  [RequireComponent(typeof(CapsuleCollider))]
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
    public float offGroundAccelerationFactor = 0.15f;

    private CapsuleCollider collider;
    private Camera camera;
    private Rigidbody body;
    private bool onGround = false;

    public void Awake()
    {
      collider = GetComponent<CapsuleCollider>();
      camera = GetComponentInChildren<Camera>();
      body = collider.attachedRigidbody;
      if (jump != null) {
        jump.performed += Jump;
      }
      move?.Enable();
      jump?.Enable();
      rotate?.Enable();
    }

    public void Jump(InputAction.CallbackContext _)
    {
      // If there's anything below the player.
      if (onGround)
      {
        body.velocity += Vector3.up * jumpVelocity;
      }
    }

    public void FixedUpdate()
    {
      var newOnGround = Physics.SphereCast(transform.position, collider.bounds.extents.x * 0.9f, Vector3.down, out var _, collider.bounds.extents.y * 1.1f);
      if (onGround != newOnGround) {
        onGround = newOnGround;
      };
      // Rotate character in VR using controller, this value is always zero if using mouse look on the PC.
      var rotation = (rotate?.ReadValue<Vector2>() ?? Vector2.zero).x;
      var movement = Vector2.Scale(move?.ReadValue<Vector2>() ?? Vector2.zero, new Vector2(strafeAcceleration, moveAcceleration));
      if (!onGround) {
        // Reduce acceleration in the air.
        movement.Scale(new Vector2(offGroundAccelerationFactor, offGroundAccelerationFactor));
      }
      transform.rotation *= Quaternion.AngleAxis(rotation * rotateVelocity, Vector3.up);
      body.AddForce(Quaternion.Euler(0, camera.transform.rotation.eulerAngles.y, 0) * new Vector3(movement.x, 0, movement.y), ForceMode.Acceleration);
      // Limit velocity
      var xzVelocity = new Vector3(body.velocity.x, 0, body.velocity.z);
      var yVelocity = new Vector3(0, body.velocity.y, 0);
      if (xzVelocity.magnitude > maxVelocity) {
        body.velocity = xzVelocity.normalized * maxVelocity + yVelocity;
      }
    }

    public void OnDestroy() {
      if (jump != null) {
        jump.performed -= Jump;
      }
    }
  }
}
