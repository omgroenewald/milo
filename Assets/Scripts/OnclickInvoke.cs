using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnclickInvoke : MonoBehaviour
{
   public UnityEvent OnClick;
    bool onclick;

    public void Update()
    {
        onclick = Input.GetMouseButtonDown(0);
        if (onclick)
        {
            OnClick.Invoke();
        }
    }
}
