Shader "Hidden/SgtShadowLayer"
{
	Properties
	{
		_Shadow1Texture("Shadow 1 Texture", 2D) = "white" {}
		_Shadow1Ratio("Shadow 1 Ratio", Float) = 1
		_Shadow2Texture("Shadow 2 Texture", 2D) = "white" {}
		_Shadow2Ratio("Shadow 2 Ratio", Float) = 1
	}
	SubShader
	{
		Tags
		{
			"Queue"           = "Transparent"
			"RenderType"      = "Transparent"
			"IgnoreProjector" = "True"
		}
		Pass
		{
			Blend DstColor Zero, One One
			Cull Back
			Lighting Off
			ZWrite Off
			Offset -0.01, -0.01

			CGPROGRAM
				#include "SgtShadow.cginc"
				#pragma vertex Vert
				#pragma fragment Frag
				// Shadows
				#pragma multi_compile __ SHADOW_1 SHADOW_2
				
				struct a2v
				{
					float4 vertex : POSITION;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
#if SHADOW_1 || SHADOW_2
					float4 texcoord0 : TEXCOORD0; // wpos
#endif
				};

				struct f2g
				{
					float4 color : SV_TARGET;
				};

				void Vert(a2v i, out v2f o)
				{
					o.vertex = UnityObjectToClipPos(i.vertex);
#if SHADOW_1 || SHADOW_2
					o.texcoord0 = mul(unity_ObjectToWorld, i.vertex);
#endif
				}

				void Frag(v2f i, out f2g o)
				{
					o.color = float4(1.0f, 1.0f, 1.0f, 1.0f);
#if SHADOW_1 || SHADOW_2
					o.color.rgb *= lerp(UNITY_LIGHTMODEL_AMBIENT, o.color.rgb, ShadowColor(i.texcoord0).rgb);
#endif
				}
			ENDCG
		} // Pass
	} // SubShader
} // Shader