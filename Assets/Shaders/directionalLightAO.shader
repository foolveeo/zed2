Shader "Custom/directionalLightAO"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_NbrSkyLights("Number of Sky Lights", int) = 100
	}
		SubShader
	{
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			Blend One One


			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			// compile shader into multiple variants, with and without shadows
			// (we don't care about any lightmaps yet, so skip these variants)
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight  multi_compile_fwdadd_fullshadows
			// shadow helper functions and macros
			#include "AutoLight.cginc"

			struct v2f
			{
				SHADOW_COORDS(0) // put shadows data into TEXCOORD0
				float4 pos : SV_POSITION;
			};
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				// compute shadows data
				TRANSFER_SHADOW(o)
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
				fixed shadow = SHADOW_ATTENUATION(i);
				// darken light's illumination with shadow, keep ambient intact
				
				return float4(shadow, shadow, 0.0f, 1.0f);
			}
			ENDCG
		}


		Pass
			{
				Tags{ "LightMode" = "ForwardAdd" }
				Blend One One

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				#include "Lighting.cginc"

				#pragma multi_compile_fwdadd_fullshadows 
				// shadow helper functions and macros
				#include "AutoLight.cginc"

				struct v2f
				{
					SHADOW_COORDS(1) // put shadows data into TEXCOORD1
					float4 pos : SV_POSITION;
				};

				v2f vert(appdata_base v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					// compute shadows data
					TRANSFER_SHADOW(o)
					return o;
				}


				uniform int _NbrSkyLights;


				fixed4 frag(v2f i) : SV_Target
				{
					// compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
					fixed shadow = SHADOW_ATTENUATION(i);
					float shadowMultiplier = shadow / _NbrSkyLights;
					// darken light's illumination with shadow, keep ambient intact

					return float4(0.0f, 0.0f, shadowMultiplier, 1.0f);
				}
					
				ENDCG
				}

		// shadow casting support
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}