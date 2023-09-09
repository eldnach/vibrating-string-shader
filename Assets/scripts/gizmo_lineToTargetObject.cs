using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gizmo_LineToTargetObject : MonoBehaviour
{
    public GameObject targetObject;
  
    void OnDrawGizmosSelected()
    {
        Vector3 target_pos = targetObject.transform.position;

        // Draws a blue line from this transform to the target
        Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, target_pos);

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

}
