using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveYourAss : MonoBehaviour
{
    public Camera normalcam;

    private Rigidbody rig;
    float t_hmove;
    float t_vmove;

    bool InvokePause = false;
    private void Start()
    {
        //Camera.main.enabled = false;
        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        t_hmove = Input.GetAxis("Horizontal");
        t_vmove = Input.GetAxis("Vertical");
    }
    public void Pause()
    {
        InvokePause = true;
    }
    public void Resume()
    {
        InvokePause = false;
    }
    public void FixedUpdate()
    {
        if (!InvokePause)
        {
            Vector3 t_direction = new Vector3(t_vmove, 0, -t_hmove);
            t_direction.Normalize();


            Vector3 t_targetvelocty = transform.TransformDirection(t_direction * Time.deltaTime);
            t_targetvelocty.y = rig.velocity.y;

            rig.velocity = t_targetvelocty;
        }
    }
}
