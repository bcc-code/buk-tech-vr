using UnityEngine;

namespace Buk.AgeOfWar
{
  public class HealthBar : LookAtCamera
  {
    private Health health;
    private Renderer renderer;
    public void Start() {
      health = GetComponentInParent<Health>();
      renderer = GetComponent<Renderer>();
    }

    public void Update() {
      transform.localScale = new Vector3(health.AsFactor, transform.localScale.y, transform.localScale.y);
      renderer.enabled = health.AsFactor < 1f;
    }
  }
}
