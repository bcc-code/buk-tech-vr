using UnityEngine;
using UnityEngine.InputSystem;

namespace Buk {
  public class Roll : MonoBehaviour {

    private Rigidbody body;
    private float initialAngularDrag;

    public InputAction gas;
    public InputAction brake;
    public float torque = 2.0f;
    public Camera camera;
    public float maximumAngularVelocity = 25f;

    public void Awake() {
      body = GetComponent<Rigidbody>();
      body.maxAngularVelocity = maximumAngularVelocity;
      var initialAngularDrag = body.angularDrag;

      // Enable controls
      gas.Enable();
      brake.Enable();

      // Handle braking
      brake.performed += Brake;
      brake.canceled += Cancel;
    }

    // Increase drag when brake is pressed
    void Brake(InputAction.CallbackContext _) => body.angularDrag = initialAngularDrag * torque;

    // Reset drag when brake is released
    void Cancel(InputAction.CallbackContext _) => body.angularDrag = initialAngularDrag;

    public void Update() {
      // Read gas input
      var throttle = gas.ReadValue<float>();
      if (throttle != 0) {
        // Add rotational force to spin ball in the direction the camera is looking.
        body.AddTorque(camera.transform.right * throttle * torque, ForceMode.Force);
      }
    }

    public void OnDestroy() {
      brake.performed -= Brake;
      brake.canceled -= Cancel;
    }
  }
}
