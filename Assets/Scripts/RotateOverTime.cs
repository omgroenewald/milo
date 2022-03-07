using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    // Use this for initialization
    public float speed;
    public float RotAngleY;
    public float AngleYStart;
    public float AngleXPotion;
    public float AngleZpotion;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float rY = Mathf.SmoothStep(AngleYStart, RotAngleY, Mathf.PingPong(Time.time * speed, 1));
        transform.rotation = Quaternion.Euler(AngleXPotion, rY, AngleZpotion);
    }
}
