Shader "Custom/ToonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Tint("Tint", Color) = (0.5, 0.5, 0.5, 1)
        // The HDR tag allows color values to go outside the [0,1] range of normal color values
        [HDR]
        _Ambient("Ambient", Color) = (0.2, 0.2, 0.2, 1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

			// Populated automatically
            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

			// Passes from vertex to fragment shader
            struct v2f
            {
                float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			float4 _Tint;
            float4 _Ambient;

			fixed4 frag(v2f i) : SV_Target
			{
                // _WorldSpaceLightPos0 gets the main light in the scene, i.e. the directional light
				float NdotL = dot(_WorldSpaceLightPos0, normalize(i.worldNormal));
                float intensity = 0;

                if(NdotL >= 0)
                    intensity = 1;
                else
                    intensity = 0;

                fixed4 textureSample = tex2D(_MainTex, i.uv);

                return _Tint * textureSample * ((intensity * _LightColor0) + _Ambient);
            }
            ENDCG
        }
    }
}
