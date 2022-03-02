using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ButtonWithIcon : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private TextMeshProUGUI textField;
    [SerializeField]
    private Button thisBtn;
    [SerializeField]
    private bool widthIsResizeable = false;
    [SerializeField]
    private int spacingBetweenItems = 5;
    [SerializeField]
    private int leftRightOffset;// = 10;

    private int _price;
    private int _currencyId;
    private Action _cbFunc;

    public void OnThisButtonClick()
    {
        _cbFunc?.Invoke();
    }

    public void SetCallback(Action cbFunc)
    {
        _cbFunc = cbFunc;
    }

    public Action GetCallback()
    {
        return _cbFunc;
    }

    public void SetButtonInfo(int price, int currencyId, bool addPlusChar=false)
    {
        _price = price;
        _currencyId = currencyId;
        textField.text = addPlusChar? "+"+_price.ToString() : _price.ToString();
        RectTransform tfRectTransform = textField.rectTransform;
        tfRectTransform.sizeDelta = new Vector2(textField.preferredWidth, tfRectTransform.rect.height);

        iconImage.sprite = GameUtility.GetCurrencyImage(_currencyId);
        if (addPlusChar)
        {
            this.GetComponent<Image>().color = Constants.BUTTON_COLOR_DEFAULT;
        }
        else
        {
            switch (_currencyId)
            {
                case Constants.CURRENCY_SOULS_ID:
                    this.GetComponent<Image>().color = Constants.BUTTON_COLOR_SOUL_BOTTLE;
                    break;

                case Constants.CURRENCY_SOUL_STONES_ID:
                    this.GetComponent<Image>().color = Constants.BUTTON_COLOR_SOUL_CRYSTAL;
                    break;

                default:
                    this.GetComponent<Image>().color = Constants.BUTTON_COLOR_DEFAULT;
                    break;
            }
        }

        if (widthIsResizeable)
        {
            RectTransform rt = (RectTransform)this.transform;
            float newWidth = tfRectTransform.rect.width + ((RectTransform)iconImage.transform).rect.width + spacingBetweenItems + leftRightOffset;
            rt.sizeDelta = new Vector2(newWidth, rt.rect.height);
        }
    }

    public void SetEnabled(bool isEnabled)
    {
        thisBtn.interactable = isEnabled;
    }

}
