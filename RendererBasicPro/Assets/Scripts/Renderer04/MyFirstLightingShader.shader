// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/My First Lighting Shader"
{
	Properties {
		_Tint ("Tint", Color) = (1, 1, 1, 1)
		// 大括号是做什么用的？ 以前，旧的固定功能着色器具有纹理设置，但现在不再使用。这些设置就是放在这些括号内。
		_MainTex ("Albedo", 2D) = "white" { }
		// 光滑度
		_Smoothness ("Smoothness", Range(0, 1)) = 0.5
	}
	SubShader {
		Pass {
			Tags {
				"LightMode" = "ForwardBase"
			}

			CGPROGRAM

			#pragma vertex myVertexProgram
			#pragma fragment myFragmentProgram

			// #include "UnityCG.cginc"	// UnityStandardBRDF.cginc包含了UnityCG.cginc，因此可以注释掉
			#include "UnityStandardBRDF.cginc"

			float4 _Tint;
			sampler2D _MainTex;
			// _ST后缀代表“缩放”和“平移”或类似名称。为什么不使用_TO来指代平铺和偏移？因为Unity一直使用_ST，并且向后兼容要求它保持这种方式，哪怕术语可能已更改了。
			float4 _MainTex_ST;
			float _Smoothness;

			struct VertexData {
				float4 position : POSITION;
				// 得到网格包含的顶点法线向量
				float3 normal : NORMAL;
				// Unity的默认网格物体具有适合纹理贴图的UV坐标。顶点程序可以通过具有TEXCOORD0语义的参数访问它们。
				float2 uv : TEXCOORD0;
			};

			// 顶点输入的数据插值的结构体
			struct Interpolators {
				// 通过将SV_POSITION语义附加到我们的方法来表明返回的数据代表什么。SV代表系统值，POSITION代表最终顶点位置。
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
			};

			Interpolators myVertexProgram (VertexData v)
			{
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);
				i.worldPos = mul(unity_ObjectToWorld, v.position);
				i.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
				// 将模型的法线转化为世界坐标的法线，因为法线表示方向不在意坐标，所有第四齐次坐标为0
				// i.normal = mul(
				// 				transpose((float3x3)unity_ObjectToWorld),
				// 				float4(v.normal, 0)
				// 			);
				// 通过UnityCG包含一个方便的UnityObjectToWorldNormal函数，该函数帮助我们完成上述任务
				i.normal = UnityObjectToWorldNormal(v.normal);
				i.normal = normalize(i.normal);
				// 逻辑同上，该写法只做了3x3的矩阵相乘
				// i.normal = mul((float3x3)unity_ObjectToWorld, v.normal);
				return i;
			}

			// SV_TARGET : 默认的着色器目标，指出最终颜色应写入的位置。是帧缓冲区，其中包含正在生成的图像。
			// 由于默认的对象空间半径为½，因此颜色通道的最终位置介于-½至½之间。将颜色调整至[0,1]
			float4 myFragmentProgram (Interpolators i) : SV_TARGET
			{
				// 在顶点程序中生成正确的法线后，它们将通过插值器传递。不过，由于不同单位长度向量之间的线性内插不会产生另一个单位长度向量。它会更短。所以需要在着色器中再次归一化
				i.normal = normalize(i.normal);
				// return float4(i.normal * 0.5 + 0.5, 1.0);

				// 光的方向
				float3 lightDir = _WorldSpaceLightPos0.xyz;
				// 视野方向
				float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
				// 反射方向
				float3 reflectionDir = reflect(-lightDir, i.normal);
				// 光的颜色
				float3 lightColor = _LightColor0.xyz;
				// 定义材质的漫反射率的颜色（反照率），描述了RGB三色被表面反射了多少，其余则被吸收。通过材质的纹理和色调来控制。
				float3 albedo = tex2D(_MainTex, i.uv).rgb * _Tint.rgb;
				// DotClamped函数：执行点积，并确保它永远不会为负。等同于：max(0, dot(lightDir, i.normal))
				// 得到漫反射颜色
				float3 diffuse = albedo * lightColor * DotClamped(lightDir, i.normal);
				// return float4(diffuse, 1);
				return pow(
						DotClamped(viewDir, i.normal),
						_Smoothness * 100
				);
			}

			ENDCG
		}
	}
}
