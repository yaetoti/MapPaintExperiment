using UnityEngine;

public class Projectile : MonoBehaviour {
  private void OnCollisionEnter(Collision collision) {
    PaintManager.Instance.RegisterHit(collision.contacts[0].point, transform.localScale.x * 0.5f);
    //PaintManager.Instance.RegisterHit(collision.transform.position, transform.localScale.x * 0.5f + 0.1f);
    Destroy(gameObject);
  }
}