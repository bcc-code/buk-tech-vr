using System.Linq;
using UnityEngine;

namespace Buk.AgeOfWar
{
  [RequireComponent(typeof(Team))]
  [RequireComponent(typeof(Rigidbody))]
  public class Enemy : MonoBehaviour
  {
    public float force = 100.0f;
    public float productionTime = 10.0f;
    public float maxSpeed = 15f;
    public Target target { get; set; }
    private Rigidbody body;
    private Team team;

    public void Awake()
    {
      body = GetComponent<Rigidbody>();
      team = GetComponent<Team>();
      target = FindObjectsOfType<Target>().Where(obj => obj.GetComponent<Team>().OtherTeam(team)).Aggregate(NearestTarget);
    }

    public void FixedUpdate()
    {
      if (target == null)
      {
        // Victory dance
        if (Physics.SphereCast(transform.position, 1.0f, Vector3.down, out var _, 0.1f)) {
          body.AddForce(Vector3.up * 3.0f, ForceMode.VelocityChange);
        };
      }
      else
      {
        // Charget the target
        transform.LookAt(target.transform);
        body.AddRelativeForce(0f, 0f, force);
        // Limit velocity
        var xzVelocity = new Vector3(body.velocity.x, 0, body.velocity.z);
        if (xzVelocity.magnitude > maxSpeed)
        {
          body.velocity = xzVelocity.normalized * maxSpeed + new Vector3(0, body.velocity.y, 0);
        }
      }
    }

    public void ApplyDifficulty(float difficulty)
    {
      productionTime /= difficulty;
      maxSpeed *= difficulty;
      force *= difficulty;
      if (TryGetComponent<TouchDamage>(out var touchDamage))
      {
        touchDamage.attack *= difficulty;
      }
    }

    private Target NearestTarget(Target a, Target b)
    => (a.transform.position - transform.position).sqrMagnitude <= (b.transform.position - transform.position).sqrMagnitude ? a : b;
  }
}
