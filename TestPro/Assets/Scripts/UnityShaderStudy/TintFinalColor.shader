// 可以使用“最终颜色修改器”函数来修改着色器计算的最终颜色。表面着色器编译指令 finalcolor:functionName 将用于此目的，其中的一个函数采用 Input IN, SurfaceOutput o, inout fixed4 color 参数。
// 下面是一个简单的着色器，它将色调应用于最终颜色。这与仅对表面反照率颜色应用色调不同：此色调还会影响来自光照贴图、光照探针和类似额外来源的任何颜色。
Shader "Example/Tint Final Color" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorTint ("Tint", Color) = (1.0, 0.6, 0.6, 1.0)
    }
    SubShader {
        Tags { "RenderType" = "Opaque" }
        CGPROGRAM
        #pragma surface surf Lambert finalcolor:mycolor
        struct Input {
            float2 uv_MainTex;
        };
        fixed4 _ColorTint;
        void mycolor (Input IN, SurfaceOutput o, inout fixed4 color)
        {
            color *= _ColorTint;
        }
        sampler2D _MainTex;
        void surf (Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
        }
        ENDCG
    } 
    Fallback "Diffuse"
}