Shader "Custom/FourColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	    _Color1("Color 1", Color) = (1,1,1,1)
	    _Color2("Color 2", Color) = (0.75, 0.75, 0.75, 0.75)
		_Color3("Color 3", Color) = (0.25, 0.25, 0.25, 0.25)
	    _Color4("Color 4", Color) = (0,0,0,0)
			
		_Width("Width", float) = 1920
		_Height("Height", float) = 1080

		_OnOff("0 = Both / 1 = Only Color / 2 = Only Pixel", Range(0,2)) = 0
    } 
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			float4 _Color1;
			float4 _Color2;
			float4 _Color3;
			float4 _Color4;

			float _Width;
			float _Height;

			float _OnOff;

            fixed4 frag (v2f i) : SV_Target
            {
				half ratioX = 1 / _Width;
				half ratioY = 1 / _Height;
				half2 uv = half2((int)(i.uv.x / ratioX) + 0.5f, (int)(i.uv.y / ratioY) + 0.5f );
				uv.x *= ratioX;
				uv.y *= ratioY;

				if (_OnOff > 1 && _OnOff != 2)
					uv = i.uv;

                fixed4 col = tex2D(_MainTex, uv);

				if (_OnOff != 2)
				{
					float sumCol = col.r + col.g + col.b;
					if (sumCol >= 0.75 * 3)
						col.rgb = _Color1.rgb;
					else if (sumCol >= 0.5 * 3)
						col.rgb = _Color2.rgb;
					else if (sumCol >= 0.25 * 3)
						col.rgb = _Color3.rgb;
					else
						col.rgb = _Color4.rgb;
				}
			    

                return col;
            }
            ENDCG
        }
    }
}
