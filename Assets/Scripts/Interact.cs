using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{
    public UnityEvent Hover;
    public UnityEvent Click;
    bool hover = false;
    public GameObject RaycastOr;
    public LayerMask Inter;

    public void FixedUpdate()
    {

        Vector3 worldSpaceRotation = Vector3.forward;// - RaycastOr.transform.rotation.eulerAngles;

        Quaternion newChildRotation = Quaternion.Euler(RaycastOr.transform.TransformDirection(worldSpaceRotation));
        
        var vec = newChildRotation.eulerAngles;
        hover = Physics.Raycast(RaycastOr.transform.position, vec, 10f, Inter);
        Debug.DrawRay(RaycastOr.transform.position, vec, Color.green);
        if (hover)
        {
            print("yes");
        }
    }
}
