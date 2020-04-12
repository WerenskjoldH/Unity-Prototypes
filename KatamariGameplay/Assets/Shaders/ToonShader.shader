Shader "Custom/ToonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Tint("Tint", Color) = (0.5, 0.5, 0.5, 1)
        // The HDR tag allows color values to go outside the [0,1] range of normal color values
        [HDR]
        _Ambient("Ambient", Color) = (0.2, 0.2, 0.2, 1)
        [HDR]
        _Specular("Specular", Color) = (0.95, 0.95, 0.95, 1)
        _Glossiness("Glossiness", Float) = 32
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
                float3 viewDirection : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDirection = WorldSpaceViewDir(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			float4 _Tint;
            float4 _Ambient;
            float4 _Specular;
            float _Glossiness;

			fixed4 frag(v2f i) : SV_Target
			{
                fixed4 textureSample = tex2D(_MainTex, i.uv);
                float3 normal = normalize(i.worldNormal);

                float3 viewDirection = normalize(i.viewDirection);

                float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDirection);
                float NdotH = dot(normal, halfVector);

                // _WorldSpaceLightPos0 gets the main light in the scene, i.e. the directional light
				float NdotL = dot(normal, _WorldSpaceLightPos0);
                
                float intensity = smoothstep(0, 0.02, NdotL);
                float specularIntensity = pow(NdotH * intensity, _Glossiness * _Glossiness);
                specularIntensity = smoothstep(0.005, 0.01, specularIntensity);
                float4 specular = specularIntensity * _Specular;


                float4 lightColor = specular + (intensity * _LightColor0) + _Ambient;

                return _Tint * textureSample * lightColor;
            }
            ENDCG
        }
    }
}
