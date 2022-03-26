using UnityEngine;

namespace Buk
{
  ///<summary> Door that opens slightly when you point at it.
  public class DoorAjar : MonoBehaviour, IPointable
  {
    private Quaternion originalRotation;
    private Quaternion fromRotation;
    public Vector3 openRotation = new Vector3(0.0f, 5.0f, 0.0f);
    public float speed = 1.0f;
    private float stateChangeTime = 0.0f;
    private DoorState doorState = DoorState.Closed;
    private DoorState state
    {
      get => doorState;
      set
      {
        doorState = value;
        stateChangeTime = Time.fixedTime;
        fromRotation = transform.localRotation;
      }
    }

    public void Awake()
    {
      originalRotation = transform.localRotation;
    }

    public void PointerEnter(GameObject sender)
    {
      state = DoorState.Opening;
    }

    public void PointerExit(GameObject sender)
    {
      state = DoorState.Closing;
    }

    public void Update()
    {
      var fraction = Time.fixedTime - stateChangeTime * speed;
      switch (state)
      {
        case DoorState.Opening:
          transform.localRotation = Quaternion.Slerp(fromRotation, originalRotation * Quaternion.Euler(openRotation), fraction);
          if (fraction >= 1.0)
          {
            state = DoorState.Ajar;
          }
          break;
        case DoorState.Closing:
          transform.localRotation = Quaternion.Slerp(fromRotation, originalRotation, fraction);
          if (fraction >= 1.0)
          {
            state = DoorState.Closed;
            // To avoid accumulation of small errors;
            transform.localRotation = originalRotation;
          }
          break;
      }
    }

    private enum DoorState
    {
      Closed,
      Opening,
      Ajar,
      Closing
    }
  }
}
