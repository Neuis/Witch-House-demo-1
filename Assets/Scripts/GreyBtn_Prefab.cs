using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GreyBtn_Prefab : ShapeImagePrefab
{
    [SerializeField]
    private Button thisBtn;
    [SerializeField]
    private GameObject checkBoxBtn;
    [SerializeField]
    private GameObject premiumMoneyImage;
    [SerializeField]
    private GameObject magnifierImage;
    [SerializeField]
    private GameObject questionMarkImage;

    private Action cbFunc;

    public void OnThisButtonClick()
    {
        cbFunc?.Invoke();
    }

    public void SetCallback(Action val)
    {
        cbFunc = val;
    }

    public void ShowThisItemAsActive()
    {
        HideAllAdditions();
        checkBoxBtn.SetActive(true);
    }

    public void ShowPremiumMoney()
    {
        HideAllAdditions();
        premiumMoneyImage.SetActive(true);
    }

    public void HideAllAdditions()
    {
        checkBoxBtn.SetActive(false);
        premiumMoneyImage.SetActive(false);
    }

    public override void ShowFullGrey()
    {
        base.ShowFullGrey();
        MagnifierShouldBeVisible(false);
        QuestionMarkShouldBeVisible(false);
    }

    public void MagnifierShouldBeVisible(bool val)
    {
        magnifierImage.SetActive(val);
    }

    public void QuestionMarkShouldBeVisible(bool val)
    {
        questionMarkImage.SetActive(val);
    }
}
