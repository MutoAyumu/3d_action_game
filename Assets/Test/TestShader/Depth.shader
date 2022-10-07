Shader "Unlit/Depth"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        pass
        {
            Tags
            {
                "Queue" = "Geometry + 1"
            }

            ZWrite On
            ColorMask 0
        }
    }
}
