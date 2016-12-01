Shader "Labertorium/DeformShader" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
	// _MainNormalTex ("Normal", 2D) = "bump" {} // unused to keep shader more simple
	_SecondTex("Second Texture", 2D) = "white" {}
	// _SecondNormalTex ("Second Normal", 2D) = "bump" {}// unused to keep shader more simple
	_ThirdTex("Third Texture", 2D) = "white" {}
	// _ThirdNormalTex ("Third Normal", 2D) = "bump" {}// unused to keep shader more simple
	// _Amount ("Extrusion Amount", Range(-1,1)) = 0.5 // unused as script no longer access it
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM
#pragma surface surf Lambert vertex:vert

		// declare input structure with uv and color fields	
		struct Input
	{
		float2 uv_MainTex;
		float4 color;
	};

	void vert(inout appdata_full v,out Input o)
	{
		UNITY_INITIALIZE_OUTPUT(Input,o);
		o.color = v.color; // pass the vertex color to the output (required for blending textures)
						   // for deformation on the graphics card the vertex shader could be used
						   // as this does not affect the physics the following lines are commented out
						   // but left for easier legacy support
						   // v.vertex.xyz += v.normal * v.color.a * _Amount; // move in normal direction
						   // v.vertex.xyz += float3(0, 1, 0) * v.color.a * _Amount; // move down
	}

	sampler2D _MainTex; //, _MainNormalTex;
	sampler2D _SecondTex; //, _SecondNormalTex;
	sampler2D _ThirdTex; //, _ThirdNormalTex;

						 // remaps a value to the range specified by outLower and outUpper
						 // using the inLower and inUpper as reference range
	float Remap(float val, float inLower, float inUpper, float outLower, float outUpper)
	{
		return outLower + (val - inLower) * (outUpper - outLower) / (inUpper - inLower);
	}

	void surf(Input IN, inout SurfaceOutput o)
	{
		// legacy which uses the uv scrolling
		float2 scroll = float2(_Time.x * 0.064f * (1 - IN.color.a), _Time.y * 0.04f * (1 - IN.color.a));

		// get rgb + alpha for all three textures
		float4 main = tex2D(_MainTex, IN.uv_MainTex);
		// the scroll offset is applied to the second to have a scrolling uv-animation
		float4 second = tex2D(_SecondTex, IN.uv_MainTex + scroll * 4);
		// apply scroll to this one too if the texture should be animated!
		float4 third = tex2D(_ThirdTex, IN.uv_MainTex);

		// remap the vertex colors alpha (set via deformable script)
		// to use it as blend factor for the first and second texture
		float blend1To2 = Remap(IN.color.a, 0.25, 1.0, 0.0, 1.0);
		// remap the vertex colors alpha (set via deformable script) 
		// to use it as blend factor for the second and third
		float blend2To3 = Remap(IN.color.a, 0.0, 0.25, 0.0, 1.0);

		// init c with white
		float3 c = float3(1, 1, 1);
		// if the vertex color's alpha is above .. blend the first and second texture
		if (IN.color.a > 0.25)
			c = lerp(second.rgb, main.rgb, blend1To2);

		// if the vertex color's alpha is below .. blend the first and second texture
		if (IN.color.a < 0.25)
			c = lerp(third.rgb, second.rgb, blend2To3);

		// put the color to the out's albedo value
		o.Albedo = c;

		// you could use emission but it doesn't make sense for the examples
		// o.Emission = c * (1 - IN.color.a);
	}
	ENDCG
	}
		Fallback "Diffuse"
}