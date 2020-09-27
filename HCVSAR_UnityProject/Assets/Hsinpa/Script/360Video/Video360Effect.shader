Shader "Hsinpa/Video360Effect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
		_MaskTex ("Mask", 2D) = "black" {}
		_NoiseTex ("Noise", 2D) = "black" {}

		[HDR]_BorderColor ("Border Color", Color) = (1,1,1,1)

		_Transition("Transition", Range(0,1)) = 0
		_Distort("Distort", Range(0, 0.2)) = 0
		_BorderRange("BorderRange", Range(0, 0.05)) = 0

    }
    SubShader
    {


		Tags 
		{ 
		    "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
		}

		ZWrite On
		//ZTest Always
		Cull off
		Lighting Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
			#include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinate			
            };

            struct v2f
            {
                float4 vertex : SV_POSITION; // clip space position
                float2 uv : TEXCOORD0; // texture coordinate
            };

            uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float3 _cameraPosition;
			
			sampler2D _MaskTex;
			sampler2D _NoiseTex;
			float4 _BorderColor;
			float _Transition;
			float _Distort;
			float _BorderRange;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
			}

			
			float RescaleNumber(float number, float min, float max) {
				return (number - min) / (max - min);
			}

			fixed4 ApplyTexTransition(float4 color, float mask) {
				fixed4 newColor = color;

				float diff = (_Transition - mask);

				if (_Transition == 1) {

					newColor.a = 0;
					
					return newColor;
				}


				if (mask < _Transition && diff < _BorderRange) {
				
					newColor *= _BorderColor;
					newColor.a = 1-lerp(0, 0.9, RescaleNumber(diff, 0, _BorderRange));

				} else if (mask < _Transition) {
					newColor.a = 0;
				}

				return newColor;
			}
            
            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv;
				uv = i.uv;
				
				fixed4 col = tex2D(_MainTex, uv);

				float2 distortUV = float2(sin(uv.x * _Time.y), sin(uv.y * _Time.x) ) * 0.1;
                fixed4 noiseTex = tex2D(_NoiseTex, uv + distortUV) * _Distort;
				float centerNoise = noiseTex.x * 2 - 1;

                fixed4 maskTex = tex2D(_MaskTex, uv + fixed2(centerNoise, centerNoise) );

                return ApplyTexTransition(col, maskTex.x);
            }
            ENDCG
        }
    }
}
