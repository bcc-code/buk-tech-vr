using UnityEngine;
using System;

namespace Buk.AgeOfWar
{
  [RequireComponent(typeof(Collider))]
  public class CollisionDamage : MonoBehaviour
  {
    public float minLethalVelocity = 5.0f;
    public float attack = 1.0f;
    public Vector3 directionality = new Vector3(1, 1, 1);
    public void OnCollisionEnter(Collision collisionTarget)
    {
      if (collisionTarget.gameObject.TryGetComponent<Health>(out var health))
      {
        health.Hit(Math.Max(Vector3.Scale(collisionTarget.relativeVelocity, directionality).sqrMagnitude, 0.0f));
      }
    }
  }
}
