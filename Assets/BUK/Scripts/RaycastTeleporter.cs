using UnityEngine;
using UnityEngine.InputSystem;

namespace Buk
{
  /// <summary>
  /// Teleport to the object this object is pointing at when the activateAction is performed
  /// </summary>
  public class RaycastTeleporter : MonoBehaviour
  {
    // Maximum raycast distance
    public float LimitDistance = 12f;
    // InputAction to perform to teleport
    public InputAction activateAction;
    // The object to teleport. By default the object that this behaviour is attached to.
    public GameObject teleportObject;
    // The teleportation offset from the point clicked.
    public Vector3 teleportOffset = new Vector3(0, 0, 0);

    public void Start()
    {
      if (teleportObject == null)
      {
        teleportObject = gameObject;
      }
      if (activateAction != null)
      {
        activateAction.performed += TryTeleport;
      }
    }
    
    public void TryTeleport(InputAction.CallbackContext _)
    {
      if (Physics.Raycast(transform.position, transform.forward, out var detected, LimitDistance))
      {
        teleportObject.transform.position = detected.transform.position;
      }
    }

    public void OnDestroy()
    {
      if (activateAction != null)
      {
        activateAction.performed -= TryTeleport;
      }
    }
  }
}
