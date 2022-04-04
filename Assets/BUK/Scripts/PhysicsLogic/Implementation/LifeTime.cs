using UnityEngine;

namespace Buk.PhysicsLogic.Implementation
{
  public class LifeTime : MonoBehaviour
  {
    public float lifeTime = 15f;

    public void Start()
    {
      // Destroy after lifeTime seconds
      Destroy(gameObject, lifeTime);
    }
  }
}
