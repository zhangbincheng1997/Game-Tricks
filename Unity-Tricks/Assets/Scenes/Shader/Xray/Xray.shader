Shader "Xray" {
    Properties {
        _NotVisibleColor("NotVisibleColor (RGB)", Color) = (0.3,0.3,0.3,1)
        _MainTex("Base (RGB)", 2D) = "white" {}
    }
    
    SubShader {
        Tags { "Queue" = "Geometry+500" "RenderType" = "Opaque" }
        LOD 200

        // 透视绘制
        Pass {
            ZWrite Off
            ZTest Greater
            Color[_NotVisibleColor]
        }

        // 正常绘制
        Pass {
            ZWrite On
            ZTest LEqual
            Material {
                Diffuse(1,1,1,1)
                Ambient(1,1,1,1)
            }
            SetTexture[_MainTex] { combine texture }
        }
    }

    FallBack "Diffuse"
}