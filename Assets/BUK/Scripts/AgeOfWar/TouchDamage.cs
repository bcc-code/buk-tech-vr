using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Buk.AgeOfWar
{
  public class TouchDamage : MonoBehaviour
  {
    public float attack = 1.0f;
    private Team team;

    private readonly IDictionary<GameObject, Health> enemiesTouching = new Dictionary<GameObject, Health>();

    public void Awake()
    {
      team = GetComponentInParent<Team>();
    }

    public void OnCollisionEnter(Collision collision)
    {
      var collisionTeam = collision.gameObject.GetComponentInParent<Team>();
      var otherHealth = collision.gameObject.GetComponentInParent<Health>();
      if (collisionTeam != null && team.OtherTeam(collisionTeam) && otherHealth != null)
      {
        enemiesTouching.Add(collision.gameObject, otherHealth);
        otherHealth.Hit(collision.impulse.sqrMagnitude);
      }
    }

    public void OnCollisionExit(Collision collision)
    {
      enemiesTouching.Remove(collision.gameObject);
    }

    public void FixedUpdate()
    {
      foreach (var kvp in enemiesTouching.Where(kvp => kvp.Key != null))
      {
        kvp.Value.Hit(attack);
      }
      foreach (var kvp in enemiesTouching.Where(kvp => kvp.Key == null).ToList())
      {
        enemiesTouching.Remove(kvp.Key);
      }
    }
  }
}
