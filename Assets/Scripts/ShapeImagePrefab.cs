using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShapeImagePrefab : MonoBehaviour
{
    [SerializeField]
    private Image mainImg;
    [SerializeField]
    private RectTransform percentageMask;
    [SerializeField]
    private TextMeshProUGUI percentageText;
    [SerializeField]
    private GameObject shapesContainer;

    private int maxMaskHeight;

    private void Awake()
    {
        maxMaskHeight = (int)((RectTransform)mainImg.transform).rect.height;
    }

    public void SetVisiblePercent(float numberBetweenOneAndOneHundred)
    {
        if (numberBetweenOneAndOneHundred >= 100)
        {
            shapesContainer.SetActive(false);
            percentageText.text = "";
        } else
        {
            shapesContainer.SetActive(true);
            percentageText.text = numberBetweenOneAndOneHundred + "%";
            float newHeightDelta = (maxMaskHeight * numberBetweenOneAndOneHundred) / 100;
            percentageMask.offsetMin = new Vector2(0, newHeightDelta);
        }
    }

    public void SetImagePath(string imgPath)
    {
        mainImg.sprite = Resources.Load<Sprite>(imgPath);
    }


    public virtual void ShowFullGrey()
    {
        SetVisiblePercent(0);
    }

    public void ShowFullImage()
    {
        SetVisiblePercent(100);
    }
}
