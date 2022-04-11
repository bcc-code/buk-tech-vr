using UnityEngine;

namespace Buk.PhysicsLogic.Implementation
{
  [RequireComponent(typeof(Rigidbody))]
  public class Freezable : MonoBehaviour
  {
    private bool frozen = false;
    private float unFreezeTime = 0.0f;
    private RigidbodyConstraints originalConstraints;
    private Rigidbody body;

    public void Awake()
    {
      body = GetComponent<Rigidbody>();
    }

    public void Freeze(float time)
    {
      if (frozen)
      {
        unFreezeTime += time;
      }
      else
      {
        frozen = true;
        originalConstraints = body.constraints;
        body.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        unFreezeTime = Time.fixedTime + time;
      }
    }

    public void UnFreeze()
    {
      body.constraints = originalConstraints;
      frozen = false;
    }

    public void FixedUpdate()
    {
      if (frozen && Time.fixedTime > unFreezeTime)
      {
        UnFreeze();
      }
    }
  }
}
