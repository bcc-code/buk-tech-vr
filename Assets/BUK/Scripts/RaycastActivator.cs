using System.Collections.Generic;
using System.Linq;
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
    [Header("Maximum raycast distance")]
    public float LimitDistance = 12f;
    public InputAction activateAction;
    private IEnumerable<IActivateable> currentTarget;

    public void Start()
    {
      if (activateAction != null)
      {
        activateAction.started += ctx =>
        {
          if (TryDetectTarget(out currentTarget))
          {
            foreach (var activator in currentTarget) {
              activator.ActivateStart(gameObject);
            }
          }
        };
        activateAction.performed += ctx =>
        {
          if (currentTarget != null)
          {
            foreach (var activator in currentTarget) {
              activator.ActivateEnd(gameObject);
            }
          }
        };
        activateAction.Enable();
      }
    }

    private bool TryDetectTarget(out IEnumerable<IActivateable> targets)
    {
      if (Physics.Raycast(transform.position, transform.forward, out var detected, LimitDistance))
      {
        targets = detected.collider.gameObject.GetComponentsInParent(typeof(Component))
          .SelectMany(component => component is IActivateable activateable
            ? new [] { activateable }
            : Enumerable.Empty<IActivateable>());
      }
      else
      {
        targets = Enumerable.Empty<IActivateable>();
      }
      return targets.Any();
    }
  }
}
