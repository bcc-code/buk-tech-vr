using UnityEngine;
using UnityEngine.InputSystem;

namespace Buk {
  public class FollowTarget : MonoBehaviour {
    public GameObject Target;

    public void Update() {
      gameObject.transform.position = Target.transform.position;
    }
  }
}
