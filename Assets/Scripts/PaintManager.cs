using System;
using System.Runtime.InteropServices;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class PaintManager : MonoBehaviour {
  private struct HitData {
    public Vector3 position;
    public float size;
  }

  public const int MAX_HITS = 16;
  
  public static PaintManager Instance { get; private set; } = null;

  // Inspector
  public Transform player;
  public GameObject projectilePrefab;
  public float projectileRadius = 0.1f;
  public float projectileVelocity = 10f;
  
  // Data
  private readonly HitData[] m_hits = new HitData[MAX_HITS];
  private int m_hitsCount = 0;
  private GraphicsBuffer m_hitsBuffer;
  
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
  private static void ResetStatics() {
    Instance = null;
  }

  private void Awake() {
    if (Instance == this) {
      return;
    }

    if (Instance is not null) {
      Destroy(this);
      return;
    }

    Instance = this;
    
    // Init
    m_hitsBuffer = new(GraphicsBuffer.Target.Structured, MAX_HITS, Marshal.SizeOf<HitData>());
  }

  private void OnDestroy() {
    if (Instance != this) {
      return;
    }

    Instance = null;
    
    // Cleanup
    m_hitsBuffer?.Dispose();
    m_hitsBuffer = null;
  }

  private void Update() {
    // Upload buffer data
    if (m_hitsCount != 0) {
      // Upload data
      m_hitsBuffer.SetData(m_hits, 0, 0, m_hitsCount);
    }
    
    // Update paint gun
    if (Input.GetMouseButtonDown(0)) {
      GameObject projectile = Instantiate(projectilePrefab, player.position, Quaternion.identity);
      projectile.transform.localScale = projectileRadius * 2f * Vector3.one;
      Rigidbody rb = projectile.GetComponent<Rigidbody>();
      if (rb != null) {
        rb.linearVelocity = player.forward * projectileVelocity;
      }
    }
  }

  private void LateUpdate() {
    // Clear hit data
    m_hitsCount = 0;
  }

  public void RegisterHit(Vector3 position, float size) {
    if (m_hitsCount >= MAX_HITS) {
      // Debug.Log("SKIP");
      return;
    }

    m_hits[m_hitsCount] = new() {
      position = position,
      size = size
    };
    m_hitsCount++;
  }

  public int GetHitsCount() {
    return m_hitsCount;
  }

  public GraphicsBuffer GetHitsBuffer() {
    return m_hitsBuffer;
  }
}