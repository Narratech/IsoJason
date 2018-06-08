// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:True,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33352,y:32712,varname:node_4013,prsc:2|diff-7036-OUT,spec-9428-OUT,normal-4920-OUT;n:type:ShaderForge.SFN_Color,id:1304,x:32625,y:32380,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1304,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:4271,x:32276,y:32664,ptovrint:False,ptlb:node_4271,ptin:_node_4271,varname:node_4271,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:2e281f4c0ce691244b8e6b5e82cd2843,ntxv:0,isnm:False|UVIN-5208-OUT;n:type:ShaderForge.SFN_Tex2d,id:521,x:32250,y:33126,ptovrint:False,ptlb:node_521,ptin:_node_521,varname:node_521,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c82748a5aa8bda5448c2541b4f16f632,ntxv:3,isnm:True|UVIN-3631-OUT;n:type:ShaderForge.SFN_Tex2d,id:2766,x:32250,y:32928,ptovrint:False,ptlb:node_2766,ptin:_node_2766,varname:node_2766,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a8272a4e7e276164c82554a062e8f40e,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Lerp,id:4346,x:32814,y:32976,varname:node_4346,prsc:2|A-2766-RGB,B-9436-OUT,T-839-OUT;n:type:ShaderForge.SFN_Vector1,id:839,x:32250,y:33288,varname:node_839,prsc:2,v1:0.5;n:type:ShaderForge.SFN_TexCoord,id:4274,x:31804,y:32594,varname:node_4274,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:5208,x:32038,y:32617,varname:node_5208,prsc:2|A-4274-UVOUT,B-1382-OUT;n:type:ShaderForge.SFN_Slider,id:1382,x:31786,y:32777,ptovrint:False,ptlb:diffuse tiling,ptin:_diffusetiling,varname:node_1382,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:10;n:type:ShaderForge.SFN_TexCoord,id:1658,x:31805,y:33045,varname:node_1658,prsc:2,uv:0;n:type:ShaderForge.SFN_Slider,id:2250,x:31787,y:33228,ptovrint:False,ptlb:detail 2 normal,ptin:_detail2normal,varname:_node_1382_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:4.871795,max:10;n:type:ShaderForge.SFN_Multiply,id:3631,x:32080,y:33126,varname:node_3631,prsc:2|A-1658-UVOUT,B-2250-OUT;n:type:ShaderForge.SFN_ComponentMask,id:8363,x:32492,y:33093,varname:node_8363,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-521-RGB;n:type:ShaderForge.SFN_Multiply,id:1762,x:32668,y:33166,varname:node_1762,prsc:2|A-8363-OUT,B-2340-OUT;n:type:ShaderForge.SFN_Slider,id:2340,x:32354,y:33400,ptovrint:False,ptlb:node_2340,ptin:_node_2340,varname:node_2340,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:5;n:type:ShaderForge.SFN_Append,id:9436,x:32904,y:33192,varname:node_9436,prsc:2|A-1762-OUT,B-521-B;n:type:ShaderForge.SFN_Tex2d,id:3091,x:32801,y:32799,ptovrint:False,ptlb:node_3091,ptin:_node_3091,varname:node_3091,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:6f52b4def7bb0f645a60eb2724701e03,ntxv:3,isnm:True|UVIN-8361-OUT;n:type:ShaderForge.SFN_Lerp,id:4920,x:33062,y:32861,varname:node_4920,prsc:2|A-3091-RGB,B-4346-OUT,T-839-OUT;n:type:ShaderForge.SFN_TexCoord,id:3291,x:31787,y:33304,varname:node_3291,prsc:2,uv:0;n:type:ShaderForge.SFN_Slider,id:6670,x:31769,y:33487,ptovrint:False,ptlb:Detail normal,ptin:_Detailnormal,varname:_node_1382_copy_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:4.871795,max:10;n:type:ShaderForge.SFN_Multiply,id:8361,x:32062,y:33385,varname:node_8361,prsc:2|A-3291-UVOUT,B-6670-OUT;n:type:ShaderForge.SFN_Multiply,id:1095,x:32869,y:32533,varname:node_1095,prsc:2|A-1304-RGB,B-4891-OUT;n:type:ShaderForge.SFN_LightVector,id:9685,x:32092,y:32269,varname:node_9685,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:379,x:32092,y:32113,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:3118,x:32371,y:32255,varname:node_3118,prsc:2,dt:0|A-379-OUT,B-9685-OUT;n:type:ShaderForge.SFN_Multiply,id:377,x:33420,y:33467,varname:node_377,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4891,x:32571,y:32586,varname:node_4891,prsc:2|A-3118-OUT,B-4271-RGB;n:type:ShaderForge.SFN_NormalVector,id:6653,x:32883,y:32324,prsc:2,pt:False;n:type:ShaderForge.SFN_ComponentMask,id:7627,x:33089,y:32363,varname:node_7627,prsc:2,cc1:2,cc2:-1,cc3:-1,cc4:-1|IN-6653-OUT;n:type:ShaderForge.SFN_Multiply,id:7869,x:33299,y:32300,varname:node_7869,prsc:2|A-8897-RGB,B-7627-OUT;n:type:ShaderForge.SFN_Color,id:8897,x:33089,y:32167,ptovrint:False,ptlb:node_8897,ptin:_node_8897,varname:node_8897,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Add,id:7036,x:33341,y:32544,varname:node_7036,prsc:2|A-7869-OUT,B-1095-OUT;n:type:ShaderForge.SFN_Vector1,id:9428,x:33001,y:32758,varname:node_9428,prsc:2,v1:0;proporder:1304-521-2766-4271-1382-2250-2340-3091-6670-8897;pass:END;sub:END;*/

