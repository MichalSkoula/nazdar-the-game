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
		PixelShader = compile ps_3_0 MainPS();
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
	}
};