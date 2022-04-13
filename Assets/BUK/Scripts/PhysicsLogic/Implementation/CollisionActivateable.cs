using UnityEngine;

namespace Buk.PhysicsLogic.Implementation
{
  [RequireComponent(typeof(Collider))]
  public class CollisionActivateable : MonoBehaviour
  {
    private IActivateable activateable;
    public void Awake() {
      activateable = GetComponent<IActivateable>();
    }
    public void OnCollisionEnter(Collision collision)
    {
      activateable.ActivateStart(collision.gameObject);
    }
    public void OnCollisionExit(Collision collision)
    {
      activateable.ActivateEnd(collision.gameObject);
    }
  }
}
