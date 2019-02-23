Shader "Tavio/Sprites/LinearGradient" {
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        _ColorCount("Color Count", int) = 3
    }
    SubShader{
        Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
        Cull Off

        Blend SrcAlpha OneMinusSrcAlpha
        Pass{

        CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

        sampler2D _MainTex;
        uniform fixed4 _Colors[32];
        uniform fixed _ColorAnchors[32];
        fixed4 _CurrentColor;
        fixed4 _NextColor;
        int _ColorCount = 0;

        struct v2f {
            float4 pos : SV_POSITION;
            half2 uv : TEXCOORD0;
        };

        v2f vert(appdata_base v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = v.texcoord;
            return o;
        }

        fixed4 frag(v2f i) : COLOR
        {
            fixed4 oricol = tex2D(_MainTex, i.uv);
            fixed pos = i.uv.y;

            fixed colorAnchor;
            fixed nextColorAnchor;

            for (int i = 0; i < _ColorCount; i++) {
                colorAnchor = _ColorAnchors[i];
                nextColorAnchor = _ColorAnchors[i + 1];
                if (pos >= colorAnchor && pos < nextColorAnchor) {
                    _CurrentColor = _Colors[i];
                    _NextColor = _Colors[i + 1];
                    i = _ColorCount;
                }
            }
            
            fixed4 result = lerp(_CurrentColor, _NextColor, smoothstep(colorAnchor, nextColorAnchor, pos));
            return result;
        }

        ENDCG
    }
    }
        FallBack "Diffuse"
}
