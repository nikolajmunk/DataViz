Shader "Custom/03UVAsColor"
{
	Properties {
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex Vert
			#pragma fragment Frag
		
			struct ToVert
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			struct ToFrag
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0; // Send on first uv coordinate channel.

			};

			ToFrag Vert( ToVert v )
			{
				ToFrag o;
				o.vertex = UnityObjectToClipPos( v.vertex );
				o.uv = v.uv;
				return o;
			}
			
			half4 Frag( ToFrag i ) : SV_Target
			{
				return half4(i.uv, 0, 0); // Same as half4(i.uv.x, i.uv.y, 0, 1)
			}

			ENDCG
		}
	}
}