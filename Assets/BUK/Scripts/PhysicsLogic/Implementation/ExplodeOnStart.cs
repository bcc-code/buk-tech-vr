using UnityEngine;

namespace Buk.PhysicsLogic.Implementation
{
  [RequireComponent(typeof(Rigidbody))]
  public class ExplodeOnStart : MonoBehaviour
  {
    public float explosionForce = 5.0f;
    public float explosionRadius = 15.0f;
    public float explosionLift = 0.0f;
    public float explosionDuration = 1.0f;
    public ForceMode forceMode = ForceMode.Force;
    public void Start() {
      Destroy(gameObject, explosionDuration);
      foreach (var hit in Physics.OverlapSphere(transform.position, explosionRadius)) 
        hit.attachedRigidbody?.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionLift);
    }
  }
}
