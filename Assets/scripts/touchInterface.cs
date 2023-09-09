using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchInterface : MonoBehaviour
{
    // create a List of type:"touch" called "touchList"
    public List<touch> touchList = new List<touch>();
    touch touch_current;

    // Update is called once per frame
    void Update()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        for (int i = 0; i < Input.touchCount; i++)
        {

            Touch getTouch = Input.GetTouch(i);

            if(getTouch.phase == TouchPhase.Began)
            {
                // When touch begins....
                   // add new touch to touchList. (input getTouch ID and Position)
                touchList.Add(new touch(getTouch.fingerId, "began", getTouch.position));

                // if (touchList[x].touchID == GetTouch(i).fingerID) return... touchList[x] 
                touch_current = touchList.Find(touch => touch.touchID == getTouch.fingerId);
                Debug.Log("touchEvent: began touch. (added) TouchID=" + touch_current.touchID + " TouchPosition=" + touch_current.touchPosition + " TouchList_Index=" + touchList.IndexOf(touch_current));

            }
            else if (getTouch.phase == TouchPhase.Ended)
            {

                // if (touchList[x].touchID == GetTouch(i).fingerID) return... touchList[x] 
                touch_current = touchList.Find(touch => touch.touchID == getTouch.fingerId);
                touch_current.touchStatus = "ended";

                Debug.Log("touchEvent: ended touch. (removed) TouchID=" + touch_current.touchID + " TouchPosition=" + touch_current.touchPosition + " TouchList_Index=" + touchList.IndexOf(touch_current));


                // remove touchList[x] from touchList
                touchList.RemoveAt(touchList.IndexOf(touch_current));
            }
            else if (getTouch.phase == TouchPhase.Moved)
            {
                // if (touchList[x].touchID == GetTouch(i).fingerID) return... touchList[x] 
                touch_current = touchList.Find(touch => touch.touchID == getTouch.fingerId);
                touch_current.touchStatus = "moved";

                // update touchList[x].touchPosition = getTouch[i].position
                touch_current.touchPosition = getTouch.position;
                Debug.Log("touchEvent: moved touch. (modified) TouchID=" + touch_current.touchID + " TouchPosition=:" + touch_current.touchPosition + " TouchList_Index=" + touchList.IndexOf(touch_current));

            }
             
        }
        
    }



}


public class touch
{
    public int touchID;
    public string touchStatus;
    public Vector2 touchPosition;


    public touch(int in_touchID, string in_touchStatus, Vector2 in_touchPosition)
    {
        touchID = in_touchID;
        touchStatus = in_touchStatus;
        touchPosition = in_touchPosition;
    }
}


