using UnityEngine;

namespace Buk.PhysicsLogic.Implementation
{
  public class SpawnOnDestroy : MonoBehaviour
  {
    public GameObject spawnType;
    public void OnDestroy() {
      Instantiate(spawnType, transform.position, transform.rotation);
    }
  }
}
