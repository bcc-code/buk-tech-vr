using UnityEngine;

namespace Buk.SixDoF
{
  public class ActivateableObelisk : MonoBehaviour, IActivateable, IPointable
  {
    private Material material = null;
    private Light light = null;

    public void Start()
    {
      light = GetComponentInChildren<Light>();
      material = GetComponentInChildren<Renderer>().material;
      Disable();
    }

    public void ActivateStart(GameObject sender)
    {
      // TODO: Draw line?
    }
    public void ActivateEnd(GameObject sender)
    {
      sender.GetComponentInParent<ObeliskHover>()?.MoveTo(gameObject);
    }

    public void PointerEnter(GameObject sender)
    {
      Enable();
    }

    public void PointerExit(GameObject sender)
    {
      Disable();
    }

    private void Enable()
    {
      material.SetColor("_EmissionColor", Color.white);
      light.enabled = true;

    }

    private void Disable()
    {
      material.SetColor("_EmissionColor", Color.black);
      light.enabled = false;
    }
  }
}
