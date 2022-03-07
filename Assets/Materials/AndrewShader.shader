Shader "AndrewShader"
{
    Properties
    {
        _MainText ("Texture", 2D) = "white" {}
        _Color ("Colour", Color) = (1,1,1,1)
    }
    SubShader
    {

        Pass
        {
            CGPROGRAM
            #pragma vertex vertexFunc
            #pragma fragment fragmentFunc

            #include "UnityCG.cginc"

            struct appdata {
                float vertex : POSTION;
                float uv : TEXCOORD0;
            };

            struct v2f {
                float position : SV_POSITION;
                float uv : TEXCOORD0;
            };
            fixed4 _Color;
            sampler2D _MainTexture;

            v2f vertexFunc(appdata IN)
            {
                v2f OUT;

                OUT.position = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.uv;

                return OUT
            }

            fixed4 fragmentFunc(v2f IN) : SV_Target
            {
                fixed4 pixelColor = text2D(_MainTexture, IN.uv);

                return pixelColor * _Color;
            }

            ENDCG
        }
    }
}
