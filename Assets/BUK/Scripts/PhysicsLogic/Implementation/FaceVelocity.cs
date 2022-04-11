using UnityEngine;

namespace Buk.PhysicsLogic.Implementation
{
  public class FaceVelocity : MonoBehaviour
  {
    private Rigidbody body;

    public void Awake() {
      body = GetComponentInChildren<Rigidbody>();
    }

    public void FixedUpdate() {
      if(body.velocity != Vector3.zero)
        transform.up = body.velocity.normalized;
    }

    public void OnCollisionEnter(Collision collisionTarget) {
      Destroy(this);
    }
  }
}
