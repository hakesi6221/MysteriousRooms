Shader "Custom/DissolveShader"
{
    Properties
    {
        _MainTex ("メインテクスチャ", 2D) = "white" {}
        _DissolveTex ("ノイズテクスチャ", 2D) = "white" {}
        _Color ("メインカラー", Color) = (1,1,1,1)
        _DissolveAmount ("ディゾルブの割合", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "RenderPipeline"="UniversalPipeline"
            "PreviewType"="Plane"
        }

        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            sampler2D _DissolveTex;
            float4 _MainTex_ST;
            float4 _DissolveTex_ST;
            float4 _Color;
            float _DissolveAmount;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

                // テクスチャをuvとして出力
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                // パラメーターのカラーを乗算
                OUT.color = IN.color * _Color;

                return OUT;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // メインのテクスチャの色を決定
                half4 mainColor = tex2D(_MainTex, i.uv) * i.color;
                // ノイズテクスチャの1値を取り出し、1,0の2値化する
                half noise = tex2D(_DissolveTex, i.uv).r;
                half alpha = step(_DissolveAmount, noise);

                // 閾値によって1, 0に分かれたノイズの値をα値に乗算
                mainColor.a *= alpha;

                return mainColor;
            }
            ENDHLSL

        }
    }
}