using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class Cams : MonoBehaviour
{
    public static bool cursorlocked = true;
    public bool Freez = false;

    public Transform player;
    public Transform camaras;
    public Transform ROb;
    public Transform weapon;
    public UnityEvent HAClick;

    public UnityEvent<RaycastHit,bool> RCHit;
    
    bool Click = false;
    public UnityEvent non;

    public float xsensitivity;
    public float ysensitivity;
    public float maxangle;
    private bool isHit = false;
    private Quaternion camcenter;
    

    void Start()
    {
        camcenter = camaras.localRotation;
    }
    public void FreezCams()
    {
        Freez = true;
    }
    public void ResumeCams()
    {

        Freez = false;
    }
    void Update()
    {
        if (!Freez)
        {
            
            Click = Input.GetMouseButton(0);
            
            sety();
            setx();
            RaycastHit rayCastHit;
            Debug.DrawRay(ROb.transform.position, ROb.TransformDirection(Vector3.forward), Color.green);
            var hover = Physics.Raycast(ROb.transform.position, ROb.TransformDirection(Vector3.forward), out rayCastHit, 2f, LayerMask.GetMask("UI"));
            if (rayCastHit.collider!= null)
            {
                isHit = true;
                print(rayCastHit.collider.tag);
                RCHit?.Invoke(rayCastHit,Click);    
            }
            if (!hover&&isHit)
            {
                non.Invoke();
            }
        }
        lockcursor();
    }

    void sety()
    {

        float t_input = Input.GetAxis("Mouse Y") * ysensitivity * Time.deltaTime;
        Quaternion t_adj = Quaternion.AngleAxis(t_input, -Vector3.right);
        Quaternion t_delta = camaras.localRotation * t_adj;

        if (Quaternion.Angle(camcenter, t_delta) < maxangle)
        {
            camaras.localRotation = t_delta;
           
        }


    }
    void setx()
    {

        float t_input = Input.GetAxis("Mouse X") * xsensitivity * Time.deltaTime;
        Quaternion t_adj = Quaternion.AngleAxis(t_input, Vector3.up);
        Quaternion t_delta = player.localRotation * t_adj;
        player.localRotation = t_delta;
    }

    void lockcursor()
    {
        if (cursorlocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                cursorlocked = false;
            }
            if (Input.GetKey(KeyCode.Tab))
            {
                cursorlocked = false;
            }

        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (!Input.GetKey(KeyCode.Tab))
            {
                cursorlocked = true;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                cursorlocked = true;
            }

        }

    }
}
