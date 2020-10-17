Shader "Example/Diffuse Texture" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType" = "Opaque" }
        CGPROGRAM
        
        // #pragma surface surf Lambert

        // 计算表面法线和光线方向之间的点积，然后应用光衰减和颜色
        #pragma surface surf SimpleLambert
        half4 LightingSimpleLambert (SurfaceOutput s, half3 lightDir, half atten) {
            half NdotL = dot (s.Normal, lightDir);
            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten);
            c.a = s.Alpha;
            return c;
        }

        struct Input {
            float2 uv_MainTex;
        };
        
        sampler2D _MainTex;
        
        void surf (Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
        }
        
        ENDCG
    }
    Fallback "Diffuse"
}