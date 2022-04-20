using UnityEngine;

namespace Buk.AgeOfBuk
{
  [RequireComponent(typeof(Team))]
  public class Target : MonoBehaviour {
    public void OnDestroy() {
      FindObjectOfType<AgeOfBukController>()?.TargetDied(GetComponent<Team>());
    }
  }
}
