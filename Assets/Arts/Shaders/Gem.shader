// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:True,tesm:0,olmd:0,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1873,x:33653,y:32784,varname:node_1873,prsc:2|emission-4526-OUT,alpha-7768-OUT,clip-2200-OUT;n:type:ShaderForge.SFN_Tex2d,id:4805,x:32153,y:32573,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:True,tagnsco:False,tagnrm:False,tex:899273d0f1e63bc49838d135d268379a,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:8019,x:30508,y:33613,ptovrint:False,ptlb:GlowAmount,ptin:_GlowAmount,varname:_GlowAmount,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Subtract,id:7666,x:30885,y:33445,varname:node_7666,prsc:2|A-4504-OUT,B-8019-OUT;n:type:ShaderForge.SFN_RemapRange,id:8477,x:31253,y:33445,varname:node_8477,prsc:2,frmn:0,frmx:0.1,tomn:1,tomx:0|IN-2053-OUT;n:type:ShaderForge.SFN_Abs,id:2053,x:31075,y:33445,varname:node_2053,prsc:2|IN-7666-OUT;n:type:ShaderForge.SFN_Add,id:9802,x:33176,y:32837,varname:node_9802,prsc:2|A-4805-RGB,B-3512-OUT,C-7494-OUT,D-6670-OUT;n:type:ShaderForge.SFN_Clamp01,id:50,x:31447,y:33445,varname:node_50,prsc:2|IN-8477-OUT;n:type:ShaderForge.SFN_Multiply,id:3512,x:32809,y:32814,varname:node_3512,prsc:2|A-4805-A,B-5629-OUT;n:type:ShaderForge.SFN_Tex2d,id:5833,x:30621,y:34275,ptovrint:False,ptlb:Disperse,ptin:_Disperse,varname:_Disperse,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:2ed6bfb6011e8c449b59f2097ce9bed0,ntxv:0,isnm:False|UVIN-3424-OUT;n:type:ShaderForge.SFN_Smoothstep,id:9388,x:31298,y:34068,varname:node_9388,prsc:2|A-2734-OUT,B-8748-OUT,V-5833-R;n:type:ShaderForge.SFN_Slider,id:2734,x:30323,y:33940,ptovrint:False,ptlb:DisperseAmount,ptin:_DisperseAmount,varname:_DisperseAmount,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.4889161,max:1;n:type:ShaderForge.SFN_Vector1,id:9494,x:30572,y:34117,varname:node_9494,prsc:2,v1:0.01;n:type:ShaderForge.SFN_Add,id:8748,x:30835,y:34094,varname:node_8748,prsc:2|A-2734-OUT,B-9494-OUT;n:type:ShaderForge.SFN_Multiply,id:7768,x:33275,y:33064,varname:node_7768,prsc:2|A-4805-A,B-1693-OUT;n:type:ShaderForge.SFN_Multiply,id:4526,x:33379,y:32859,varname:node_4526,prsc:2|A-9802-OUT,B-6643-OUT;n:type:ShaderForge.SFN_Smoothstep,id:2892,x:31298,y:33924,varname:node_2892,prsc:2|A-2734-OUT,B-9088-OUT,V-5833-R;n:type:ShaderForge.SFN_Vector1,id:4738,x:30896,y:33848,varname:node_4738,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Add,id:9088,x:31066,y:33804,varname:node_9088,prsc:2|A-2734-OUT,B-4738-OUT;n:type:ShaderForge.SFN_OneMinus,id:9361,x:31524,y:33870,varname:node_9361,prsc:2|IN-2892-OUT;n:type:ShaderForge.SFN_Multiply,id:7494,x:32884,y:33048,varname:node_7494,prsc:2|A-855-RGB,B-4256-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:7830,x:31288,y:32853,ptovrint:False,ptlb:Selected,ptin:_Selected,varname:_Selected,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False;n:type:ShaderForge.SFN_Multiply,id:3333,x:31773,y:32930,varname:node_3333,prsc:2|A-7830-OUT,B-1104-OUT,C-8568-OUT;n:type:ShaderForge.SFN_Vector1,id:1104,x:31311,y:32969,varname:node_1104,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Time,id:891,x:31062,y:32986,varname:node_891,prsc:2;n:type:ShaderForge.SFN_Sin,id:2067,x:31357,y:33033,varname:node_2067,prsc:2|IN-891-TTR;n:type:ShaderForge.SFN_Abs,id:8568,x:31586,y:33008,varname:node_8568,prsc:2|IN-2067-OUT;n:type:ShaderForge.SFN_Set,id:6061,x:31782,y:33446,varname:Glow,prsc:2|IN-6858-OUT;n:type:ShaderForge.SFN_Get,id:5629,x:32437,y:32857,varname:node_5629,prsc:2|IN-6061-OUT;n:type:ShaderForge.SFN_Set,id:8909,x:31692,y:34075,varname:DisperseInner,prsc:2|IN-9388-OUT;n:type:ShaderForge.SFN_Set,id:6935,x:31785,y:33819,varname:DisperseOuter,prsc:2|IN-9361-OUT;n:type:ShaderForge.SFN_Get,id:4256,x:32615,y:33151,varname:node_4256,prsc:2|IN-6935-OUT;n:type:ShaderForge.SFN_Get,id:6643,x:33064,y:32996,varname:node_6643,prsc:2|IN-8909-OUT;n:type:ShaderForge.SFN_Get,id:1693,x:32732,y:33253,varname:node_1693,prsc:2|IN-8909-OUT;n:type:ShaderForge.SFN_Set,id:5202,x:31994,y:32930,varname:Selected,prsc:2|IN-3333-OUT;n:type:ShaderForge.SFN_Get,id:6670,x:32458,y:32941,varname:node_6670,prsc:2|IN-5202-OUT;n:type:ShaderForge.SFN_Desaturate,id:1677,x:32577,y:32509,varname:node_1677,prsc:2|COL-4805-RGB;n:type:ShaderForge.SFN_Set,id:7044,x:32805,y:32528,varname:Desaturate,prsc:2|IN-1677-OUT;n:type:ShaderForge.SFN_Get,id:4504,x:30480,y:33426,varname:node_4504,prsc:2|IN-7044-OUT;n:type:ShaderForge.SFN_VertexColor,id:855,x:32566,y:33026,varname:node_855,prsc:2;n:type:ShaderForge.SFN_Append,id:3424,x:30356,y:34267,varname:node_3424,prsc:2|A-5689-X,B-5689-Y;n:type:ShaderForge.SFN_FragmentPosition,id:5689,x:29967,y:34214,varname:node_5689,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:374,x:29982,y:34709,ptovrint:False,ptlb:_YClip,ptin:_YClip,varname:_Clip,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Subtract,id:7997,x:30426,y:34653,varname:node_7997,prsc:2|A-1615-OUT,B-5689-Y;n:type:ShaderForge.SFN_Set,id:8230,x:30737,y:34660,varname:AlphaClip,prsc:2|IN-7997-OUT;n:type:ShaderForge.SFN_Get,id:2200,x:33254,y:33216,varname:node_2200,prsc:2|IN-8230-OUT;n:type:ShaderForge.SFN_Vector1,id:5747,x:29982,y:34805,varname:node_5747,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Add,id:1615,x:30224,y:34712,varname:node_1615,prsc:2|A-374-OUT,B-5747-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9128,x:31433,y:33629,ptovrint:False,ptlb:HintAmount,ptin:_HintAmount,varname:_HintAmount,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:6858,x:31635,y:33446,varname:node_6858,prsc:2|A-50-OUT,B-9128-OUT;proporder:4805-5833-8019-2734-7830-9128;pass:END;sub:END;*/

