using UnityEngine;
using UnityEngine.InputSystem;

namespace Buk {
  public class Roll : MonoBehaviour {

    private Rigidbody body;
    private float initialAngularDrag;

    public InputAction Gas;
    public InputAction Brake;
    public float Torque = 2.0f;
    public Camera camera;
    public float maximumAngularVelocity = 25f;

    public void Awake() {
      body = GetComponent<Rigidbody>();
      body.maxAngularVelocity = maximumAngularVelocity;
      var initialAngularDrag = body.angularDrag;

      // Enable controls
      Gas.Enable();
      Brake.Enable();

      // Handle braking
      // Increase drag when brake is pressed
      Brake.performed += (_) => body.angularDrag = initialAngularDrag * Torque;
      // Reset drag when brake is released
      Brake.canceled += (_) => body.angularDrag = initialAngularDrag;
    }

    public void Update() {
      // Read gas input
      var gas = Gas.ReadValue<float>();
      if (gas != 0) {
        // Add rotational force to spin ball in the direction the camera is looking.
        body.AddTorque(camera.transform.right * gas * Torque, ForceMode.Force);
      }
    }
  }
}
