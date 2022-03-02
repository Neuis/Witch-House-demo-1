using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CallBackButton : MonoBehaviour
{
    private Action cbFunc;

    public virtual void OnThisButtonClick()
    {
        cbFunc?.Invoke();
    }

    public void SetCallback(Action val)
    {
        cbFunc = val;
    }
}
