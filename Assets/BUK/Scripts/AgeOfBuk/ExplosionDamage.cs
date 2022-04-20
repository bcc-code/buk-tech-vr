using UnityEngine;

namespace Buk.AgeOfBuk
{
  [RequireComponent(typeof(Rigidbody))]
  public class ExplosionDamage : MonoBehaviour
  {
    public float explosionForce = 5.0f;
    public float explosionDamage = 0.0f;
    public float explosionRadius = 15.0f;
    public float explosionLift = 0.0f;
    public float explosionDuration = 1.0f;
    public ForceMode forceMode = ForceMode.Force;
    public void Start() {
      Destroy(gameObject, explosionDuration);
      foreach (var hit in Physics.OverlapSphere(transform.position, explosionRadius)) {
        hit.attachedRigidbody?.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionLift);
        if (explosionDamage > 0f && hit.TryGetComponent<Health>(out var health) == true) {
          health.Hit(explosionDamage * explosionRadius / (hit.gameObject.transform.position - transform.position).sqrMagnitude);
        }
      }
    }
  }
}
