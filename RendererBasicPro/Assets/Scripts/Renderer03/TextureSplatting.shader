Shader "Custom/TextureSplatting"
{
	Properties {
		// 大括号是做什么用的？ 以前，旧的固定功能着色器具有纹理设置，但现在不再使用。这些设置就是放在这些括号内。
		_MainTex ("Splat Map", 2D) = "white" { }
		[NoScaleOffset] _Texture1 ("Texture 1", 2D) = "white" { }
		[NoScaleOffset] _Texture2 ("Texture 2", 2D) = "white" { }
		[NoScaleOffset] _Texture3 ("Texture 3", 2D) = "white" { }
		[NoScaleOffset] _Texture4 ("Texture 4", 2D) = "white" { }
	}
	SubShader {
		Pass {
			CGPROGRAM

			#pragma vertex myVertexProgram
			#pragma fragment myFragmentProgram

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			// _ST后缀代表“缩放”和“平移”或类似名称。为什么不使用_TO来指代平铺和偏移？因为Unity一直使用_ST，并且向后兼容要求它保持这种方式，哪怕术语可能已更改了。
			float4 _MainTex_ST;

			sampler2D _Texture1, _Texture2, _Texture3, _Texture4;

			struct VertexData {
				float4 position 	:	POSITION;
				// Unity的默认网格物体具有适合纹理贴图的UV坐标。顶点程序可以通过具有TEXCOORD0语义的参数访问它们。
				float2 uv 			: 	TEXCOORD0;
			};

			// 顶点输入的数据插值的结构体
			struct Interpolators {
				// 通过将SV_POSITION语义附加到我们的方法来表明返回的数据代表什么。SV代表系统值，POSITION代表最终顶点位置。
				float4 position 	: 	SV_POSITION;
				float2 uv 			: 	TEXCOORD0;
				float2 uvSplat 		: 	TEXCOORD1;
			};

			Interpolators myVertexProgram (VertexData v)
			{
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);
				i.uv =TRANSFORM_TEX(v.uv, _MainTex);
				i.uvSplat = v.uv;
				return i;
			}

			// SV_TARGET : 默认的着色器目标，指出最终颜色应写入的位置。是帧缓冲区，其中包含正在生成的图像。
			// 由于默认的对象空间半径为½，因此颜色通道的最终位置介于-½至½之间。将颜色调整至[0,1]
			float4 myFragmentProgram (Interpolators i) : SV_TARGET
			{
				// 对纹理进行采样
				float4 splat = tex2D(_MainTex, i.uvSplat);
				float4 color = tex2D(_Texture1, i.uv) * splat.r;
				color += tex2D(_Texture2, i.uv) * splat.g;
				color += tex2D(_Texture3, i.uv) * splat.b;
				color += tex2D(_Texture4, i.uv) * (1 - splat.r - splat.g - splat.b);
				return color;
			}

			ENDCG
		}
	}
}
