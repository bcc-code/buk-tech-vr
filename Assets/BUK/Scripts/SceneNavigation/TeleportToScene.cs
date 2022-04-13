using UnityEngine;
using UnityEngine.SceneManagement;

namespace Buk.SceneNavigation
{
  public class TeleportToScene : MonoBehaviour, IActivateable {
    [Header("Scene name or path")]
    public string targetScene;
    public bool teleportOnActivateStart = false;

    public void ActivateStart(GameObject sender) {
      if (teleportOnActivateStart) {
        Teleport();
      }
    }

    public void ActivateEnd(GameObject sender) {
      if (!teleportOnActivateStart) {
        Teleport();
      }
    }

    public void Teleport() {
      if (!string.IsNullOrWhiteSpace(targetScene)) {
        SceneManager.LoadScene(targetScene);
      }
    }
  }
}
