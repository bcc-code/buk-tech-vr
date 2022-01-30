using UnityEngine;

namespace Buk
{
  public class DebugPointable : MonoBehaviour, IPointable
  {
    public void PointerEnter(GameObject sender)
    {
      Debug.Log($"{name} knows it is being watched by {sender.name}");

    }
    public void PointerExit(GameObject sender)
    {
      Debug.Log($"{name} knows it is no longer being watched by {sender.name}");
    }
  }
}
