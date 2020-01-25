Shader "Sky/Clouds"
{
    Properties
    {
        _NoiseTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
				float3 worldPos: TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _NoiseTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float2 scrollUVOne = i.uv, scrollUVTwo = i.uv;

				scrollUVOne.x += _Time.x;
				scrollUVOne.y += _Time.x;

				scrollUVTwo.x += _Time.x * 0.314;
				scrollUVTwo.y += _Time.x * 0.395;

                // sample the texture twice and average the outcome at different uv coords
				fixed4 col = (tex2D(_NoiseTex, scrollUVOne) + tex2D(_NoiseTex, scrollUVTwo)) * 0.4;

				float dist = distance(i.worldPos, _WorldSpaceCameraPos);
				float calculatedValue = 0.6/exp(0.002*dist*dist);
				col.r *= calculatedValue;
				col.g *= calculatedValue;
				col.b *= calculatedValue;

                return col;
            }
            ENDCG
        }
    }
}
