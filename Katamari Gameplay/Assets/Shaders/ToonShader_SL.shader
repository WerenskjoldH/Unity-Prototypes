Shader "Custom/ToonShader"
{
    // https://roystan.net/articles/toon-shader.html - Made following this
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Tint("Tint", Color) = (0.5, 0.5, 0.5, 1)
        // The HDR tag allows color values to go outside the [0,1] range of normal color values
        [HDR]
        _Ambient("Ambient", Color) = (0.2, 0.2, 0.2, 1)
        [HDR]
        _Specular("Specular", Color) = (0.95, 0.95, 0.95, 1)
        _GlossinessAmount("Glossiness Amount", Float) = 32
        [HDR]
        _RimColor("Rim Color", Color) = (1, 1, 1, 1)
        _RimAmount("Rim Amount", Range(0, 1)) = 0.716
        _RimThreshold("Rim Threshold", Range(0, 1)) = 0.1

    }
    SubShader
    {
        Pass
        {
            Tags { "RenderType"="Opaque" }
            LOD 100


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // Built-in shortcut, makes our lives easier for sampling shadow maps
            // It lets us handle cases where we may or may not want to cast shadows from the directional light source by compiling everything necessary for forward-based rendering
            #pragma multi_compile_fwdbase
            // #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            // #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE


            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

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
                // Generate a 4D value to store the shadow map
                //SHADOW_COORDS(2)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                // Transforms vertex space to shadow map space and stores it in SHADOW_COORD
                TRANSFER_SHADOW(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDirection = WorldSpaceViewDir(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			float4 _Tint;
            float4 _Ambient;

            float4 _Specular;
            float _GlossinessAmount;

            float4 _RimColor;
            float _RimAmount;
            float _RimThreshold;

			fixed4 frag(v2f i) : SV_Target
			{
                fixed4 textureSample = tex2D(_MainTex, i.uv);
                float3 normal = normalize(i.worldNormal);

                float3 viewDirection = normalize(i.viewDirection);

                float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDirection);
                float NdotH = dot(normal, halfVector);

                // _WorldSpaceLightPos0 gets the main light in the scene, i.e. the directional light
				float NdotL = dot(normal, _WorldSpaceLightPos0);
                
                // This is a shaderlab macro that returns a value [0, 1] describing how much shadow is covering an object
                float shadow = SHADOW_ATTENUATION(i);

                float intensity = smoothstep(0, 0.01, NdotL * shadow);
                float specularIntensity = pow(NdotH * intensity, _GlossinessAmount * _GlossinessAmount);
                specularIntensity = smoothstep(0.005, 0.01, specularIntensity);
                float4 specular = specularIntensity * _Specular;

                float4 inverseDot = 1 - dot(viewDirection, normal);
                float rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, inverseDot * pow(NdotL, _RimThreshold));
                float4 rim = rimIntensity * _RimColor; 
                

                float4 lightColor = specular + (intensity * _LightColor0) + _Ambient + rim;

                return _Tint * textureSample * lightColor;
            }
            ENDCG
        }
        // Allows our object to cast shadows
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
