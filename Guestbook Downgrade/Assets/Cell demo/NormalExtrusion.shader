// Upgrade NOTE: upgraded instancing buffer 'ASESampleShadersNormalExtrusion' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASESampleShaders/NormalExtrusion"
{
	Properties
	{
		_ExtrusionPoint("ExtrusionPoint", Float) = 0.1
		_ExtrusionAmount("Extrusion Amount", Float) = 1
		_Color("Color", Color) = (0,0,0,0)
		_Opacity("Opacity", Float) = 0
		_Smooth("Smooth", Float) = 0
		_Axis_x("Axis_x", Float) = 0
		_Axis_y("Axis_y", Float) = 0
		_Axis_z("Axis_z", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
		[Header(Forward Rendering Options)]
		[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		[ToggleOff] _GlossyReflections("Reflections", Float) = 1.0
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Overlay+0" "IgnoreProjector" = "True" }
		Cull Back
		Blend One One
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma shader_feature _SPECULARHIGHLIGHTS_OFF
		#pragma shader_feature _GLOSSYREFLECTIONS_OFF
		struct Input
		{
			half filler;
		};

		uniform float _ExtrusionPoint;
		uniform float _ExtrusionAmount;
		uniform float _Smooth;
		uniform float _Opacity;

		UNITY_INSTANCING_BUFFER_START(ASESampleShadersNormalExtrusion)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
#define _Color_arr ASESampleShadersNormalExtrusion
			UNITY_DEFINE_INSTANCED_PROP(float, _Axis_x)
#define _Axis_x_arr ASESampleShadersNormalExtrusion
			UNITY_DEFINE_INSTANCED_PROP(float, _Axis_y)
#define _Axis_y_arr ASESampleShadersNormalExtrusion
			UNITY_DEFINE_INSTANCED_PROP(float, _Axis_z)
#define _Axis_z_arr ASESampleShadersNormalExtrusion
		UNITY_INSTANCING_BUFFER_END(ASESampleShadersNormalExtrusion)

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float _Axis_x_Instance = UNITY_ACCESS_INSTANCED_PROP(_Axis_x_arr, _Axis_x);
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 break90 = ( sin( ( ( ase_vertex3Pos + _Time.x ) / _ExtrusionPoint ) ) + float3( 1,1,1 ) );
			float _Axis_y_Instance = UNITY_ACCESS_INSTANCED_PROP(_Axis_y_arr, _Axis_y);
			float _Axis_z_Instance = UNITY_ACCESS_INSTANCED_PROP(_Axis_z_arr, _Axis_z);
			float4 appendResult94 = (float4(( _Axis_x_Instance * break90.x ) , ( _Axis_y_Instance * break90.y ) , ( _Axis_z_Instance * break90.z ) , 0.0));
			float4 temp_output_24_0 = ( appendResult94 / _ExtrusionAmount );
			v.vertex.xyz += ( float4( ase_vertexNormal , 0.0 ) * temp_output_24_0 ).xyz;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float4 _Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color);
			o.Albedo = _Color_Instance.rgb;
			o.Smoothness = _Smooth;
			o.Alpha = _Opacity;
		}

		ENDCG
		CGPROGRAM
		#pragma only_renderers d3d9 d3d11_9x d3d11 glcore gles gles3 
		#pragma surface surf StandardSpecular keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18600
6;67;1080;855;1281.775;792.5349;1.526584;True;False
Node;AmplifyShaderEditor.TimeNode;25;-1312,272;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;18;-1371.541,72.25986;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-1056,160;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1052.238,256;Float;False;Property;_ExtrusionPoint;ExtrusionPoint;0;0;Create;True;0;0;False;0;False;0.1;0.04661835;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;19;-832,160;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SinOpNode;20;-681.6423,158.0275;Inherit;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;86;-739.3633,-132.2095;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-704.211,-542.4324;Inherit;False;InstancedProperty;_Axis_y;Axis_y;7;0;Create;True;0;0;False;0;False;0;0.4343104;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;99;-651.0423,-412.1339;Inherit;False;InstancedProperty;_Axis_z;Axis_z;8;0;Create;True;0;0;False;0;False;0;0.7746398;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-553.7132,-583.8194;Inherit;False;InstancedProperty;_Axis_x;Axis_x;6;0;Create;True;0;0;False;0;False;0;0.1653674;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;90;-529.9146,-271.6154;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-358.3084,-259.7805;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-372.1158,-395.8819;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;-378.7583,-153.3248;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;94;-182.7574,-431.3866;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-770.0161,388.2061;Float;False;Property;_ExtrusionAmount;Extrusion Amount;2;0;Create;True;0;0;False;0;False;1;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;2;-304,0;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;24;-464,158.4107;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;10;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;34;-136.0389,-235.0122;Inherit;False;InstancedProperty;_Color;Color;3;0;Create;True;0;0;False;0;False;0,0,0,0;1,0,0.3581705,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMaxOpNode;26;-297,234;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-55.8,39.39999;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;88;-104.6302,-35.86948;Inherit;False;Property;_Opacity;Opacity;4;0;Create;True;0;0;False;0;False;0;0.64;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-15.09677,-435.3322;Inherit;False;Property;_Smooth;Smooth;5;0;Create;True;0;0;False;0;False;0;0.92;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;95;128,-257.3;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;ASESampleShaders/NormalExtrusion;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;True;True;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Overlay;ForwardOnly;6;d3d9;d3d11_9x;d3d11;glcore;gles;gles3;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;4;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;22;0;18;0
WireConnection;22;1;25;1
WireConnection;19;0;22;0
WireConnection;19;1;21;0
WireConnection;20;0;19;0
WireConnection;86;0;20;0
WireConnection;90;0;86;0
WireConnection;92;0;98;0
WireConnection;92;1;90;1
WireConnection;91;0;97;0
WireConnection;91;1;90;0
WireConnection;89;0;99;0
WireConnection;89;1;90;2
WireConnection;94;0;91;0
WireConnection;94;1;92;0
WireConnection;94;2;89;0
WireConnection;24;0;94;0
WireConnection;24;1;3;0
WireConnection;26;0;24;0
WireConnection;4;0;2;0
WireConnection;4;1;24;0
WireConnection;95;0;34;0
WireConnection;95;4;96;0
WireConnection;95;9;88;0
WireConnection;95;11;4;0
ASEEND*/
//CHKSM=EBBFCB97C49BD4360A3ECC14CD23990E91722D4D