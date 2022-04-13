using UnityEngine;

namespace Buk.AgeOfWar
{
  public class HealthBar : MonoBehaviour
  {
    public Color color = Color.red;
    private Health health;
    private Renderer renderer;
    public void Awake() {
      health = GetComponentInParent<Health>();
      renderer = GetComponent<Renderer>();
      renderer.material.color = color;
      transform.localScale.Scale(new Vector3(transform.parent.gameObject.GetComponentInChildren<Collider>().bounds.size.x, 1, 1));
    }

    public void Update() {
      transform.localScale = new Vector3(health.AsFactor, transform.localScale.y, transform.localScale.y);
      renderer.enabled = health.AsFactor < 1f;
    }
  }
}