Shader "Shader Forge/Rock" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _node_521 ("node_521", 2D) = "bump" {}
        _node_2766 ("node_2766", 2D) = "bump" {}
        _node_4271 ("node_4271", 2D) = "white" {}
        _diffusetiling ("diffuse tiling", Range(0, 10)) = 0
        _detail2normal ("detail 2 normal", Range(0, 10)) = 4.871795
        _node_2340 ("node_2340", Range(0, 5)) = 0
        _node_3091 ("node_3091", 2D) = "bump" {}
        _Detailnormal ("Detail normal", Range(0, 10)) = 4.871795
        _node_8897 ("node_8897", Color) = (0.5,0.5,0.5,1)
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
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _Color;
            uniform sampler2D _node_4271; uniform float4 _node_4271_ST;
            uniform sampler2D _node_521; uniform float4 _node_521_ST;
            uniform sampler2D _node_2766; uniform float4 _node_2766_ST;
            uniform float _diffusetiling;
            uniform float _detail2normal;
            uniform float _node_2340;
            uniform sampler2D _node_3091; uniform float4 _node_3091_ST;
            uniform float _Detailnormal;
            uniform float4 _node_8897;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD10;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
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
                float2 node_8361 = (i.uv0*_Detailnormal);
                float3 _node_3091_var = UnpackNormal(tex2D(_node_3091,TRANSFORM_TEX(node_8361, _node_3091)));
                float3 _node_2766_var = UnpackNormal(tex2D(_node_2766,TRANSFORM_TEX(i.uv0, _node_2766)));
                float2 node_3631 = (i.uv0*_detail2normal);
                float3 _node_521_var = UnpackNormal(tex2D(_node_521,TRANSFORM_TEX(node_3631, _node_521)));
                float node_839 = 0.5;
                float3 normalLocal = lerp(_node_3091_var.rgb,lerp(_node_2766_var.rgb,float3((_node_521_var.rgb.rg*_node_2340),_node_521_var.b),node_839),node_839);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float node_9428 = 0.0;
                float3 specularColor = float3(node_9428,node_9428,node_9428);
                float3 directSpecular = 1 * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float2 node_5208 = (i.uv0*_diffusetiling);
                float4 _node_4271_var = tex2D(_node_4271,TRANSFORM_TEX(node_5208, _node_4271));
                float3 diffuseColor = ((_node_8897.rgb*i.normalDir.b)+(_Color.rgb*(dot(i.normalDir,lightDirection)*_node_4271_var.rgb)));
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
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
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _Color;
            uniform sampler2D _node_4271; uniform float4 _node_4271_ST;
            uniform sampler2D _node_521; uniform float4 _node_521_ST;
            uniform sampler2D _node_2766; uniform float4 _node_2766_ST;
            uniform float _diffusetiling;
            uniform float _detail2normal;
            uniform float _node_2340;
            uniform sampler2D _node_3091; uniform float4 _node_3091_ST;
            uniform float _Detailnormal;
            uniform float4 _node_8897;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
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
                float2 node_8361 = (i.uv0*_Detailnormal);
                float3 _node_3091_var = UnpackNormal(tex2D(_node_3091,TRANSFORM_TEX(node_8361, _node_3091)));
                float3 _node_2766_var = UnpackNormal(tex2D(_node_2766,TRANSFORM_TEX(i.uv0, _node_2766)));
                float2 node_3631 = (i.uv0*_detail2normal);
                float3 _node_521_var = UnpackNormal(tex2D(_node_521,TRANSFORM_TEX(node_3631, _node_521)));
                float node_839 = 0.5;
                float3 normalLocal = lerp(_node_3091_var.rgb,lerp(_node_2766_var.rgb,float3((_node_521_var.rgb.rg*_node_2340),_node_521_var.b),node_839),node_839);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float node_9428 = 0.0;
                float3 specularColor = float3(node_9428,node_9428,node_9428);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float2 node_5208 = (i.uv0*_diffusetiling);
                float4 _node_4271_var = tex2D(_node_4271,TRANSFORM_TEX(node_5208, _node_4271));
                float3 diffuseColor = ((_node_8897.rgb*i.normalDir.b)+(_Color.rgb*(dot(i.normalDir,lightDirection)*_node_4271_var.rgb)));
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _Color;
            uniform sampler2D _node_4271; uniform float4 _node_4271_ST;
            uniform float _diffusetiling;
            uniform float4 _node_8897;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                o.Emission = 0;
                
                float2 node_5208 = (i.uv0*_diffusetiling);
                float4 _node_4271_var = tex2D(_node_4271,TRANSFORM_TEX(node_5208, _node_4271));
                float3 diffColor = ((_node_8897.rgb*i.normalDir.b)+(_Color.rgb*(dot(i.normalDir,lightDirection)*_node_4271_var.rgb)));
                float node_9428 = 0.0;
                float3 specColor = float3(node_9428,node_9428,node_9428);
                o.Albedo = diffColor + specColor * 0.125; // No gloss connected. Assume it's 0.5
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
