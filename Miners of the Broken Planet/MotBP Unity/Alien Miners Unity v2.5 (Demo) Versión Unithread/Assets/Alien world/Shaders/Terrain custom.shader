// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:34509,y:32968,varname:node_4013,prsc:2|diff-2706-OUT,normal-9515-OUT;n:type:ShaderForge.SFN_Tex2d,id:9381,x:31627,y:33042,ptovrint:False,ptlb:Splatmap,ptin:_Splatmap,varname:node_9381,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c0ed73cbfcf739a4696bbcc3203a7b93,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:8812,x:32972,y:33333,ptovrint:False,ptlb:Normal map terrain,ptin:_Normalmapterrain,varname:node_8812,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f8a14ff0ba282144d873859536892a97,ntxv:3,isnm:True;n:type:ShaderForge.SFN_TexCoord,id:3196,x:31064,y:33815,varname:node_3196,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:2175,x:31282,y:33825,varname:node_2175,prsc:2|A-3196-UVOUT,B-3822-OUT;n:type:ShaderForge.SFN_Slider,id:3822,x:31052,y:33995,ptovrint:False,ptlb:Normal map  far details,ptin:_Normalmapfardetails,varname:node_3822,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:200,max:200;n:type:ShaderForge.SFN_Lerp,id:9355,x:33155,y:33856,varname:node_9355,prsc:2|A-1979-OUT,B-5997-OUT,T-3042-OUT;n:type:ShaderForge.SFN_Vector1,id:3042,x:33033,y:34017,varname:node_3042,prsc:2,v1:0.5;n:type:ShaderForge.SFN_TexCoord,id:2684,x:31070,y:33423,varname:node_2684,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:7954,x:31276,y:33428,varname:node_7954,prsc:2|A-2684-UVOUT,B-3958-OUT;n:type:ShaderForge.SFN_Slider,id:3958,x:31058,y:33603,ptovrint:False,ptlb:Near UV,ptin:_NearUV,varname:_node_3822_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:1000;n:type:ShaderForge.SFN_Lerp,id:5518,x:33448,y:31865,varname:node_5518,prsc:2|A-2101-RGB,B-4185-RGB,T-9381-R;n:type:ShaderForge.SFN_Lerp,id:2706,x:33715,y:32016,varname:node_2706,prsc:2|A-5518-OUT,B-3409-RGB,T-9381-G;n:type:ShaderForge.SFN_Tex2dAsset,id:8633,x:32439,y:31755,ptovrint:False,ptlb:Texture 1,ptin:_Texture1,varname:node_8633,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:8fd14c5021d819e4f8f39d900fc6ee01,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2dAsset,id:7155,x:32454,y:32079,ptovrint:False,ptlb:Texture 2,ptin:_Texture2,varname:node_7155,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:fa35600a07d8c0e4191724d14342c0b2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2dAsset,id:6385,x:32483,y:32303,ptovrint:False,ptlb:Texture 3,ptin:_Texture3,varname:node_6385,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a083837239d87b6478ec00dd4d0a80d9,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:4185,x:33071,y:32044,varname:node_4185,prsc:2,tex:fa35600a07d8c0e4191724d14342c0b2,ntxv:0,isnm:False|UVIN-7954-OUT,TEX-7155-TEX;n:type:ShaderForge.SFN_Tex2d,id:3409,x:33071,y:32257,varname:node_3409,prsc:2,tex:a083837239d87b6478ec00dd4d0a80d9,ntxv:0,isnm:False|UVIN-7954-OUT,TEX-6385-TEX;n:type:ShaderForge.SFN_Tex2d,id:2101,x:33071,y:31839,varname:node_2101,prsc:2,tex:8fd14c5021d819e4f8f39d900fc6ee01,ntxv:0,isnm:False|UVIN-7954-OUT,TEX-8633-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:9325,x:31543,y:34187,ptovrint:False,ptlb:Normal 1,ptin:_Normal1,varname:node_9325,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c82748a5aa8bda5448c2541b4f16f632,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2dAsset,id:3093,x:31556,y:34383,ptovrint:False,ptlb:Normal 2,ptin:_Normal2,varname:node_3093,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a1193b03c1b8fed40b778d49374701c0,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2dAsset,id:3523,x:31556,y:34603,ptovrint:False,ptlb:Normal 3,ptin:_Normal3,varname:node_3523,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0db86668269d1a84796262ab8e40da8a,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:9201,x:32165,y:34236,varname:node_9201,prsc:2,tex:c82748a5aa8bda5448c2541b4f16f632,ntxv:0,isnm:False|UVIN-7954-OUT,TEX-9325-TEX;n:type:ShaderForge.SFN_Tex2d,id:3087,x:32165,y:34406,varname:node_3087,prsc:2,tex:a1193b03c1b8fed40b778d49374701c0,ntxv:0,isnm:False|UVIN-7954-OUT,TEX-3093-TEX;n:type:ShaderForge.SFN_Tex2d,id:3027,x:32153,y:34587,varname:node_3027,prsc:2,tex:0db86668269d1a84796262ab8e40da8a,ntxv:0,isnm:False|UVIN-7954-OUT,TEX-3523-TEX;n:type:ShaderForge.SFN_Lerp,id:5404,x:32522,y:34266,varname:node_5404,prsc:2|A-9201-RGB,B-3087-RGB,T-9381-R;n:type:ShaderForge.SFN_Lerp,id:1979,x:32723,y:34360,varname:node_1979,prsc:2|A-5404-OUT,B-3027-RGB,T-9381-G;n:type:ShaderForge.SFN_Lerp,id:9515,x:33537,y:33566,varname:node_9515,prsc:2|A-8812-RGB,B-9355-OUT,T-4829-OUT;n:type:ShaderForge.SFN_Vector1,id:4829,x:33271,y:33684,varname:node_4829,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Tex2d,id:2274,x:31919,y:33776,varname:node_2274,prsc:2,tex:8238403a96f1dc7438313114f1ce4eed,ntxv:3,isnm:True|UVIN-2175-OUT,TEX-8947-TEX;n:type:ShaderForge.SFN_Multiply,id:9209,x:32581,y:33648,varname:node_9209,prsc:2|A-5591-OUT,B-8364-OUT;n:type:ShaderForge.SFN_Slider,id:5591,x:32200,y:33517,ptovrint:False,ptlb: terrain normal intensity,ptin:_terrainnormalintensity,varname:node_5591,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:5,max:5;n:type:ShaderForge.SFN_ComponentMask,id:871,x:32319,y:33889,varname:node_871,prsc:2,cc1:2,cc2:-1,cc3:-1,cc4:-1|IN-2274-RGB;n:type:ShaderForge.SFN_Append,id:5997,x:32775,y:33772,varname:node_5997,prsc:2|A-9209-OUT,B-871-OUT;n:type:ShaderForge.SFN_ComponentMask,id:8364,x:32278,y:33645,varname:node_8364,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-2274-RGB;n:type:ShaderForge.SFN_Tex2dAsset,id:8947,x:31543,y:33936,ptovrint:False,ptlb:Normal far details,ptin:_Normalfardetails,varname:node_8947,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:8238403a96f1dc7438313114f1ce4eed,ntxv:3,isnm:True;proporder:8812-9381-3958-8633-7155-6385-9325-3093-3523-3822-5591-8947;pass:END;sub:END;*/

