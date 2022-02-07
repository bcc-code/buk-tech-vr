using System.Collections.Generic;
using Buk.PhysicsLogic.Interfaces;
using UnityEngine;

namespace Buk.PhysicsLogic.Implementation
{
  public class NewtonianGravityManager : MonoBehaviour, INewtonianGravityManager
  {
    private SortedList<INewtonianGravity, INewtonianGravity> gravityObjects = new SortedList<INewtonianGravity, INewtonianGravity>(new NewtonianGravityComparer());

    public void Awake()
    {
      CalculateGravities();
    }

    public void Update()
    {
      CalculateGravities();
      ApplyGravities();
    }

    public void Add(INewtonianGravity gravityObject)
    {
      gravityObjects.Add(gravityObject, gravityObject);
    }
    public void Remove(INewtonianGravity gravityObject)
    {
      gravityObjects.Remove(gravityObject);
    }
    public void CalculateGravities()
    {
      foreach (var gravityObject in gravityObjects.Values)
      {
        gravityObject.ClearGravity();
      }
      for (var i = 0; i < gravityObjects.Count; i++)
      {
        var objectA = gravityObjects.Values[i];
        for (var j = i + 1; j < gravityObjects.Count; j++)
        {
          var objectB = gravityObjects.Values[j];
          // Newtonian gravity with gravitational constant of 1
          var direction = objectA.position - objectB.position;
          var force = objectA.mass * objectB.mass / direction.sqrMagnitude / direction.magnitude;
          objectA.AddGravityComponent(-direction * force);
          objectB.AddGravityComponent(direction * force);
        }
      }
    }
    public void ApplyGravities()
    {
      foreach (var gravityObject in gravityObjects.Values)
      {
        gravityObject.ApplyGravity();
      }
    }
  }
}
