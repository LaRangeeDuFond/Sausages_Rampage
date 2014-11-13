Shader "Custom/BRDF" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_BRDF ("BRDF (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BRDF

		sampler2D _BRDF;
		float4 _Color;

		struct Input {
			float2 uv_BRDF;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		
		inline float4 LightingBRDF(SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten) {
			float difLight = dot(s.Normal, lightDir);
			float rimLight = dot(s.Normal, viewDir);
			float hLambert = difLight * 0.5 + 0.5;
			float3 ramp = tex2D(_BRDF, float2(hLambert, rimLight)).rgb;
			
			float4 col;
			col.rgb = s.Albedo * _LightColor0.rgb * (ramp);
			col.a = s.Alpha;
			return col;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
