using UnityEngine;

namespace Buk.PhysicsLogic.Implementation
{
  [RequireComponent(typeof(Rigidbody))]
  public class CollisionDestroyer : MonoBehaviour
  {
    public float pierce = 1;
    public float minLethalVelocity = 5.0f;
    private Rigidbody body;
    public void Start()
    {
      body = gameObject.GetComponent<Rigidbody>();
    }
    public void OnCollisionEnter(Collision collisionTarget)
    {
      if (collisionTarget.gameObject.CompareTag("destructible") && (body.velocity.magnitude - (collisionTarget.body as Rigidbody)?.velocity.magnitude ?? 0.0f)>= minLethalVelocity)
      {
        Destroy(collisionTarget.gameObject);
        pierce--;
      }
      if (pierce == 0)
      {
        Destroy(gameObject);
      }
    }
  }
}
