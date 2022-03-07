using UnityEngine;

public class look : MonoBehaviour
{

    public static bool cursorlocked = true;
    public bool Freez = false;

    public Transform player;
    public Transform cams;
    public Transform weapon;

    public float xsensitivity;
    public float ysensitivity;
    public float maxangle;

    private Quaternion camcenter;
    void Start()
    {
        camcenter = cams.localRotation;
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
            sety();
            setx();
        }
            lockcursor();
        
    }

    void sety()
    {

        float t_input = Input.GetAxis("Mouse Y") * ysensitivity * Time.deltaTime;
        Quaternion t_adj = Quaternion.AngleAxis(t_input, -Vector3.right);
        Quaternion t_delta = cams.localRotation * t_adj;

        if (Quaternion.Angle(camcenter, t_delta) < maxangle)
        {
            cams.localRotation = t_delta;
            
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

        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                cursorlocked = true;
            }

        }

    }

}
