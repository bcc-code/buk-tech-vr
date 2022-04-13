using UnityEngine;

namespace Buk
{
  public class LookAtCamera : MonoBehaviour
  {
    /// <summary>
    /// Use LateUpdate for pointing the object toward the player's camera after any camera movements
    /// </summary>
    void LateUpdate()
    {
      if (Camera.current != null)
      {
        transform.LookAt(Camera.current.transform);
        transform.rotation = Quaternion.LookRotation(Camera.current.transform.forward);
      }
    }
  }
}
