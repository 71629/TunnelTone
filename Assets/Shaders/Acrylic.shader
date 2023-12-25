Shader "UI/AcrylicEffect"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        GrabPass
        {
            "_GrabTexture"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest Always

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1;
            };

            fixed4 _Color;
            sampler2D _MainTex;
            sampler2D _NoiseTex;
            sampler2D _GrabTexture;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                
                o.color = v.color * _Color;
                o.screenPos = ComputeScreenPos(o.vertex);
                o.screenPos.y = 1 - o.screenPos.y;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the noise texture
                float noise = tex2D(_NoiseTex, i.texcoord).r;
            
                // Use the noise value to distort the UV coordinates
                // Reduce the distortion strength to prevent inversion
                float2 distortedUV = i.texcoord + noise * 20;
            
                // Sample the main texture with the distorted UV coordinates
                fixed4 col = i.color * tex2D(_MainTex, distortedUV);
            
                // Sample the grabbed texture using the screen coordinates
                fixed4 grabCol = tex2Dproj(_GrabTexture, i.screenPos);
            
                // Apply a blur effect to the grabbed texture
                // Increase the blur radius for a stronger blur effect
                float2 offset = 1.5 / _ScreenParams.xy;
                grabCol = 0;
                _MainTex = _GrabTexture;
                for (int y = -18; y <= 18; y++)
                {
                    for (int x = -18; x <= 18; x++)
                    {
                        grabCol += tex2Dproj(_GrabTexture, i.screenPos + float4(offset.x * x, offset.y * y, 0, 0));
                    }
                }
                grabCol /= 1521;
                grabCol *= 0.55;
            
                // Combine the UI color and the blurred background
                col = lerp(grabCol, i.color * tex2D(_MainTex, distortedUV), 0);
                col.a = 1;
            
                return col;
            }
            ENDCG
        }
    }
}

/* I have no idea of what 7 I'm doing */