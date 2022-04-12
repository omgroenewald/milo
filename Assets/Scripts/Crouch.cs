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
    private Vector3 _originalScale;
    private Vector3 _crouchedScale;

    public void Start()
    {
        _originalScale = Player.transform.localScale;
        _crouchedScale = Player.transform.localScale - SizeChange;
    }
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
        if (crouch && !Input.GetKey(KeyCode.LeftControl))
            _crouchUnPressed = true;
        
    }
    
    public void FixedUpdate()
    {
        if (_crouchPressed)
        {
            crouch = true;
            Player.transform.localScale = _crouchedScale;
            _crouchPressed = false;
            Allcrouch = false;
            SpeedChange.Invoke();
        }
        if (_crouchUnPressed)
        {
            crouch = false;
            Player.transform.localScale = _originalScale;
            Player.transform.localPosition.Set(Player.transform.localPosition.x, - 6.11f, Player.transform.localPosition.z);
            _crouchUnPressed = false;
            Allcrouch = true;
            SpeedUnChange.Invoke();
        }
    }
}
