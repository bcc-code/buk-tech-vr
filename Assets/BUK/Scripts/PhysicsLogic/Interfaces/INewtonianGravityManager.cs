using UnityEngine;

namespace Buk.PhysicsLogic.Interfaces
{
  public interface INewtonianGravityManager
  {
    public void Add(INewtonianGravity gravityObject);
    public void Remove (INewtonianGravity gravityObject);
    void CalculateGravities();
    void ApplyGravities();
  }
}
