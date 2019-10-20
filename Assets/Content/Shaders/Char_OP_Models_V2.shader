// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'


Shader "OP/Char/Models_V2" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		//_RampTex("Ramp", 2D) = "white"{}
		_BandWidth("Band Width", Float) = 8
		

		_OutlineColor("Outline Color",color) = (0,0,0,1)
		_Outline("Outline Width",range(0, 0.1)) = 0.0013		// 挤出描边的粗细
		_OutlineMin("Outline Min", Float) = 0.5
		_OutlineMax("Outline Max", Float) = 5
		_OutlineWidth("Outline Width",range(0, 1.0)) = 0.1

		[HideInInspector] _OutlineParam("OutlineParamType", Float) = 1.0
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 2
		//_BrightnessFactor("MinBrightness", range(0, 1)) = 0
		//
		//_RimColor ("Rim Color", Color) = (1, 1, 1, 1)
		//_RimWidth ("Rim Width", Range(0.0,1.0)) = 0.7
		//_RimColorCoeff ("Rim Color Coeff", Range(0.0, 20.0)) = 1.0 //边缘轮廓亮度
  //      _RimParameter("Rim Parameter(r取0、1表示开关)", Vector) = (0,0,0,0)
  //      _FlashColor("Flash Color", Color) = (1,1,1,1)
  //      _FlashFreq("Flash Frequence", Range(0, 10000)) = 0       
	}
	SubShader 
	{
		Tags{ "RenderType"="Opaque" "Queue" = "Geometry+100" "MobileShadow" = "Character" "FastShadow" = "Opaque" "PostOutline" = "Char" }

		pass
		{
			Name "OUTLINE"
			Tags{ "RenderType"="Opaque" "LightMode" = "Always" }
			Cull Front
			ZWrite On
			
			CGPROGRAM
			#pragma vertex vert_outline
			#pragma fragment frag_outline
			#pragma multi_compile __ OUTLINE_TANGENT
			#pragma only_renderers gles gles3 d3d9 glcore
			#include "UnityCG.cginc"
			#include "__Outline.cginc"
			ENDCG
		}
		
		pass 
		{
			Name "TOON"
			//平行光的的pass渲染
			Tags{ "RenderType"="Opaque" "LightMode"="ForwardBase"}
			Cull[_Cull]
			//Lighting Off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#pragma multi_compile __ DAYFADECOLORCHAR
			#pragma only_renderers gles gles3 d3d9 glcore
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "__CharV2.cginc"
			ENDCG
		}


		//pass {
		//	Name "Rim"
		//	Tags{ "RenderType"="Opaque" "LightMode"="Always"}
		//	Lighting Off
		//	Cull Back
		//	Blend One OneMinusSrcAlpha
		//	ZWrite Off

		//	CGPROGRAM
		//	#pragma vertex vert
		//	#pragma fragment frag
		//	#include "UnityCG.cginc"
		//	#include "Lighting.cginc"

		//	float4 _RimColor;
		//	float _RimWidth;
		//	float _RimColorCoeff;

		//	float _Outline;
		//	struct v2f {
		//		float4 pos:SV_POSITION;
		//		float4 color:COLOR;
		//	};

		//	v2f vert (appdata_full v) {
		//		v2f o;
		//		float4 pos = mul(UNITY_MATRIX_MV, v.vertex); 
		//		float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);  
		//		normal.z = -0.5;

		//		float3 viewDir = ObjSpaceViewDir(v.vertex);
		//		float dst = length(viewDir);
		//		pos = pos + float4(normalize(normal), 0) * _Outline * v.color.b * clamp(dst, 0.5, 2);
		//		o.pos = mul(UNITY_MATRIX_P, pos);

		//		float mulView = max(0.0, dot(normalize(v.normal), normalize(viewDir)));
		//		mulView = lerp(0, 1, step(0.6, mulView));
		//		o.color.rgb = _RimColor.rgb * (1 - mulView) * _RimWidth * _RimColorCoeff;
		//		o.color.a = min(1 - mulView, _RimColorCoeff);
		//		return o;
		//	}

		//	half4 frag(v2f i) : COLOR
		//	{
		//		return i.color;
		//	}
		//	ENDCG
		//}
	}

	CustomEditor "CharShaderGUI_v2"
}