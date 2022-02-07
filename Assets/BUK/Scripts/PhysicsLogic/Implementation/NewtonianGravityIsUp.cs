using Buk.PhysicsLogic.Interfaces;
using Buk.PhysicsLogic.Extensions;
using UnityEngine;
using System;
using System.Linq;

namespace Buk.PhysicsLogic.Implementation
{
  public class NewtonianGravityIsUp : MonoBehaviour
  {
    private INewtonianGravity gravity;
    public GameObject gravityObject;
    public float smooth = 5;
    public void Awake() {
      gravity = gravityObject.GetComponent<INewtonianGravity>();
    }
    public void Update() {
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, -gravity.gravityVector), Time.deltaTime * smooth);
    }
  }
}
