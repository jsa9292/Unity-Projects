// Upgrade NOTE: upgraded instancing buffer 'ASESampleShadersNormalExtrusion' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASESampleShaders/NormalExtrusion"
{
	Properties
	{
		_ExtrusionPoint("ExtrusionPoint", Float) = 0.1
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 3
		_TessMin( "Tess Min Distance", Float ) = 17.62
		_TessMax( "Tess Max Distance", Float ) = 25
		_TessPhongStrength( "Phong Tess Strength", Range( 0, 1 ) ) = 1
		_BrushedMetalNormal("BrushedMetalNormal", 2D) = "bump" {}
		_Distortion("Distortion", Range( 0 , 1)) = 0.292
		_ExtrusionAmount("Extrusion Amount", Float) = 1
		_Color("Color", Color) = (0,0,0,0)
		[Header(Translucency)]
		_Translucency("Strength", Range( 0 , 50)) = 1
		_TransNormalDistortion("Normal Distortion", Range( 0 , 1)) = 0.1
		_TransScattering("Scaterring Falloff", Range( 1 , 50)) = 2
		_TransDirect("Direct", Range( 0 , 1)) = 1
		_TransAmbient("Ambient", Range( 0 , 1)) = 0.2
		_TransShadow("Shadow", Range( 0 , 1)) = 0.9
		_Axis_x("Axis_x", Float) = 0
		_Axis_y("Axis_y", Float) = 0
		_Axis_z("Axis_z", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
		[Header(Forward Rendering Options)]
		[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		[ToggleOff] _GlossyReflections("Reflections", Float) = 1.0
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "ForceNoShadowCasting" = "True" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma multi_compile_instancing
		#pragma shader_feature _SPECULARHIGHLIGHTS_OFF
		#pragma shader_feature _GLOSSYREFLECTIONS_OFF
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma only_renderers d3d9 d3d11_9x d3d11 glcore gles gles3 
		#pragma surface surf StandardCustom keepalpha noshadow exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction tessphong:_TessPhongStrength 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		struct SurfaceOutputStandardCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			half3 Translucency;
		};

		uniform float _ExtrusionPoint;
		uniform float _ExtrusionAmount;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform sampler2D _BrushedMetalNormal;
		uniform float _Distortion;
		uniform half _Translucency;
		uniform half _TransNormalDistortion;
		uniform half _TransScattering;
		uniform half _TransDirect;
		uniform half _TransAmbient;
		uniform half _TransShadow;
		uniform float _TessValue;
		uniform float _TessMin;
		uniform float _TessMax;
		uniform float _TessPhongStrength;

		UNITY_INSTANCING_BUFFER_START(ASESampleShadersNormalExtrusion)
			UNITY_DEFINE_INSTANCED_PROP(half4, _BrushedMetalNormal_ST)
#define _BrushedMetalNormal_ST_arr ASESampleShadersNormalExtrusion
			UNITY_DEFINE_INSTANCED_PROP(half4, _Color)
#define _Color_arr ASESampleShadersNormalExtrusion
			UNITY_DEFINE_INSTANCED_PROP(half, _Axis_x)
#define _Axis_x_arr ASESampleShadersNormalExtrusion
			UNITY_DEFINE_INSTANCED_PROP(half, _Axis_y)
#define _Axis_y_arr ASESampleShadersNormalExtrusion
			UNITY_DEFINE_INSTANCED_PROP(half, _Axis_z)
#define _Axis_z_arr ASESampleShadersNormalExtrusion
		UNITY_INSTANCING_BUFFER_END(ASESampleShadersNormalExtrusion)


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, _TessMin, _TessMax, _TessValue );
		}

		void vertexDataFunc( inout appdata_full v )
		{
			half3 ase_vertexNormal = v.normal.xyz;
			half _Axis_x_Instance = UNITY_ACCESS_INSTANCED_PROP(_Axis_x_arr, _Axis_x);
			float3 ase_vertex3Pos = v.vertex.xyz;
			half3 break90 = ( sin( ( ( ase_vertex3Pos + _Time.x ) / _ExtrusionPoint ) ) + float3( 1,1,1 ) );
			half _Axis_y_Instance = UNITY_ACCESS_INSTANCED_PROP(_Axis_y_arr, _Axis_y);
			half _Axis_z_Instance = UNITY_ACCESS_INSTANCED_PROP(_Axis_z_arr, _Axis_z);
			half4 appendResult94 = (half4(( _Axis_x_Instance * break90.x ) , ( _Axis_y_Instance * break90.y ) , ( _Axis_z_Instance * break90.z ) , 0.0));
			v.vertex.xyz += ( half4( ase_vertexNormal , 0.0 ) * ( appendResult94 / _ExtrusionAmount ) ).xyz;
			v.vertex.w = 1;
		}

		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			#if !DIRECTIONAL
			float3 lightAtten = gi.light.color;
			#else
			float3 lightAtten = lerp( _LightColor0.rgb, gi.light.color, _TransShadow );
			#endif
			half3 lightDir = gi.light.dir + s.Normal * _TransNormalDistortion;
			half transVdotL = pow( saturate( dot( viewDir, -lightDir ) ), _TransScattering );
			half3 translucency = lightAtten * (transVdotL * _TransDirect + gi.indirect.diffuse * _TransAmbient) * s.Translucency;
			half4 c = half4( s.Albedo * translucency * _Translucency, 0 );

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + c;
		}

		inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi )
		{
			#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
				gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
			#else
				UNITY_GLOSSY_ENV_FROM_SURFACE( g, s, data );
				gi = UnityGlobalIllumination( data, s.Occlusion, s.Normal, g );
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardCustom o )
		{
			half4 _BrushedMetalNormal_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_BrushedMetalNormal_ST_arr, _BrushedMetalNormal_ST);
			float2 uv_BrushedMetalNormal = i.uv_texcoord * _BrushedMetalNormal_ST_Instance.xy + _BrushedMetalNormal_ST_Instance.zw;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			half4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor107 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( half3( (( UnpackNormal( tex2D( _BrushedMetalNormal, uv_BrushedMetalNormal ) ) * _Distortion )).xy ,  0.0 ) + (ase_grabScreenPosNorm).xyw ).xy);
			half4 _Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color);
			half4 lerpResult116 = lerp( screenColor107 , _Color_Instance , float4( 0.4811321,0.4811321,0.4811321,0 ));
			o.Albedo = lerpResult116.rgb;
			o.Emission = screenColor107.rgb;
			o.Metallic = 1.0;
			o.Smoothness = -1.0;
			half3 temp_cast_4 = (0.5).xxx;
			o.Translucency = temp_cast_4;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18600
