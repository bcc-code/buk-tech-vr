using UnityEngine;
using System;

namespace Buk.PhysicsLogic.Interfaces
{
  public interface INewtonianGravity
  {
    Vector3 gravityVector { get; }
    Vector3 position { get; }
    float mass { get; }
    void ClearGravity();
    void AddGravityComponent(Vector3 forceComponent);
    void ApplyGravity();
  }
}
