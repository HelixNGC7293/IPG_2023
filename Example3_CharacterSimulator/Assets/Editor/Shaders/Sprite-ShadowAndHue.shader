    Shader "Outlined/Sprite-Shadow And Hue" {
        Properties {
            [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
			_Color ("Tint", Color) = (1,1,1,1)
			[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
			_Cutoff("Shadow alpha cutoff", Range(0,1)) = 0.5
        	_HueShift("HueShift", Float ) = 0
       	 	_Sat("Saturation", Float) = 1
       		 _Val("Value", Float) = 1

			// Add values to determine if outlining is enabled and outline color. [PerRendererData]
			[MaterialToggle]_Outline("Outline", Float) = 0
			_OutlineColor("Outline Color", Color) = (1,1,1,1)
			_OutlineSize("Outline Size", int) = 1
        }
	    CGINCLUDE
	    #include "UnityCG.cginc"

		float _Cutoff;
		uniform sampler2D _MainTex;
		fixed4 _Color;
	    uniform float4 _SihouettleColor;
	    
	    ENDCG

        SubShader {
            Tags {  "Queue"="Transparent" 
					"IgnoreProjector"="True" 
					"RenderType"="Transparent" 
					"PreviewType"="Plane"
					"CanUseSpriteAtlas"="True" }
            // note that a vertex shader is specified here but its using the one above
			
            Pass {
                Name "SPRITE"
                Tags { "LightMode" = "Always" }
                //Offset -100, -100
                Cull Off
                ZWrite Off
                ColorMask RGB // alpha not used
                // you can choose what kind of blending mode you want for the Sihouettle
                Blend SrcAlpha OneMinusSrcAlpha // Normal
                //Blend One OneMinusSrcAlpha // Premultiplied transparency

			    CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag2
	            #pragma target 2.5
				#pragma multi_compile_instancing
				#pragma multi_compile _ PIXELSNAP_ON
				#pragma multi_compile _ ETC1_EXTERNAL_ALPHA


				float _HueShift;
				float _Sat;
				float _Val;
				float _Outline;
				fixed4 _OutlineColor;
				int _OutlineSize;
				float4 _MainTex_TexelSize;
	 
	 
	            float3 shift_col(float3 RGB, float3 shift)
	            {
	            float3 RESULT = float3(RGB);
	            float VSU = shift.z*shift.y*cos(shift.x*3.14159265/180);
	                float VSW = shift.z*shift.y*sin(shift.x*3.14159265/180);
	               
	                RESULT.x = (.299*shift.z+.701*VSU+.168*VSW)*RGB.x
	                        + (.587*shift.z-.587*VSU+.330*VSW)*RGB.y
	                        + (.114*shift.z-.114*VSU-.497*VSW)*RGB.z;
	               
	                RESULT.y = (.299*shift.z-.299*VSU-.328*VSW)*RGB.x
	                        + (.587*shift.z+.413*VSU+.035*VSW)*RGB.y
	                        + (.114*shift.z-.114*VSU+.292*VSW)*RGB.z;
	               
	                RESULT.z = (.299*shift.z-.3*VSU+1.25*VSW)*RGB.x
	                        + (.587*shift.z-.588*VSU-1.05*VSW)*RGB.y
	                        + (.114*shift.z+.886*VSU-.203*VSW)*RGB.z;
	               
	            return (RESULT);
	            }

				

				fixed4 frag2(v2f_img i) : SV_Target {
				    //return fixed4(1.0,0.0,0.0,1.0);
				    fixed4 c = tex2D(_MainTex, i.uv) * _Color;
				    if(c.a < _Cutoff){
				    	c = fixed4 (0,0,0,0);
				    }

                	float3 shift = float3(_HueShift, _Sat, _Val);
					c = half4(half3(shift_col(c, shift)), c.a);

					if (_Outline > 0 && c.a != 0) {
						float totalAlpha = 1.0;

						[unroll(16)]
						for (int j = 1; j < _OutlineSize + 1; j++) {
							fixed4 pixelUp = tex2Dlod(_MainTex, float4(i.uv + fixed2(0, j * _MainTex_TexelSize.y),0,0));
							//fixed4 pixelDown = tex2Dlod(_MainTex, float4(i.uv - fixed2(0, j *  _MainTex_TexelSize.y), 0, 0));
							fixed4 pixelRight = tex2Dlod(_MainTex, float4(i.uv + fixed2(j * _MainTex_TexelSize.x, 0), 0, 0));
							fixed4 pixelLeft = tex2Dlod(_MainTex, float4(i.uv - fixed2(j * _MainTex_TexelSize.x, 0), 0, 0));

							//totalAlpha = totalAlpha * pixelUp.a * pixelDown.a * pixelRight.a * pixelLeft.a;
							totalAlpha = totalAlpha * pixelUp.a * pixelRight.a * pixelLeft.a;
						}

						if (totalAlpha == 0) {
							c.rgba = fixed4(1, 1, 1, 1) * _OutlineColor;
						}
					}
				    return c;
				}
			    ENDCG
            }
        }
		Fallback "Transparent/Cutout/VertexLit"
    }
