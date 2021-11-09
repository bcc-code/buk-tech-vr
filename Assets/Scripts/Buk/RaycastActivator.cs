using UnityEngine;
using UnityEngine.InputSystem;

namespace Buk
{
  /// <summary>
  /// Activate any IActivateable that this object is pointing at when the activateAction is performed
  /// </summary>
  public class RaycastActivator : MonoBehaviour
  {
    // Maximum raycast distance
    public float LimitDistance = 12f;

    private InputAction activateAction;

    public void Start()
    {
      if (activateAction != null)
      {
        activateAction.performed += ctx =>
        {
          if (Physics.Raycast(transform.position, transform.forward, out var detected, LimitDistance))
          {
            detected.collider.gameObject.GetComponent<IActivateable>()?.Activate(gameObject);
          }
        };
      }
    }
  }
}
