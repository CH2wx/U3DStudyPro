Shader "Custom/NewSurfaceShader" {
	Properties {
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0

		struct Input {
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo：物质的漫反射率的颜色。rgba值范围在[0, 1]
			// 因为世界坐标向量值在[-1, 1]，所以把向量值进行控制，使范围也在[0, 1]，颜色为负会变成黑色
			// o.Albedo.rgb = IN.worldPos.xyz * 0.5 + float3(0.5, 0.5, 0.5);
			o.Albedo.rgb = IN.worldPos.xyz * 0.5 + 0.5;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
