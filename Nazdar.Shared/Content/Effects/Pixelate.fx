#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler inputTexture;
int pixelation;

float4 MainPS(float2 originalUV: TEXCOORD0): COLOR0
{
	// my game runs at 960x540; change to reflect the resolution YOUR game runs at
	originalUV *= float2(1280, 720);
		
	float2 newUV;
	newUV.x = round(originalUV.x / pixelation) * pixelation;
	newUV.y = round(originalUV.y / pixelation) * pixelation;
		
	// again: change this to match your screen's resolution
	newUV /= float2(1280, 720);
		
	return tex2D(inputTexture, newUV);
}

technique Techninque1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};