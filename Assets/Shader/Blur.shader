Shader "Hidden/Blur"
{
	Properties
	{
		_Radius("Radius", Range(1, 255)) = 1
	}

		Category
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Opaque" }

		SubShader
		{
			GrabPass
			{
				Tags{ "LightMode" = "Always" }
			}

			Pass
			{
				Tags{ "LightMode" = "Always" }

				HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

				#pragma multi_compile_instancing

				CBUFFER_START(UnityPerMaterial)
				
				float _Radius;

				CBUFFER_END

				struct appdata
				{
					float4 vertex : POSITION;
					float2 texcoord: TEXCOORD0;

					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f
				{
					float4 vertex : POSITION;
					float4 uvgrab : TEXCOORD0;

					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				v2f vert(appdata v)
				{
					v2f o;

					UNITY_SETUP_INSTANCE_ID(v);
                	UNITY_TRANSFER_INSTANCE_ID(v, o);

					o.vertex = TransformObjectToHClip(v.vertex);
					#if UNITY_UV_STARTS_AT_TOP
					float scale = -1.0;
					#else
					float scale = 1.0;
					#endif
					o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
					o.uvgrab.zw = o.vertex.zw;
					return o;
				}

				sampler2D _CameraOpaqueTexture;
				float4 _CameraOpaqueTexture_TexelSize;
				
				float4 ProjCoord(float4 a)
				{
					a.xy = a.xy/a.w;
					
					return a;
				}

				half4 frag(v2f i) : COLOR
				{
					UNITY_SETUP_INSTANCE_ID(i);

					half4 sum = half4(0,0,0,0);

					#define GRABXYPIXEL(kernelx, kernely) tex2Dproj(_CameraOpaqueTexture, ProjCoord(float4(i.uvgrab.x + _CameraOpaqueTexture_TexelSize.x * kernelx, i.uvgrab.y + _CameraOpaqueTexture_TexelSize.y * kernely, i.uvgrab.z, i.uvgrab.w)));

					sum += GRABXYPIXEL(0.0, 0.0);
					int measurments = 1;

					for (float range = 0.1f; range <= _Radius; range += 0.1f)
					{
						sum += GRABXYPIXEL(range, range);
						sum += GRABXYPIXEL(range, -range);
						sum += GRABXYPIXEL(-range, range);
						sum += GRABXYPIXEL(-range, -range);
						measurments += 4;
					}

					return sum / measurments;
				}
				ENDHLSL
			}
			GrabPass
			{
				Tags{ "LightMode" = "Always" }
			}

			Pass
			{
				Tags{ "LightMode" = "Always" }

				HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

				#pragma multi_compile_instancing

				CBUFFER_START(UnityPerMaterial)
				
				float _Radius;

				CBUFFER_END

				struct appdata
				{
					float4 vertex : POSITION;
					float2 texcoord: TEXCOORD0;

					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f
				{
					float4 vertex : POSITION;
					float4 uvgrab : TEXCOORD0;

					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				v2f vert(appdata v)
				{
					v2f o;

					UNITY_SETUP_INSTANCE_ID(v);
                	UNITY_TRANSFER_INSTANCE_ID(v, o);

					o.vertex = TransformObjectToHClip(v.vertex);
					#if UNITY_UV_STARTS_AT_TOP
					float scale = -1.0;
					#else
					float scale = 1.0;
					#endif
					o.uvgrab.xy = (float2(o.vertex.x, -o.vertex.y * scale) + o.vertex.w) * 0.5;
					o.uvgrab.zw = o.vertex.zw;
					return o;
				}

				sampler2D _CameraOpaqueTexture;
				float4 _CameraOpaqueTexture_TexelSize;

				float4 ProjCoord(float4 a)
				{
					a.xy = a.xy/a.w;
					
					return a;
				}

				half4 frag(v2f i) : COLOR
				{
					UNITY_SETUP_INSTANCE_ID(i);

					half4 sum = half4(0,0,0,0);
					float radius = 1.41421356237 * _Radius;

					#define GRABXYPIXEL(kernelx, kernely) tex2Dproj( _CameraOpaqueTexture, ProjCoord(float4(i.uvgrab.x + _CameraOpaqueTexture_TexelSize.x * kernelx, i.uvgrab.y + _CameraOpaqueTexture_TexelSize.y * kernely, i.uvgrab.z, i.uvgrab.w)));

					sum += GRABXYPIXEL(0.0, 0.0);
					int measurments = 1;

					for (float range = 1.41421356237f; range <= radius * 1.41; range += 1.41421356237f)
					{
						sum += GRABXYPIXEL(range, 0);
						sum += GRABXYPIXEL(-range, 0);
						sum += GRABXYPIXEL(0, range);
						sum += GRABXYPIXEL(0, -range);
						measurments += 4;
					}

					return sum / measurments;
				}
				ENDHLSL
			}
		}
	}
}
