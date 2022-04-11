using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public float force = 100.0f;
    public float productionTime = 10.0f;
    public float maxSpeed = 15f;
    public GameObject target { get; set; }
    private Rigidbody body;

    public void Awake()
    {
        body = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectsWithTag("enemyTarget").Aggregate(Nearest);
    }

    public void FixedUpdate()
    {
        transform.LookAt(target.transform);
        body.AddRelativeForce(0f, 0f, force);
        // Limit velocity
        var xzVelocity = new Vector3(body.velocity.x, 0, body.velocity.z);
        if (xzVelocity.magnitude > maxSpeed) {
          body.velocity = xzVelocity.normalized * maxSpeed + new Vector3(0, body.velocity.y, 0);
        }
    }

    public void ApplyDifficulty(float difficulty) {
        productionTime /= difficulty;
        maxSpeed *= difficulty;
        force *= difficulty;
    }

    private GameObject Nearest(GameObject a, GameObject b)
    => (a.transform.position - transform.position).sqrMagnitude <= (b.transform.position - transform.position).sqrMagnitude ? a : b;
}
