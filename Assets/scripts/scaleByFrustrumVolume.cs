using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleByFrustrumVolume : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // frustrum's height (in scene units)
        // get orthographicSize = 0.5 * frustrum's height        
        float height = (float)Camera.main.orthographicSize * 2.0f;

        // frustrum's width = height * aspect ratio
        float width = height * Screen.width / Screen.height;

        // scale object according to frustrum width and height
        transform.localScale = new Vector3(width / 10, 1.0f, height / 10);

    }
}
