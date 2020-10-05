//참고: https://blog.naver.com/mnpshino/221844164319
//VsCode: Use "Better Comments" Plugins.
Shader "Custom/CustomToonShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        //_AmbientIntensity("Ambient Intensity", Range(0, 5)) = 1
        //_LightMapIntensity("LightMap Intensity", Range(0, 2)) = 1
        _ShadowIntensity("ShadowIntensity", Range(0, 1))= 0
        _ShadowOffset("Shadow Offset", Range(-1, 1)) = 0
        _ShadowColor ("Shadow Color", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Tags 
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry+0"
        }
        LOD 100
        
        //*기본적인 렌더링 pass
        Pass
        {
            Tags{"LightMode" = "UniversalForward"}

            //Blend[_SrcBlend][_DstBlend]
            //ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            // Lighting.hlsl에서 메인 라이트의 디렉션, 컬러, 실시간 그림자, 거리 감쇠 등의 정보를 가져올 수 있음.

            // GPU Instancing
            // fog 사용을 위해서 "multi_compile_fog"
            #pragma multi_compile_instancing
            #pragma multi_compile_fog 



            //* 프로퍼티로 선언한 부분을 CBUFFER로 감싸주면 됨.
            CBUFFER_START(UnityPerMaterial)
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            half4 _MainTex_ST;
            
            float _ShadowOffset;
            float4 _ShadowColor;

            float _AmbientIntensity;
            float _LightMapIntensity;
            float _ShadowIntensity;
            
            
            CBUFFER_END
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL; //사용하려나?
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL; //사용하려나?
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float fogCoord : TEXCOORD2; //안개
                float4 shadowCoord : TEXCOORD3; //그림자

                UNITY_VERTEX_INPUT_INSTANCE_ID
                // UNITY_VERTEX_OUTPUT_STEREO << VR이 활성화 되어 있는 경우 유효한 매크로
            };

            v2f vert (appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                //UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); << VR이 활성화 되어 있는 경우 유효한 매크로

                //* Transform~ -> 공간변환을 하는 행렬.
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.normal = TransformObjectToWorldNormal(v.normal);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = v.uv2.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                o.fogCoord = ComputeFogFactor(o.vertex.z);

                //#ifdef _MAIN_LIGHT_SHADOWS
                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
                o.shadowCoord = GetShadowCoord(vertexInput);
                //#endif
                
                //o.objSpaceViewDir = TransformWorldToObject(GetCameraPositionWS());

                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                //UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); << VR이 활성화 되어 있는 경우 유효한 매크로
                
                Light mainLight = GetMainLight(i.shadowCoord);

                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                float NdotL1 = saturate(dot(_MainLightPosition.xyz, i.normal) + _ShadowOffset); //NdotL로 간단히 라이팅
                half3 ambient = SampleSH(i.normal); //? 구면조화함수가 머지

                float3 lightAndShadow = smoothstep(0, 0.01, NdotL1);
                lightAndShadow = lightAndShadow == 1 ? lightAndShadow : 1-((1-lightAndShadow) * _ShadowIntensity * (1-_ShadowColor.rgb));

                // float3 lightMap = SAMPLE_TEXTURE2D(unity_Lightmap, sampler_MainTex, i.uv2.xy);

                // float4 ramp = SAMPLE_TEXTURE2D(_RampTex, sampler_RampTex, float2(NdotL1, _ShadowOffset));
                // col.rgb *= _MainLightColor.rgb * mainLight.shadowAttenuation * mainLight.distanceAttenuation + ambient;
                // col = all(ramp.rgb != float3(1,1,1)) ? col * ramp : col;
                // col.rgb = MixFog(col.rgb, i.fogCoord);

                col.rgb *= _MainLightColor.rgb * lightAndShadow + (ambient*_AmbientIntensity*_ShadowColor); 
                //col.rgb += NdotL1 * 0.01; 부드럽게?
                col.rgb = MixFog(col.rgb, i.fogCoord);
                
                return col;
            }
            ENDHLSL
        }

        Pass
        {
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }

        //* 셰도우 패스
        //이해: https://medium.com/@tmdcks2368/unity-urp-custom-shadow-shader-%EB%8F%84%EC%A0%84%ED%95%98%EA%B8%B0-frame-debugger%EB%A1%9C-%EC%9B%90%EC%9D%B8-%EC%B0%BE%EA%B8%B0-3-3-bae7825480d3
        //코드 참조: https://blog.naver.com/mnpshino/221850973199
        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ColorMask 0
            Cull Back

            HLSLPROGRAM
            
            // Required to compile gles 2.0 with standard srp library
            // 표준 srp 라이브러리로 gles 2.0을 컴파일하는 데 필요
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON
            
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNAL_A

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"


            CBUFFER_START(UnityPerMaterial)
            CBUFFER_END

            struct VertexInput
            {          
            float4 vertex : POSITION;
            float4 normal : NORMAL;
            
            #if _ALPHATEST_ON
            float2 uv     : TEXCOORD0;
            #endif

            UNITY_VERTEX_INPUT_INSTANCE_ID  
            };
        
            struct VertexOutput
            {          
            float4 vertex : SV_POSITION;

            #if _ALPHATEST_ON
            float2 uv     : TEXCOORD0;
            #endif
            UNITY_VERTEX_INPUT_INSTANCE_ID          
            // UNITY_VERTEX_OUTPUT_STEREO

            };

            VertexOutput ShadowPassVertex(VertexInput v)
            {
            VertexOutput o;
            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_TRANSFER_INSTANCE_ID(v, o);
            // UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);                             
        
            float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
            float3 normalWS   = TransformObjectToWorldNormal(v.normal.xyz);
            float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _MainLightPosition.xyz));
            
            o.vertex = positionCS;

            #if _ALPHATEST_ON
            o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw; ;
            #endif

            return o;
            }

            half4 ShadowPassFragment(VertexOutput i) : SV_TARGET
            {  
                UNITY_SETUP_INSTANCE_ID(i);
                // UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
            
                #if _ALPHATEST_ON
                float4 col = tex2D(_MainTex, i.uv);
                clip(col.a - _Alpha);
                #endif

                return half4(1, 1, 0.5, 0);
            }
            ENDHLSL
        }
        

        //3번째 패스, ssao 시도?
        // Pass
        // {
        //     Name "SSAO"
            
        //     ZWrite Off
            
        //     HLSLPROGRAM
        //     #pragma prefer_hlslcc gles
        //     #pragma exclude_renderers d3d11_9x
        //     #pragma vertex vert
        //     #pragma fragment frag
        //     #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        //     #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
        //     // Lighting.hlsl에서 메인 라이트의 디렉션, 컬러, 실시간 그림자, 거리 감쇠 등의 정보를 가져올 수 있음.

        //     // GPU Instancing
        //     // fog 사용을 위해서 "multi_compile_fog"
        //     #pragma multi_compile_instancing
            
        //     // Recive Shadow, 
        //     // 리시브 섀도우를 하기 위해서는 "_MAIN_LIGHT_SHADOS"를 포함해야 함
        //     #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
        //     #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
        //     #pragma multi_compile _ _ADDITIONAL_LIGHTS
        //     #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
        //     #pragma multi_compile _ _SHADOW_SOFT

        //     CBUFFER_START(UnityPerMaterial)
            
        //     TEXTURE2D(_MainTex);
        //     SAMPLER(sampler_MainTex);
        //     half4 _MainTex_ST;

        //     //TEXTURE2D (_CameraDepthTexture);
        //     //SAMPLER (sampler_CameraDepthTexture);

        //     TEXTURE2D(_RandomTexture);
        //     SAMPLER(_RandomTextureSampler);
        //     half4 _RandomTexture_ST;

        //     CBUFFER_END
            
        //     struct appdata
        //     {
        //         float4 vertex : POSITION;
        //         float3 normal : NORMAL; //사용하려나?
        //         float2 uv : TEXCOORD0;

        //         UNITY_VERTEX_INPUT_INSTANCE_ID
        //     };

        //     struct v2f
        //     {
        //         float4 vertex : SV_POSITION;
        //         float3 normal : NORMAL; //사용하려나?
        //         float2 uv : TEXCOORD0;

        //         UNITY_VERTEX_INPUT_INSTANCE_ID
        //         // UNITY_VERTEX_OUTPUT_STEREO << VR이 활성화 되어 있는 경우 유효한 매크로
        //     };

        //     v2f vert (appdata v)
        //     {
        //         v2f o;

        //         UNITY_SETUP_INSTANCE_ID(v);
        //         UNITY_TRANSFER_INSTANCE_ID(v, o);
        //         //UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); << VR이 활성화 되어 있는 경우 유효한 매크로

        //         //* Transform~ -> 공간변환을 하는 행렬.
        //         o.vertex = TransformObjectToHClip(v.vertex.xyz);
        //         o.normal = TransformObjectToWorldNormal(v.normal);
        //         o.uv = TRANSFORM_TEX(v.uv, _MainTex);

        //         //#ifdef _MAIN_LIGHT_SHADOWS
        //         VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
        //         //#endif

        //         return o;
        //     }


        //     float3 normal_from_depth(float depth, float2 texcoords)
        //     {
        //         const float2 offset1 = float2(0.0, 0.001);
        //         const float2 offset2 = float2(0.001, 0.0);

        //         float depth1 = tex2D(sampler_CameraDepthTexture, texcoords + offset1).r;
        //         float depth2 = tex2D(sampler_CameraDepthTexture, texcoords + offset2).r;
  
        //         float3 p1 = float3(offset1, depth1 - depth);
        //         float3 p2 = float3(offset2, depth2 - depth);
  
        //         float3 normal = cross(p1, p2);
        //         normal.z = -normal.z;

        //         return normalize(normal);
        //     }

        //     half4 frag(v2f In) : SV_TARGET
        //     {
        //         half4 Output;

        //         const float total_strength = 1.0;
        //         const float base = 0.2;
  
        //         const float area = 0.0075;
        //         const float falloff = 0.000001;
  
        //         const float radius = 0.0002;

        //         const int samples = 16;
        //         float3 sample_sphere[samples] = {
        //         float3( 0.5381, 0.1856,-0.4319), float3( 0.1379, 0.2486, 0.4430),
        //         float3( 0.3371, 0.5679,-0.0057), float3(-0.6999,-0.0451,-0.0019),
        //         float3( 0.0689,-0.1598,-0.8547), float3( 0.0560, 0.0069,-0.1843),
        //         float3(-0.0146, 0.1402, 0.0762), float3( 0.0100,-0.1924,-0.0344),
        //         float3(-0.3577,-0.5301,-0.4358), float3(-0.3169, 0.1063, 0.0158),
        //         float3( 0.0103,-0.5869, 0.0046), float3(-0.0897,-0.4940, 0.3287),
        //         float3( 0.7119,-0.0154,-0.0918), float3(-0.0533, 0.0596,-0.5411),
        //         float3( 0.0352,-0.0631, 0.5460), float3(-0.4776, 0.2847,-0.0271)
        //         };

        //         float3 random = normalize(tex2D(_RandomTextureSampler, In.uv * 4.0).rgb );
        //         float depth = tex2D(sampler_CameraDepthTexture, In.uv).r;
                
        //         float3 position = float3(In.uv, depth);
        //         float3 normal = normal_from_depth(depth, In.uv);
                
        //         float radius_depth = radius/depth;
        //         float occlusion = 0.0;

        //         for(int i=0; i < samples; i++) 
        //         {
        //             float3 ray = radius_depth * reflect(sample_sphere[i], random);
        //             float3 hemi_ray = position + sign(dot(ray,normal)) * ray;
                    
        //             float occ_depth = tex2D(sampler_CameraDepthTexture, saturate(hemi_ray.xy)).r;
        //             float difference = depth - occ_depth;
                    
        //             occlusion += step(falloff, difference) * (1.0-smoothstep(falloff, area, difference));
        //         }
  
        //         float ao = 1.0 - total_strength * occlusion * (1.0 / samples);
        //         Output.rgb = saturate(ao + base);
  
        //         return Output;
        //     }
        //     ENDHLSL
        // }
    }
}
