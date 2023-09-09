using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orthoCamSettings : MonoBehaviour
{

    // frustrum's height in scene units
    public float frustrumHeight;

    public Camera orthoCam;
    public float screenScaleX, screenScaleY;
    public float screenPosX, screenPosY;

    // Start is called before the first frame update
    void Start()
    {



        // enable cam
        orthoCam.enabled = true;

        if (orthoCam)
        {
            // set camera projection mode to orthographic
            orthoCam.orthographic = true;

            // frustrum´s height = orthographicSize * 2.0
            // frusrtum´s width = aspect ratio * height;

            orthoCam.orthographicSize = frustrumHeight / 2;


            // place camera render in screen space 
            // screen space = [0,1] , bottom left -> top right

            // screenPos.x [0, 1] - offset to camera's renderTexture position_x in screen space
            // screenScale.x [0, 1] - offset to camera's renderTexture scale_x in screen space
            orthoCam.rect = new Rect(screenPosX, screenPosY, screenScaleX, screenScaleY);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    { 
        
    }
}
