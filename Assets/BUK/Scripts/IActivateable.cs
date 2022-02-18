using UnityEngine;

namespace Buk
{
  public interface IActivateable
  {
    void ActivateStart(GameObject sender);
    void ActivateEnd(GameObject sender);
  }
}
