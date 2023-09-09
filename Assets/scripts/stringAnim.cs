using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stringAnim : MonoBehaviour
{
    float animationID;
    float scrubbing;
    float amplitude;
    Vector2 touchPos;
    Vector2 hitboxPos;
    float hitboxSize;
    public bool stringPluck;
    IEnumerator coroutine;

    public touchInterface touch_interface;

    Renderer rend;


    // Start is called before the first frame update
    void Start()
    {
        // instantiate "touchScript" class object. (fetch from this gameObject)
        touch_interface = GetComponent<touchInterface>();

        // get Renderer component
        rend = GetComponent<Renderer>();
        amplitude = 0.0f;
        hitboxSize = 0.35f;
        animationID = 0.0f;
        scrubbing = 0.0f;
        stringPluck = false;

        rend.material.SetFloat("_Amp", amplitude);
        rend.material.SetFloat("_AnimationID", animationID);
        rend.material.SetFloat("_Scrubbing", scrubbing);
        rend.material.SetFloat("_AspectRatioWidth", (float)Screen.width / (float)Screen.height);
    }

    // Update is called once per frame;
    void Update()
    {
    hitboxPos = new Vector2(0.0f, 0.0f);

    if (touch_interface.touchList.Count > 0)
    {
        Vector2 touchPos = new Vector2(touch_interface.touchList[0].touchPosition.x, touch_interface.touchList[0].touchPosition.y);
        touchPos = new Vector2(touchPos.x / Screen.width, touchPos.y / Screen.height);
        // remap to [-1, 1]
        touchPos = 2.0f * touchPos - new Vector2(1.0f, 1.0f);

        if (stringPluck == true)
        {

            amplitude = touchPos.y * 0.5f;
            scrubbing = 0.0f;
            animationID = 0.0f;
            rend.material.SetFloat("_Amp", amplitude);

        }

        Vector2 radius = touchPos - hitboxPos;
        // a = radius.x
        // b = radius.y
        // c = length(radius) = distance between touchPos and hitboxPos

        // c^2 = a^2 + b^2
        // c = sqrt(a^2 + b^2)
        float distance = Mathf.Sqrt(Mathf.Pow(radius.x, 2.0f) + Mathf.Pow(radius.y, 2.0f));

        if (distance <= hitboxSize)
        {
            stringPluck = true;
        }
    }


    if (touch_interface.touchList.Count == 0 && stringPluck == true)
    {
        stringPluck = false;
        //  coroutine = coroutine_stringAnim();
        StartCoroutine(coroutine_stringAnim());
     
    }

    rend.material.SetFloat("_AnimationID", animationID);
    rend.material.SetFloat("_Scrubbing", scrubbing);
    rend.material.SetFloat("_AspectRatioWidth", (float)Screen.width / (float)Screen.height);

    }

    IEnumerator coroutine_stringAnim()
    {
        // when string is released (stringPluck == false), play animation
        while(scrubbing < 1.0f)
        {
            animationID = 1.0f;

            // increase timeline scrubbing for pluck animation_part2
            scrubbing += Time.deltaTime * 0.3f;
            scrubbing = Mathf.Min(1.0f, scrubbing);

            yield return null;
        }
        // when scrubbing reaches 1.0, quit coroutine
    }
}



