Shader "Tavio/Sprites/LinearAlphaGradient" {
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        _ColorCount("Color Count", int) = 3
        _AlphaCount("Alpha Count", int) = 3
        [Toggle] _Horizontal("Horizontal", float) = 0
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
        float _Horizontal = 0;

        uniform fixed _Alphas[32];
        uniform fixed _AlphaAnchors[32];
        fixed _CurrentAlpha;
        fixed _NextAlpha;
        int _AlphaCount = 0;

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

            fixed pos = i.uv.y * ( 1 -_Horizontal ) + i.uv.x * _Horizontal;

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
            
            fixed alphaAnchor;
            fixed nextAlphaAnchor;

            for (int i = 0; i < _AlphaCount; i++) {
                alphaAnchor = _AlphaAnchors[i];
                nextAlphaAnchor = _AlphaAnchors[i + 1];
                if (pos >= alphaAnchor && pos < nextAlphaAnchor) {
                    _CurrentAlpha = _Alphas[i];
                    _NextAlpha = _Alphas[i + 1];
                    i = _AlphaCount;
                }
            }
            
            fixed4 result = lerp(_CurrentColor, _NextColor, smoothstep(colorAnchor, nextColorAnchor, pos));
            result.w = lerp(_CurrentAlpha, _NextAlpha, smoothstep(alphaAnchor, nextAlphaAnchor, pos))* oricol.w;
            return result;
        }

        ENDCG
    }
    }
        FallBack "Diffuse"
}
