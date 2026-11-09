using UnityEngine;

public class Projectile : MonoBehaviour {
  private void OnCollisionEnter(Collision collision) {
    PaintManager.Instance.RegisterHit(collision.contacts[0].point, transform.localScale.x * 0.5f);
    Destroy(gameObject);
  }
}