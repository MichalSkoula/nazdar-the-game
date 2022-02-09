#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler inputTexture;

float4 MainPS(float2 textureCoordinates: TEXCOORD0): COLOR0
{
	float4 color = tex2D(inputTexture, textureCoordinates);
	color.rgb = 1.0f;
	return color;
}

technique Techninque1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
	}
};