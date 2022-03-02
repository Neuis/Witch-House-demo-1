using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PressableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eData)
    {
        if (GetComponent<Button>().interactable)
        {
            gameObject.transform.localScale = Constants.BUTTON_PRESSED_SCALE;
        }        
    }

    public void OnPointerUp(PointerEventData eData)
    {
        if (GetComponent<Button>().interactable)
        {
            gameObject.transform.localScale = Constants.BUTTON_NOT_PRESSED_SCALE;
        }            
    }
}
