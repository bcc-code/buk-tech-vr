using System;
using System.Collections.Generic;
using Buk.PhysicsLogic.Interfaces;
using UnityEngine;

namespace Buk.PhysicsLogic.Implementation
{
  public class NewtonianGravityComparer : IComparer<INewtonianGravity>
  {
    public int Compare(INewtonianGravity a, INewtonianGravity b)
    {
      if (a.mass != b.mass)
      {
        // Default sort by mass
        return (int)Math.Round(a.mass - b.mass, MidpointRounding.AwayFromZero);
      }
      // For equal masses try gameobject instance identifier comparison for monostable sorting
      if (a is MonoBehaviour aBehaviour && b is MonoBehaviour bBehaviour) {
        return aBehaviour.gameObject.GetInstanceID() - bBehaviour.gameObject.GetInstanceID();
      }
      // Unsortable, will probably cause errors.
      return 0;
    }
  }
}
