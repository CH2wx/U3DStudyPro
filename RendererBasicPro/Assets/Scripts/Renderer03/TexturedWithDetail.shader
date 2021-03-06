﻿Shader "Custom/Textured With Detail"
{
	Properties {
		_Tint ("Tint", Color) = (1, 1, 1, 1)
		// 大括号是做什么用的？ 以前，旧的固定功能着色器具有纹理设置，但现在不再使用。这些设置就是放在这些括号内。
		_MainTex ("Texture", 2D) = "white" { }
		_DetailTex ("Detail Texture", 2D) = "white" { }
	}
	SubShader {
		Pass {
			CGPROGRAM

			#pragma vertex myVertexProgram
			#pragma fragment myFragmentProgram

			#include "UnityCG.cginc"

			float4 _Tint;
			sampler2D _MainTex;
			sampler2D _DetailTex;
			// _ST后缀代表“缩放”和“平移”或类似名称。为什么不使用_TO来指代平铺和偏移？因为Unity一直使用_ST，并且向后兼容要求它保持这种方式，哪怕术语可能已更改了。
			float4 _MainTex_ST, _DetailTex_ST;

			struct VertexData {
				float4 position : POSITION;
				// Unity的默认网格物体具有适合纹理贴图的UV坐标。顶点程序可以通过具有TEXCOORD0语义的参数访问它们。
				float2 uv : TEXCOORD0;
			};

			// 顶点输入的数据插值的结构体
			struct Interpolators {
				// 通过将SV_POSITION语义附加到我们的方法来表明返回的数据代表什么。SV代表系统值，POSITION代表最终顶点位置。
				float4 position 	: 	SV_POSITION;
				float2 uv 			: 	TEXCOORD0;
				float2 uvDetail 	:	TEXCOORD1;
			};

			Interpolators myVertexProgram (VertexData v)
			{
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);
				i.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
				// 通过使用细节纹理的平铺和偏移来转换原始UV，可以创建新的细节UV。
				i.uvDetail = TRANSFORM_TEX(i.uv, _DetailTex);
				return i;
			}

			// SV_TARGET : 默认的着色器目标，指出最终颜色应写入的位置。是帧缓冲区，其中包含正在生成的图像。
			// 由于默认的对象空间半径为½，因此颜色通道的最终位置介于-½至½之间。将颜色调整至[0,1]
			float4 myFragmentProgram (Interpolators i) : SV_TARGET
			{
				// float4 tint = _Tint * _Tint * 2;	// 加深黑白对比色差
				// 对纹理进行采样
				float4 color = tex2D(_MainTex, i.uv) * _Tint;
				// unity_ColorSpaceDouble，一个Unity定义的统一变量，值为2或者大约4.59（1 /（½2.2）(½的2.2次幂)≈4.59）
				// 防止从伽马转换为线性空间，着色器始终访问原始图像数据（导致亮度的不一致）
				color *= tex2D(_DetailTex, i.uvDetail) * unity_ColorSpaceDouble;
				return color;
			}

			ENDCG
		}
	}
}
