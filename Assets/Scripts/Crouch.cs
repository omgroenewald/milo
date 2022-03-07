using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Crouch : MonoBehaviour
{
    public GameObject Player;
    public bool crouch = false;
    public Vector3 SizeChange;
    bool Allcrouch = true;
    public UnityEvent SpeedChange;
    public UnityEvent SpeedUnChange;


    private bool _crouchPressed = false;
    private bool _crouchUnPressed = false;
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _crouchPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _crouchUnPressed = true;
        }
    }
    public void FixedUpdate()
    {
        if (_crouchPressed)
        {
            Player.transform.localScale = Player.transform.localScale - SizeChange;
            _crouchPressed = false;
            Allcrouch = false;
            SpeedChange.Invoke();
        }
        if (_crouchUnPressed)
        {
            Player.transform.localScale = Player.transform.localScale + SizeChange;
            Player.transform.localPosition.Set(Player.transform.localPosition.x, - 6.11f, Player.transform.localPosition.z);
            _crouchUnPressed = false;
            Allcrouch = true;
            SpeedUnChange.Invoke();
        }
    }
}
