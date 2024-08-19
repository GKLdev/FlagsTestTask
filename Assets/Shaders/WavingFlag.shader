Shader "Custom/WavingFlag"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
       
        _HeightMap("Height Map", 2D) = "white" {}

        [Header(Settings)]
        _HeightFactor("Displacement factor", Range(-2,2)) = 0.5
        _PinThreshold("Pin Threshold in percent", Range(-0.05, 100.0)) = 0.0
        _PinSmoothless("Pin Threshold Smooth factor", Range(-0.05, 1.0)) = 0.0

        [Header(Diffuse Map movement)]
        _HorAnimSpd("Horizontal speed", float) = 0.0
        _VertAnimSpd("Vertical speed", float) = 0.0

         [Header(Height Map movement)]
        _HMHorAnimSpd("Horizontal speed", float) = 0.0
        _HMVertAnimSpd("Vertical speed", float) = 0.0
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100
        Cull off

        CGPROGRAM

        #pragma surface surf Standard vertex:vert fullforwardshadows addshadow  //adshadow is for SSAO to work correctly
        #pragma target 3.0

        sampler2D   _MainTex;
        sampler2D   _HeightMap;
        half        _Glossiness;
        half        _Metallic;
        fixed4      _Color;
        float       _HeightFactor;
        float4      _HeightMap_ST;

        float _HorAnimSpd;
        float _VertAnimSpd;

        float _HMHorAnimSpd;
        float _HMVertAnimSpd;

        float _PinThreshold;
        float _PinSmoothless;


        // *****************************
        // Input
        // *****************************
        struct Input
        {
            float2 uv_MainTex;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        //UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        //UNITY_INSTANCING_BUFFER_END(Props)

        // *****************************
        // vert
        // *****************************
        void vert(inout appdata_full v) {

            // get uv including tiling and offset
            // v.texcoord.xy would only get us default values
            float2 uv = TRANSFORM_TEX(v.texcoord, _HeightMap);
            
            // apply wave translation //
            uv.x += -_Time.y * _HMHorAnimSpd;
            uv.y += -_Time.y * _HMVertAnimSpd;

            // get values from texture from mipmap 0
            float4  tex                         = tex2Dlod(_HeightMap, float4(uv.xy, 0, 0));
            float   pinThresholdNormalized      = _PinThreshold / 100.0;
            bool    pinEnabled                  = pinThresholdNormalized > 0.0;
            bool    shouldDisplaceVetex         = !pinEnabled || pinEnabled && v.texcoord.x > pinThresholdNormalized;

            // pin or displace //
            if (shouldDisplaceVetex) {
                float pinInfluenceFactor = 0.0; // 1.0 means zero effect on offset, 0.0 - maximum possible effect i. e. no offset, 1-0 will be values which are decreases offset

                // make calculations //
                float   texcoordNormalised      = v.texcoord.x - pinThresholdNormalized;
                bool    ignoreSmoothing         = !pinEnabled || _PinSmoothless < 0.0;

                float smoothFactor = _PinSmoothless < 0.001 ? 1.0f : clamp(texcoordNormalised / _PinSmoothless, 0.0, 1.0);
                pinInfluenceFactor = ignoreSmoothing ? 1.0 : smoothFactor;

                // displace //
                float offset = (tex.r - 0.5) / 1.0;
                v.vertex.xyz += -v.normal * offset * pinInfluenceFactor * _HeightFactor;
            }
        }

        // *****************************
        // surf
        // *****************************
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // cashe uv //
            float2 uv = IN.uv_MainTex;

            // translate //
            uv.x += -_Time.y * _HorAnimSpd;
            uv.y += -_Time.y * _VertAnimSpd;

            // standart PBR thing
            fixed4 color    = tex2D(_MainTex, uv) * _Color;
            o.Albedo        = color.rgb;
            o.Metallic      = _Metallic;
            o.Smoothness    = _Glossiness;
            o.Alpha         = color.a;
        }

        ENDCG
    }

    FallBack "Diffuse"
}
