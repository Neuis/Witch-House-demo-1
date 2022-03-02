using UnityEngine;
using System;
using UnityEngine.UI;

public class SimpleButton : PressableButton
{
    private Action cbFunc=null;

    public void OnThisButtonClick()
    {
        if (cbFunc != null)
        {
            cbFunc.Invoke();
        }        
    }

    public void SetCallback(Action val)
    {
        cbFunc = val;
    }

    public void SetEnabled(bool isEnabled)
    {
        GetComponent<Button>().interactable = isEnabled;
    }
}
