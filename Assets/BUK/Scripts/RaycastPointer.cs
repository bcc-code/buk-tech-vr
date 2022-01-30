using System.Collections;
using UnityEngine;

namespace Buk
{
  /// <summary>
  /// Point at any IPointable GameObject
  /// </summary>
  public class RaycastPointer : MonoBehaviour
  {
    // Maximum raycast distance
    public float LimitDistance = 12f;

    // Current target
    private IPointable target = null;

    public void Update()
    {
      // Raycast
      if (Physics.Raycast(transform.position, transform.forward, out var detected, LimitDistance))
      {
        var newTarget = detected.collider.gameObject.GetComponentInParent<IPointable>();
        // Object detected
        if (newTarget != target)
        {
          // Notify old target (if any) that we're no longer watching
          target?.PointerExit(gameObject);
          target = newTarget;
          target?.PointerEnter(gameObject);
        }
      }
      else
      {
        Debug.Log("Found nothing");
        // Unregister old target (if any)
        target?.PointerExit(gameObject);
        target = null;
      }
    }
  }
}
