Shader "Unlit/MyShader"
{
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f {
                float4 pos : SV_POSITION;
                fixed3 color : COLOR0;
            };

            void vert (appdata_base v, out v2f o)
            {
                // v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.normal * 0.5 + 0.5;
                // return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4 (i.color, 1);
            }
			ENDCG
		}
	}
}
