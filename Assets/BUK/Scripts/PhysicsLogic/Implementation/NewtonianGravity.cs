using Buk.PhysicsLogic.Interfaces;
using Buk.PhysicsLogic.Extensions;
using UnityEngine;
using System;
using System.Linq;

namespace Buk.PhysicsLogic.Implementation
{
  [RequireComponent(typeof(Rigidbody))]
  public class NewtonianGravity : MonoBehaviour, INewtonianGravity
  {
    private Rigidbody body;
    public Vector3 initialVelocity;
    public Vector3 gravityVector { get; private set; } = Vector3.zero;
    public Vector3 position { get => transform.position; }
    public float gravitationalConstant = 1;
    [Header("Set to auto-calculate mass")]
    public float density = 0;
    public float mass
    {
      get => body?.mass ?? 0;
      set
      {
        if (body != null)
        {
          body.mass = value;
        }
      }
    }

    public void Awake()
    {
      // Register for gravity calculations
      FindObjectsOfType<MonoBehaviour>().OfType<INewtonianGravityManager>().Single().Add(this);
      body = GetComponent<Rigidbody>();
      body.velocity = initialVelocity;
      if (density != 0) {
        mass = density * GetComponent<MeshFilter>().sharedMesh?.Volume(transform.localScale) ?? throw new ApplicationException("Cannot calculate mass from density if no mesh is present");
      }
    }

    public void OnDestroy() {
      // Deregister from gravity calculations
      FindObjectsOfType<MonoBehaviour>().OfType<INewtonianGravityManager>().Single().Remove(this);
    }

    public void ClearGravity()
    {
      gravityVector = Vector3.zero;
    }
    public void AddGravityComponent(Vector3 forceComponent)
    {
      gravityVector += forceComponent;
    }
    public void ApplyGravity()
    {
      body.AddForce(gravityVector * gravitationalConstant);
    }
  }
}
