Shader "Camera/PixelateCameraShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_XPixels ("X Pixels", Float) = 800
		_YPixels ("Y Pixels", Float )=  600
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			float _XPixels;
			float _YPixels;

            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;

				uv.x *= _XPixels;
				uv.y *= _YPixels;

				uv.x = round(uv.x);
				uv.y = round(uv.y);

				uv.x /= _XPixels;
				uv.y /= _YPixels;

				fixed4 col = tex2D(_MainTex, uv);
                
                return col;
            }
            ENDCG
        }
    }
}
