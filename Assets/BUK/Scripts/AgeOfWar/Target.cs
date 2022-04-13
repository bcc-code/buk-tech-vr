using UnityEngine;

namespace Buk.AgeOfWar
{
  [RequireComponent(typeof(Team))]
  public class Target : MonoBehaviour {
    void OnDestroy() {
      FindObjectOfType<AgeOfWarController>().TargetDied(GetComponent<Team>());
    }
  }
}