Shader "Shader Forge/Terrain custom" {
    Properties {
        _Normalmapterrain ("Normal map terrain", 2D) = "bump" {}
        _Splatmap ("Splatmap", 2D) = "white" {}
        _NearUV ("Near UV", Range(1, 1000)) = 1
        _Texture1 ("Texture 1", 2D) = "white" {}
        _Texture2 ("Texture 2", 2D) = "white" {}
        _Texture3 ("Texture 3", 2D) = "white" {}
        _Normal1 ("Normal 1", 2D) = "bump" {}
        _Normal2 ("Normal 2", 2D) = "bump" {}
        _Normal3 ("Normal 3", 2D) = "bump" {}
        _Normalmapfardetails ("Normal map  far details", Range(1, 200)) = 200
        _terrainnormalintensity (" terrain normal intensity", Range(1, 5)) = 5
        _Normalfardetails ("Normal far details", 2D) = "bump" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Splatmap; uniform float4 _Splatmap_ST;
            uniform sampler2D _Normalmapterrain; uniform float4 _Normalmapterrain_ST;
            uniform float _Normalmapfardetails;
            uniform float _NearUV;
            uniform sampler2D _Texture1; uniform float4 _Texture1_ST;
            uniform sampler2D _Texture2; uniform float4 _Texture2_ST;
            uniform sampler2D _Texture3; uniform float4 _Texture3_ST;
            uniform sampler2D _Normal1; uniform float4 _Normal1_ST;
            uniform sampler2D _Normal2; uniform float4 _Normal2_ST;
            uniform sampler2D _Normal3; uniform float4 _Normal3_ST;
            uniform float _terrainnormalintensity;
            uniform sampler2D _Normalfardetails; uniform float4 _Normalfardetails_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normalmapterrain_var = UnpackNormal(tex2D(_Normalmapterrain,TRANSFORM_TEX(i.uv0, _Normalmapterrain)));
                float2 node_7954 = (i.uv0*_NearUV);
                float3 node_9201 = UnpackNormal(tex2D(_Normal1,TRANSFORM_TEX(node_7954, _Normal1)));
                float3 node_3087 = UnpackNormal(tex2D(_Normal2,TRANSFORM_TEX(node_7954, _Normal2)));
                float4 _Splatmap_var = tex2D(_Splatmap,TRANSFORM_TEX(i.uv0, _Splatmap));
                float3 node_3027 = UnpackNormal(tex2D(_Normal3,TRANSFORM_TEX(node_7954, _Normal3)));
                float2 node_2175 = (i.uv0*_Normalmapfardetails);
                float3 node_2274 = UnpackNormal(tex2D(_Normalfardetails,TRANSFORM_TEX(node_2175, _Normalfardetails)));
                float3 normalLocal = lerp(_Normalmapterrain_var.rgb,lerp(lerp(lerp(node_9201.rgb,node_3087.rgb,_Splatmap_var.r),node_3027.rgb,_Splatmap_var.g),float3((_terrainnormalintensity*node_2274.rgb.rg),node_2274.rgb.b),0.5),0.5);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 node_2101 = tex2D(_Texture1,TRANSFORM_TEX(node_7954, _Texture1));
                float4 node_4185 = tex2D(_Texture2,TRANSFORM_TEX(node_7954, _Texture2));
                float4 node_3409 = tex2D(_Texture3,TRANSFORM_TEX(node_7954, _Texture3));
                float3 diffuseColor = lerp(lerp(node_2101.rgb,node_4185.rgb,_Splatmap_var.r),node_3409.rgb,_Splatmap_var.g);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Splatmap; uniform float4 _Splatmap_ST;
            uniform sampler2D _Normalmapterrain; uniform float4 _Normalmapterrain_ST;
            uniform float _Normalmapfardetails;
            uniform float _NearUV;
            uniform sampler2D _Texture1; uniform float4 _Texture1_ST;
            uniform sampler2D _Texture2; uniform float4 _Texture2_ST;
            uniform sampler2D _Texture3; uniform float4 _Texture3_ST;
            uniform sampler2D _Normal1; uniform float4 _Normal1_ST;
            uniform sampler2D _Normal2; uniform float4 _Normal2_ST;
            uniform sampler2D _Normal3; uniform float4 _Normal3_ST;
            uniform float _terrainnormalintensity;
            uniform sampler2D _Normalfardetails; uniform float4 _Normalfardetails_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normalmapterrain_var = UnpackNormal(tex2D(_Normalmapterrain,TRANSFORM_TEX(i.uv0, _Normalmapterrain)));
                float2 node_7954 = (i.uv0*_NearUV);
                float3 node_9201 = UnpackNormal(tex2D(_Normal1,TRANSFORM_TEX(node_7954, _Normal1)));
                float3 node_3087 = UnpackNormal(tex2D(_Normal2,TRANSFORM_TEX(node_7954, _Normal2)));
                float4 _Splatmap_var = tex2D(_Splatmap,TRANSFORM_TEX(i.uv0, _Splatmap));
                float3 node_3027 = UnpackNormal(tex2D(_Normal3,TRANSFORM_TEX(node_7954, _Normal3)));
                float2 node_2175 = (i.uv0*_Normalmapfardetails);
                float3 node_2274 = UnpackNormal(tex2D(_Normalfardetails,TRANSFORM_TEX(node_2175, _Normalfardetails)));
                float3 normalLocal = lerp(_Normalmapterrain_var.rgb,lerp(lerp(lerp(node_9201.rgb,node_3087.rgb,_Splatmap_var.r),node_3027.rgb,_Splatmap_var.g),float3((_terrainnormalintensity*node_2274.rgb.rg),node_2274.rgb.b),0.5),0.5);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 node_2101 = tex2D(_Texture1,TRANSFORM_TEX(node_7954, _Texture1));
                float4 node_4185 = tex2D(_Texture2,TRANSFORM_TEX(node_7954, _Texture2));
                float4 node_3409 = tex2D(_Texture3,TRANSFORM_TEX(node_7954, _Texture3));
                float3 diffuseColor = lerp(lerp(node_2101.rgb,node_4185.rgb,_Splatmap_var.r),node_3409.rgb,_Splatmap_var.g);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
