using UnityEngine;

namespace Buk.SixDoF {
  public class ObeliskHover : MonoBehaviour {
    public Vector3 localOffset = new Vector3(0.0f, 0.0f, 0.0f);
    public GameObject target;
    public float moveSpeed = 10.0f;
    private Vector3 moveStartPosition;
    private Quaternion moveStartRotation;
    private Vector3 moveTargetPosition;
    private Quaternion moveTargetRotation;
    private float moveStartTime;
    private GameObject moveTo;

    public void Start() {
      if (target != null)
      MoveTo(target);
    }

    public void Update() {
      if (moveTo != null && moveTargetPosition != moveStartPosition) {
        var fraction = (Time.time - moveStartTime) * moveSpeed / (moveTargetPosition - moveStartPosition).magnitude ;
        gameObject.transform.position = Vector3.Lerp(moveStartPosition, moveTargetPosition, fraction);
        // TODO: Figure out how to only align rotation with the up-axis of the obelisk
        gameObject.transform.rotation = Quaternion.Slerp(moveStartRotation, moveTargetRotation, fraction);
      }
      if (gameObject.transform.position == moveTargetPosition) {
        target = moveTo;
        moveTo = null;
      }
    }

    public Vector3 HoverPosition(GameObject hoverTarget) {
      return hoverTarget.transform.position + hoverTarget.transform.rotation * hoverTarget.GetComponentInChildren<Hoverable>().transform.localPosition + localOffset;
    }

    public void MoveTo(GameObject newTarget) {
      moveTo = newTarget;
      moveTargetPosition = HoverPosition(moveTo);
      moveTargetRotation = Quaternion.LookRotation(transform.forward, moveTo.transform.forward);
      moveStartPosition = transform.position;
      moveStartRotation = transform.rotation;
      moveStartTime = Time.time;
    }
  }
}
