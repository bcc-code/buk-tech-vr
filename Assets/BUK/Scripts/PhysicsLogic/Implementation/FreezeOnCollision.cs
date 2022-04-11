using UnityEngine;

namespace Buk.PhysicsLogic.Implementation
{
  [RequireComponent(typeof(Collider))]
  public class FreezeOnCollision : MonoBehaviour
  {
    public int bounce = 1;
    public float freezeTime = 5f;

    public void OnCollisionEnter(Collision collisionTarget) {
      var freezable = collisionTarget.gameObject.GetComponentInChildren<Freezable>();
      if (freezable != null) {
        freezable.Freeze(freezeTime);
      } else {
        GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        Destroy(this);
      }
    }
  }
}
