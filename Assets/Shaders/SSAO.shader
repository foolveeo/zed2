Shader "Custom/SSAO"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {} // constaining the screen pixel texture
		_Depth("Depth Texture", 2D) = "white" {}
		_Normals("Normal Texture", 2D) = "white" {}
		_Noise("Noise Texture", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_Resolution("Resolution", Vector) = (1280.0, 720.0, 0.00078125, 0.0013888888889)	// needs to be set from script accordigly to zed resolution, it is accessiblle from  SDK. first two compionent are resolution x and resolution y, last two are the inverse of the resoltion
		_nbrSamplesPerDirection("Number of samples for each direction", int) = 1   // adjusted manually
		_nbrDirections("Number of sampling direction", int) = 1	 // adjusted manually
		_maxRadiusPixel("Maximum radius of sampling", float) = 50 // adjusted manually, maybe can be set with respect to the resolution values
		_rFar("distance between pixel position and sample position to cast occlusion", float) = 1 // adjusted manually
	}

	SubShader
	{
		// No culling or depth
		Cull Off
		ZWrite Off
		ZTest Always


		Pass
	{
		CGPROGRAM

		#define PI 3.14159265
		#pragma vertex vert
		#pragma fragment frag


		#include "UnityCG.cginc"

		struct appdata
		{
		float4 vertex : POSITION;
		float2 normal : NORMAL;
		float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
		};

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = float2(v.uv.x, 1 - v.uv.y);
			return o;
		}

		sampler2D _Mask;
		sampler2D _Depth;
		sampler2D _Normals;
		sampler2D _Noise;
		float4 _Resolution;
		float4 _InvResolution;
		int _nbrSamplesPerDirection;
		int _nbrDirections;
		float _maxRadiusPixel;
		float _rFar;


		float3 get3DPosInViewCoord(float screen_x, float screen_y, float depth_buffer_value)
		{
			float4 temp = float4(screen_x, screen_y, depth_buffer_value, 1);
			temp = mul(unity_CameraInvProjection, temp);
			float3 camera_space = temp.xyz / temp.w;
			return float3(camera_space.x, 1 - camera_space.y, 1 - depth_buffer_value);
		}



		float lenght(float3 v)
		{
			return dot(v, v);
		}

		float4 frag(v2f i) : SV_Target
		{
			if (tex2D(_Mask, i.uv).x)
			{
				float pixelDepth = tex2D(_Depth, i.uv);
				float3 pixelNormal = tex2D(_Normals, i.uv);
				float4 randomValue = tex2D(_Noise, i.uv);		// we can set up 4 random values, we could use them for the entire process

				float2 pixelImageSpacePos = float2(i.uv.x, i.uv.y);
				float3 pixelViewSpacePos = get3DPosInViewCoord(pixelImageSpacePos.x, pixelImageSpacePos.y, pixelDepth);



				return float4(pixelNormal.x, pixelNormal.y, pixelNormal.z, 1);

			}

		return float4(1, 0, 0, 1);
		
		}

			ENDCG
		}
	}
}