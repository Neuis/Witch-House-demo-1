using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBoxButton : CallBackButton
{
    [SerializeField]
    private GameObject activeImage;

    [SerializeField] private bool canBeDeactivated = true;

    public override void OnThisButtonClick()
    {
        if (activeImage.activeSelf)
        {
            if (canBeDeactivated)
            {
                ShouldBeActive(false);
                base.OnThisButtonClick();
            }
        } else
        {
            ShouldBeActive(true);
            base.OnThisButtonClick();
        }        
    }

    public void MakeAsUnableForDeactivation()
    {
        canBeDeactivated = false;
    }

    public void ShouldBeActive(bool val)
    {
        activeImage.SetActive(val);
    }
}
