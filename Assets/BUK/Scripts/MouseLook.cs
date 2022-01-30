using UnityEngine;
using UnityEngine.InputSystem;

namespace Buk
{
  // Add this behaviour on a player object that is parent to a camera.
  public class MouseLook : MonoBehaviour
  {
    // Quaternion that represents straight forward.
    private static Quaternion down = Quaternion.Euler(90, 0, 0);
    // Quaternion that represents straight down.
    private static Quaternion forward = Quaternion.Euler(0, 0, 0);

    public InputAction mouseLook;

    // Mouse sensitivity
    public Vector2 sensitivity = new Vector2(1f, 1f);
    // The camera for the player.
    public new Camera camera;
    // Maximum angle that you can look down
    public float lookDownLimit = 90f;
    // Maximum angle that you can look up
    public float lookUpLimit = 90f;

    public void Awake() {
      this.mouseLook.performed += Look;
      this.mouseLook.Enable();
    }

    public void Look(InputAction.CallbackContext context)
    {
      var delta = context.ReadValue<Vector2>();
      if (delta != Vector2.zero) {
        this.transform.rotation *= Quaternion.AngleAxis(delta.x * sensitivity.x, Vector3.up);
        if (this.camera != null) {
          this.camera.transform.localRotation *= Quaternion.AngleAxis(delta.y * sensitivity.y, Vector3.left);
          // The angle between where we're looking and straight forward.
          // Since the camera only rotates over its local x-axis, this is on that axis.
          var offForwardAngle = Quaternion.Angle(this.camera.transform.localRotation, forward);
          var offDownAngle = Quaternion.Angle(this.camera.transform.localRotation, down);
          if (offDownAngle < 90) {
            // Looking down
            if (offForwardAngle > lookDownLimit) {
              // Looking down from the horizon is positive degrees starting from 0, easy.
              this.camera.transform.localRotation = Quaternion.Euler(lookDownLimit, 0, 0);
            }
          } else {
            // Looking up
            if (offForwardAngle > lookUpLimit) {
              // Looking from the horizon to the zenith is 360 degrees to 270 degrees. Hence 360 - lookUpLimit
              this.camera.transform.localRotation = Quaternion.Euler(360 - lookUpLimit, 0, 0);
            }
          }
        }
      }
    }
  }
}
