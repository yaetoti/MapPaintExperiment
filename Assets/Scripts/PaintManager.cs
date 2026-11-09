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
    if (m_hitsCount != 0) {
      // Upload data
      m_hitsBuffer.SetData(m_hits, 0, 0, m_hitsCount);
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