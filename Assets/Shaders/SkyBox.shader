Shader "Custom/SpaceSkybox"
{
    Properties
    {
        _Tint ("Tint Color", Color) = (1,1,1,1)
        _StarTex ("Star Texture", 2D) = "white" {}
        _NebulaTex ("Nebula Texture", 2D) = "white" {}
        _StarIntensity ("Star Intensity", Range(0, 5)) = 1
        _NebulaIntensity ("Nebula Intensity", Range(0, 5)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Background" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex : POSITION;
            };
            
            struct v2f
            {
                float4 position : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            sampler2D _StarTex;
            sampler2D _NebulaTex;
            float4 _Tint;
            float _StarIntensity;
            float _NebulaIntensity;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.vertex.xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 dir = normalize(i.texcoord);
                
                // Nebula effect
                float4 nebulaColor = tex2D(_NebulaTex, dir.xy * 0.5 + 0.5) * _NebulaIntensity;

                // Star effect
                float4 starColor = tex2D(_StarTex, dir.xy * 10.0) * _StarIntensity;

                return _Tint * (nebulaColor + starColor);
            }
            ENDCG
        }
    }
    FallBack "RenderType/Background"
}