Shader "Match3/Gem" {
    Properties {
        [PerRendererData]_MainTex ("MainTex", 2D) = "white" {}
        _Disperse ("Disperse", 2D) = "white" {}
        _GlowAmount ("GlowAmount", Range(0, 1)) = 1
        _DisperseAmount ("DisperseAmount", Range(0, 1)) = 0.4889161
        [MaterialToggle] _Selected ("Selected", Float ) = 0
        _HintAmount ("HintAmount", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _GlowAmount;
            uniform sampler2D _Disperse; uniform float4 _Disperse_ST;
            uniform float _DisperseAmount;
            uniform fixed _Selected;
            uniform float _YClip;
            uniform float _HintAmount;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                #ifdef PIXELSNAP_ON
                    o.pos = UnityPixelSnap(o.pos);
                #endif
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float AlphaClip = ((_YClip+0.5)-i.posWorld.g);
                clip(AlphaClip - 0.5);
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float Desaturate = dot(_MainTex_var.rgb,float3(0.3,0.59,0.11));
                float Glow = (saturate((abs((Desaturate-_GlowAmount))*-10.0+1.0))*_HintAmount);
                float2 node_3424 = float2(i.posWorld.r,i.posWorld.g);
                float4 _Disperse_var = tex2D(_Disperse,TRANSFORM_TEX(node_3424, _Disperse));
                float DisperseOuter = (1.0 - smoothstep( _DisperseAmount, (_DisperseAmount+0.1), _Disperse_var.r ));
                float4 node_891 = _Time;
                float Selected = (_Selected*0.2*abs(sin(node_891.a)));
                float DisperseInner = smoothstep( _DisperseAmount, (_DisperseAmount+0.01), _Disperse_var.r );
                float3 emissive = ((_MainTex_var.rgb+(_MainTex_var.a*Glow)+(i.vertexColor.rgb*DisperseOuter)+Selected)*DisperseInner);
                float3 finalColor = emissive;
                return fixed4(finalColor,(_MainTex_var.a*DisperseInner));
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _YClip;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float4 posWorld : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                #ifdef PIXELSNAP_ON
                    o.pos = UnityPixelSnap(o.pos);
                #endif
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float AlphaClip = ((_YClip+0.5)-i.posWorld.g);
                clip(AlphaClip - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
