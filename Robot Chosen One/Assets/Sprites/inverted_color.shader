//Made by Gemini
Shader "Custom/inverted_color"
{
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent+1"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Cull Off
		ZWrite Off
		ZTest Always
		// This line handles the mathematical inversion of the screen pixels underneath
		Blend OneMinusDstColor Zero

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
			};

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.color = IN.color;
				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				return fixed4(1, 1, 1, 1);
			}
			ENDCG
		}
	}
}