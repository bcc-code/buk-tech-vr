using UnityEngine;

namespace Buk.AgeOfWar
{
  [RequireComponent(typeof(Rigidbody))]
  public class Health : MonoBehaviour
  {
    public float health = 1.0f;
    public float AsFactor { get => health / maxHealth; }
    private float maxHealth;

    public void Start() {
      maxHealth = health;
    }

    public void Hit(float points) {
      health -= points;
      if (health <= 0) {
        Destroy(gameObject);
      }
    }
  }
}
