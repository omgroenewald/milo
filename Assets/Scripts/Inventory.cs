using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;


public class Inventory : MonoBehaviour
{

    [DllImport("user32.dll")] static extern uint GetActiveWindow();
    [DllImport("user32.dll")] static extern bool SetForegroundWindow(IntPtr hWnd);
    public UnityEvent Bag;
    public UnityEvent nonBag;
    public UnityEvent Key;
    public UnityEvent nonKey;
    public bool HasKey = false;
    bool tabT = false;
    private IntPtr _hWnd_Unity;

    public Inventory()
    {
        _hWnd_Unity = (IntPtr)GetActiveWindow();
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            SetForegroundWindow(_hWnd_Unity);
        }
            
    }
    public void FixedUpdate()
    {
        tabT = Input.GetKey(KeyCode.Tab);
        if (tabT)
        {
            Bag.Invoke();
        }
        if (!tabT)
        {
            nonBag.Invoke();
        }
        if (HasKey)
        {
            Key.Invoke();
        }
        else
        {
            nonKey.Invoke();
        }
    }
}
