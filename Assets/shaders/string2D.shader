Shader "Unlit/string2D"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AnimationID("AnimationID", Float) = 1.0
        _Scrubbing("Scrubbing", Float) = 1.0
        _Amp("Amp", Float) = 1.0
        _AspectRatioWidth("aspectRatioWidth", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            #define PI 3.14159265359
            #define E 2.71828

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _AnimationID;
            float _Scrubbing;
            float _Amp;
            float _AspectRatioWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

              
            float drawLine2D (float width, float shapefunc, float nfc_y){
              return  step(shapefunc-width*0.5, nfc_y) -
                      step(shapefunc+width*0.5, nfc_y);
    
                // for width = 0.1, shape = 0.5 , nfc_y = 0.5
                // return step(0.45, 0.5) - step(0.55, 0.5) =
                // = 1 - 0 = 1
    
                // for width = 0.1, shape = 0.5 , nfc_y = 0.3
                // return step(0.45, 0.3) - step(0.55, 0.3) =
                // = 0 - 0 = 0
            }

            float drawCircleEdge2D(float2 size, float2 edgeBlur, float radius){  
            edgeBlur = max(edgeBlur * 0.5f, 0.0f);
    
            float mask = smoothstep (size.x, size.x + edgeBlur.x, radius);
            mask *= smoothstep(size.y , size.y - edgeBlur.y, radius);
            return mask;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                float2 nfc = i.uv;
                nfc.y = 1.0 - nfc.y;

                fixed4 col;
              
                // --- draw vibrating string line ----
                float a = _Amp;// amplitude
                //float t =  lerp(0.5, 5.0, smoothstep(-1.0,1.0,(cos(_Time.y*2.0)))); //time

                float animID = _AnimationID;
                float anim_pluck = 0.5f;
                float anim_release = animID * lerp(0.5,20.0, _Scrubbing);
                float t =  anim_release + anim_pluck; // timeline scrubbing using "_Scrubbing[0,1]";

                float k = 1.0;  // oscillation frequency (vibration speed)
                float w = PI;   // radial frequency
                float x = nfc.x; // longtitude position [0, 1]
                float n = 2.0;   // amount of harmonics
                float d = 5.;   // dampening delta
                float l = 0.125;  // limit 2nd harmonic wave amplitude
                float offsety = 0.5;

                // f(x) = a * Σ(sin(k*i*PI*t) * sin(w*i*x)) * pow(e, -t/d)
                float damping = pow(E, -t/d);
                float shape =  sin(k*1.0*PI*t) * sin(w*1.0*x) + l * sin(k*2.0*PI*t) * sin(w*2.0*x);
                float shapefunc = a * shape * damping + offsety;

                float width = 0.01;
                float mask_string = drawLine2D(width, shapefunc, nfc.y);

                // --- draw touch indicator ---
                float2 pos = float2(0.5f, shapefunc);

                // scale (poisiton as pivot)
                    // move from position to (0,0)
                nfc = nfc - pos;
                    // scale
                nfc.x = nfc.x * _AspectRatioWidth;
                     // move nack to position
                nfc = nfc + pos;

                float2 tc = nfc - pos;
                float r = length(tc);
                float outerSize = 0.06f;
                float innerSize = 0.05f;
                float2 size = float2(innerSize, outerSize);
               
                float2 edgeBlur = 0.001f;
                float mask_touch = drawCircleEdge2D(size, edgeBlur, r);
                float mask = mask_string + mask_touch;
                col = fixed4(mask,mask,mask,1.0);
                return col;
            }
            ENDCG
        }
    }
}
