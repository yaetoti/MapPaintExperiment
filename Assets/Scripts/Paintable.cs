using System;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Paintable : MonoBehaviour {
  private static readonly int PaintTexture = Shader.PropertyToID("_PaintTexture");
  
  // Inspector
  public RenderTexture texture;

  // Components
  private MeshFilter m_meshFilter;
  private MeshRenderer m_meshRenderer;
  
  // Data
  private Material m_paintMaterial; 
  private CommandBuffer m_cmd; 
  
  private void Awake() {
    m_meshFilter = GetComponent<MeshFilter>();
    m_meshRenderer = GetComponent<MeshRenderer>();
    
    // Setup
    m_cmd = new();
    var paintShader = Shader.Find("Custom/Paint");
    m_paintMaterial = new(paintShader);
    
    // Clear target and draw mesh
    m_cmd.SetRenderTarget(texture);
    m_cmd.ClearRenderTarget(false, true, new Color(0, 0, 0, 0));
    m_cmd.DrawMesh(m_meshFilter.mesh, Matrix4x4.identity, m_paintMaterial);
    
    Graphics.ExecuteCommandBuffer(m_cmd);
    m_cmd.Clear();
    
    // Set material texture
    m_meshRenderer.material = new(m_meshRenderer.material);
    m_meshRenderer.material.SetTexture(PaintTexture, texture);
  }

  private void Update() {
  }
}