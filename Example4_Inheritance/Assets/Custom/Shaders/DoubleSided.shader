Shader "Custom/DoubleSided" {
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _BumpMap("NormalMap", 2D) = "Bump" {}
        _BumpScale("Normal Power", Float) = 1.0
        _NormalDefault("Normal Default", Color) = (128, 128, 255)
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 200

            // Render back faces first
            Cull Front
            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

            // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

            sampler2D _MainTex;
        sampler2D _BumpMap;

        struct Input {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _BumpScale;
        half _Metallic;
        fixed4 _Color;
        half3 _NormalDefault;

        void surf(Input IN, inout SurfaceOutputStandard o) {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Normal = -UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
            //o.Normal = -_NormalDefault;
            o.Alpha = c.a;
        }
        ENDCG


            // Now render front faces
            Cull Back
        CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf Standard fullforwardshadows

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0

            sampler2D _MainTex;
            sampler2D _BumpMap;

            struct Input {
                float2 uv_MainTex;
            };

            half _Glossiness;
            half _BumpScale;
            half _Metallic;
            fixed4 _Color;
            half3 _NormalDefault;

            void surf(Input IN, inout SurfaceOutputStandard o) {
                // Albedo comes from a texture tinted by color
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb;
                // Metallic and smoothness come from slider variables
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
                //o.Normal = _NormalDefault;
                o.Alpha = c.a;
            }
            ENDCG
        }
            FallBack "Diffuse"
}