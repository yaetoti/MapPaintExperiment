Shader "Custom/Paint" {
  Properties {}

  SubShader {
    Tags {
      "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"
    }

    Pass {
      Cull Off
      
      HLSLPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
      
      struct VertexIn {
        float4 positionOS : POSITION;
        float2 uv : TEXCOORD0;
      };

      struct VertexOut {
        float4 positionHCS : SV_POSITION;
        float3 positionWS : WS_POSITION;
        float2 uv : TEXCOORD0;
      };
      
      struct HitData {
        float3 position;
        float size;
      };
      
      StructuredBuffer<HitData> hits;
      int hitsCount;

      VertexOut vert(VertexIn IN) {
        VertexOut OUT;
        // Screen position = UV
        float2 resultCoord = float2(IN.uv.x, 1 - IN.uv.y) * 2 - 1;
        OUT.positionHCS = float4(resultCoord, 0, 1);
        OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
        OUT.uv = IN.uv;
        return OUT;
      }

      half4 frag(VertexOut IN) : SV_Target {
        
        for (int i = 0; i < hitsCount; ++i) {
          HitData data = hits[i];
          if (length(IN.positionWS - data.position) < data.size) {
            return half4(1, 0, 0, 1);
          }
        }
        
        return half4(0, 0, 0, 0);
      }
      ENDHLSL
    }
  }
}