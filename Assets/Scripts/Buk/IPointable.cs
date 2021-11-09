using UnityEngine;

namespace Buk
{
  public interface IPointable
  {
    public void PointerEnter(GameObject sender);
    public void PointerExit(GameObject sender);
  }
}
