using UnityEngine;
using UnityEngine.SceneManagement;

namespace Buk.SceneNavigation
{
  public class TeleportToScene : MonoBehaviour, IActivateable {
    [Header("Scene name or path")]
    public string targetScene;

    public void ActivateStart(GameObject sender) {
      // Do nothing
    }

    public void ActivateEnd(GameObject sender) {
      if (!string.IsNullOrWhiteSpace(targetScene)) {
        SceneManager.LoadScene(targetScene);
      }
    }
  }
}
