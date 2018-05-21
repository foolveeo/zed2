//Shader "Custom/HBAO"
//{
//	Properties
//	{
//		_MainTex("Main Texture", 2D) = "white" {}
//	_Depth("Depth Texture", 2D) = "white" {}
//	_Normals("Normal Texture", 2D) = "white" {}
//	_Noise("Noise Texture", 2D) = "white" {}
//	_Mask("Mask", 2D) = "white" {}
//	_Resolution("Resolution", Vector) = (0.00078125, 0.0013888888889, 0, 0)
//		_InvResolution("Inverse of Resolution", Vector) = (1280.0, 720.0, 0, 0)
//		_nbrSamplesPerDirection("Number of samples for each direction", int) = 1
//		_nbrDirections("Number of sampling direction", int) = 1
//		_maxRadiusPixel("Maximum radius of sampling", float) = 50
//		_rFar("distance between pixel position and sample position to cast occlusion", float) = 1
//	}
//		SubShader
//	{
//		// No culling or depth
//		Cull Off
//		ZWrite Off
//		ZTest Always
//
//
//		Pass
//	{
//		CGPROGRAM
//
//#define PI 3.14159265
//#pragma vertex vert
//#pragma fragment frag
//
//
//#include "UnityCG.cginc"
//
//		struct appdata
//	{
//		float4 vertex : POSITION;
//		float2 normal : NORMAL;
//		float2 uv : TEXCOORD0;
//	};
//
//	struct v2f
//	{
//		float2 uv : TEXCOORD0;
//		float4 vertex : SV_POSITION;
//	};
//
//	v2f vert(appdata v)
//	{
//		v2f o;
//		o.vertex = UnityObjectToClipPos(v.vertex);
//		o.uv = float2(v.uv.x, 1 - v.uv.y);
//		return o;
//	}
//
//	sampler2D _Mask;
//	sampler2D _Depth;
//	sampler2D _Normals;
//	sampler2D _Noise;
//	float4 _Resolution;
//	float4 _InvResolution;
//	int _nbrSamplesPerDirection;
//	int _nbrDirections;
//	float _maxRadiusPixel;
//	float _rFar;
//
//	float TanToSin(float x)
//	{
//		return 1 / sqrt(x*x + 1.0);
//	}
//
//	float3 get3DPosInViewCoord(float screen_x, float screen_y, float depth_buffer_value)
//	{
//		float4 temp = float4(screen_x, screen_y, depth_buffer_value, 1);
//		temp = mul(unity_CameraInvProjection, temp);
//		float3 camera_space = temp.xyz / temp.w;
//		return float3(camera_space.x, 1 - camera_space.y, 1 - depth_buffer_value);
//	}
//
//
//	float Tangent(float3 P, float3 S)
//	{
//		return -(P.z - S.z) * (1 / sqrt(pow(S.x - P.x, 2) + pow(S.y - P.x, 2)));
//	}
//
//	float Lenght2(float3 v)
//	{
//		return dot(v, v);
//	}
//
//	float3 minDiff(float3 p, float3 p_right, float3 p_left)
//	{
//		float3 v1 = p_right - p;
//		float3 v2 = p - p_left;
//		float lenghtV1 = sqrt(pow(v1.x, 2) + pow(v1.y, 2) + pow(v1.z, 2));
//		float lenghtV2 = sqrt(pow(v2.x, 2) + pow(v2.y, 2) + pow(v2.z, 2));
//		if (lenghtV1 < lenghtV2)
//		{
//			return v1;
//		}
//		else
//		{
//			return v2;
//		}
//	}
//
//
//	float2 rotateSampleDirection(float2 dirVec, float2 noise)
//	{
//		return float2(dirVec.x, dirVec.y);
//	}
//
//	float BiasedTangent(float3 v)
//	{
//		return v.z * 1 / (sqrt(dot(v, v)));
//	}
//
//	float Falloff(float d)
//	{
//		return d * (-(1 / d)) + 1.0;
//	}
//
//
//	float computeSingleOcclusion(float2 deltaUV, float2 samplingDirection, float tanH, float sinH, float2 pixelImageSpacePos, float sampleRadiusStep, float3 pixelViewSpacePos)
//	{
//		float singleOcclusion = 0;
//		[unroll(606)]
//		for (int _sample = 1; _sample < _nbrSamplesPerDirection + 1; _sample++)
//		{
//			deltaUV += deltaUV;
//			float sampleRadius = _sample * sampleRadiusStep;// +tex2D(_Noise, float2(maxRadiusUV * dir, maxRadiusUV * _sample);
//			float2 sampleDisplacement = float2(samplingDirection.x * sampleRadius, samplingDirection.y * sampleRadius);
//
//			float2 sampleImageSpacePos = float2(pixelImageSpacePos.x + sampleDisplacement.x, pixelImageSpacePos.y + sampleDisplacement.y);
//
//			float sampleDepth = tex2D(_Depth, sampleImageSpacePos);
//
//			float3 sampleViewSpacePos = get3DPosInViewCoord(sampleImageSpacePos.x, sampleImageSpacePos.y, sampleDepth);
//
//			float tanS = Tangent(pixelViewSpacePos, sampleViewSpacePos);
//			float d2 = Lenght2(pixelViewSpacePos - sampleViewSpacePos);
//
//			if (d2 < _rFar && tanS > tanH)
//			{
//				float sinS = TanToSin(tanS);
//				singleOcclusion = sinS - sinH;
//
//				tanH = tanS;
//				sinH = sinS;
//
//			}
//		}
//		return singleOcclusion;
//	}
//
//	float4 frag(v2f i) : SV_Target
//	{
//		if (tex2D(_Mask, i.uv).x)
//		{
//			float pixelDepth = tex2D(_Depth, i.uv);
//			float3 pixelNormal = tex2D(_Normals, i.uv);
//
//
//
//
//			float2 pixelImageSpacePos = float2(i.uv.x, i.uv.y);
//			float3 pixelViewSpacePos = get3DPosInViewCoord(pixelImageSpacePos.x, pixelImageSpacePos.y, pixelDepth);
//
//			// neighbour pixels of P
//			float2 rightPixelImageSpacePos = float2(pixelImageSpacePos.x + _InvResolution.x, 0);
//			float2 leftPixelImageSpacePos = float2(pixelImageSpacePos.x - _InvResolution.x, 0);
//			float2 topPixelImageSpacePos = float2(0, pixelImageSpacePos.y + _InvResolution.y);
//			float2 bottomPixelImageSpacePos = float2(0, pixelImageSpacePos.y - _InvResolution.y);
//
//			float rightPixelDepth = tex2D(_Depth, rightPixelImageSpacePos);
//			float leftPixelDepth = tex2D(_Depth, leftPixelImageSpacePos);
//			float topPixelDepth = tex2D(_Depth, topPixelImageSpacePos);
//			float bottomPixelDepth = tex2D(_Depth, bottomPixelImageSpacePos);
//
//			float3 rightPixelViewSpacePos = get3DPosInViewCoord(rightPixelImageSpacePos.x, rightPixelImageSpacePos.y, rightPixelDepth);
//			float3 leftPixelViewSpacePos = get3DPosInViewCoord(leftPixelImageSpacePos.x, leftPixelImageSpacePos.y, leftPixelDepth);
//			float3 topPixelViewSpacePos = get3DPosInViewCoord(topPixelImageSpacePos.x, topPixelImageSpacePos.y, topPixelDepth);
//			float3 bottomPixelViewSpacePos = get3DPosInViewCoord(bottomPixelImageSpacePos.x, bottomPixelImageSpacePos.y, bottomPixelDepth);
//
//
//			// compute tangent basis vector
//
//			float3 dPdu = minDiff(pixelViewSpacePos, rightPixelViewSpacePos, leftPixelViewSpacePos);
//			float3 dPdv = minDiff(pixelViewSpacePos, topPixelViewSpacePos, bottomPixelViewSpacePos) * (_Resolution.y * _InvResolution.x);
//
//			float3 randomValue = tex2D(_Noise, i.uv);
//
//			float2 samplingDirection = float2(1.0, 0.0);
//			float samplingRadius;
//			float2 sampleDisplacement;
//			float2 sampleImageSpacePos;
//			float3 sampleViewSpacePos;
//			float sampleDepth;
//
//			float singleOcclusion;
//			float totalOcclusion = 0;
//
//			float maxRadiusUV = _maxRadiusPixel * ((_InvResolution.x + _InvResolution.y) / 2);
//			float sampleRadiusStep = maxRadiusUV / _nbrSamplesPerDirection;
//
//			float alpha = 2.0 * PI / _nbrDirections;
//
//			[unroll(10000)]
//			for (int dir = 0; dir < _nbrDirections; dir++)
//			{
//				float theta = alpha * dir;
//
//				//samplingDirection = rotateSampleDirection( float2(cos(theta), sin(theta)), tex2D(_Noise, float2(theta * dir, maxRadiusUV * dir).xy);
//				samplingDirection = float2(cos(theta), sin(theta));
//				float2 deltaUV = samplingDirection * sampleRadiusStep;
//				float3 tangentVector = deltaUV.x * dPdu + deltaUV.y * dPdv;
//				float tanH = BiasedTangent(tangentVector);
//				float sinH = TanToSin(tanH);
//
//				float singleOcclusion = 0;
//
//				singleOcclusion = computeSingleOcclusion(deltaUV, samplingDirection, tanH, sinH, pixelImageSpacePos, sampleRadiusStep, pixelViewSpacePos);
//
//				totalOcclusion += singleOcclusion;
//				//rotateSampleDir(sammpleDir);
//			}
//
//			totalOcclusion /= (_nbrDirections);
//			return float4(totalOcclusion, totalOcclusion, totalOcclusion, 1);
//		}
//		else
//		{
//			return float4(1, 0, 0, 0);
//		}
//	}
//
//		ENDCG
//	}
//	}
//}
