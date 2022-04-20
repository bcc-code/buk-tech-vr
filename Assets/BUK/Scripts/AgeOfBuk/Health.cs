using UnityEngine;

namespace Buk.AgeOfBuk
{
  [RequireComponent(typeof(Team))]
  public class Health : MonoBehaviour
  {
    public float health = 1.0f;
    public float AsFactor { get => health / maxHealth; }
    private float maxHealth;

    public void Awake() {
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