0;73;1046;415;511.1778;654.1223;1.507173;True;False
Node;AmplifyShaderEditor.TimeNode;25;-1312,272;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;18;-1371.541,72.25986;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-1056,160;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1052.238,256;Float;False;Property;_ExtrusionPoint;ExtrusionPoint;0;0;Create;True;0;0;False;0;False;0.1;0.33;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;19;-832,160;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SinOpNode;20;-681.6423,158.0275;Inherit;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;86;-739.3633,-132.2095;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;101;-280.5464,-821.4866;Inherit;True;Property;_BrushedMetalNormal;BrushedMetalNormal;6;0;Create;True;0;0;False;0;False;-1;None;77fdad851e93f394c9f8a1b1a63b56f3;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;100;-233.1846,-579.1099;Float;False;Property;_Distortion;Distortion;7;0;Create;True;0;0;False;0;False;0.292;0.063;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;99;-651.0423,-412.1339;Inherit;False;InstancedProperty;_Axis_z;Axis_z;20;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-704.211,-542.4324;Inherit;False;InstancedProperty;_Axis_y;Axis_y;19;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-553.7132,-583.8194;Inherit;False;InstancedProperty;_Axis_x;Axis_x;18;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;102;27.26553,-918.1992;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;33.05233,-680.1926;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;90;-529.9146,-271.6154;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ComponentMaskNode;105;226.1457,-682.3926;Inherit;False;True;True;False;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;104;282.9456,-902.2924;Inherit;False;True;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-358.3084,-259.7805;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;-378.7583,-153.3248;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-372.1158,-395.8819;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;106;511.3514,-830.1919;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;94;-182.7574,-431.3866;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-770.0161,388.2061;Float;False;Property;_ExtrusionAmount;Extrusion Amount;8;0;Create;True;0;0;False;0;False;1;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;34;-126.835,-266.3055;Inherit;False;InstancedProperty;_Color;Color;9;0;Create;True;0;0;False;0;False;0,0,0,0;0,0.1145825,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;2;-304,0;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;24;-464,158.4107;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;10;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ScreenColorNode;107;698.7264,-881.5917;Float;False;Global;_ScreenGrab0;Screen Grab 0;-1;0;Create;True;0;0;False;0;False;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;88;-92.84756,-68.86089;Inherit;False;Property;_Opacity;Opacity;17;0;Create;True;0;0;False;0;False;0;0.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-55.8,39.39999;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;121;251.4519,-319.5297;Inherit;False;Constant;_Metal;Metal;7;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;208.1775,-222.8232;Inherit;False;Constant;_Smooth;Smooth;6;0;Create;True;0;0;False;0;False;-1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;116;335.8172,-480.1833;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0.4811321,0.4811321,0.4811321,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;124;243.9162,-140.1761;Inherit;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;95;511.3094,-385.0699;Half;False;True;-1;6;ASEMaterialInspector;0;0;Standard;ASESampleShaders/NormalExtrusion;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;False;False;True;True;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;True;Opaque;;Geometry;ForwardOnly;6;d3d9;d3d11_9x;d3d11;glcore;gles;gles3;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;0;3;17.62;25;True;1;False;3;1;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.18;1,1,1,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;11;10;-1;1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;22;0;18;0
WireConnection;22;1;25;1
WireConnection;19;0;22;0
WireConnection;19;1;21;0
WireConnection;20;0;19;0
WireConnection;86;0;20;0
WireConnection;103;0;101;0
WireConnection;103;1;100;0
WireConnection;90;0;86;0
WireConnection;105;0;102;0
WireConnection;104;0;103;0
WireConnection;92;0;98;0
WireConnection;92;1;90;1
WireConnection;89;0;99;0
WireConnection;89;1;90;2
WireConnection;91;0;97;0
WireConnection;91;1;90;0
WireConnection;106;0;104;0
WireConnection;106;1;105;0
WireConnection;94;0;91;0
WireConnection;94;1;92;0
WireConnection;94;2;89;0
WireConnection;24;0;94;0
WireConnection;24;1;3;0
WireConnection;107;0;106;0
WireConnection;4;0;2;0
WireConnection;4;1;24;0
WireConnection;116;0;107;0
WireConnection;116;1;34;0
WireConnection;95;0;116;0
WireConnection;95;2;107;0
WireConnection;95;3;121;0
WireConnection;95;4;96;0
WireConnection;95;7;124;0
WireConnection;95;11;4;0
ASEEND*/
//CHKSM=65A20C8A8E448717A0198BA2A8468E79C9807B7E