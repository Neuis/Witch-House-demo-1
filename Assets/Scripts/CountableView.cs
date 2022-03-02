using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CountableView : MonoBehaviour
{
    [SerializeField]
    public Image itemImage;
    [SerializeField]
    public TextMeshProUGUI itemCountText;
    [SerializeField]
    private RectTransform itemCountBackgroundImage;

    private void Awake()
    {
        GetComponent<RectTransform>().localScale = Vector3.one;
    }
    public virtual void UpdateAmountBackground()
    {
        itemCountBackgroundImage.sizeDelta = new Vector2(itemCountText.preferredWidth + Constants.ITEM_COUNT_BG_PADDING, itemCountBackgroundImage.rect.height);
    }

    public void ShowAsDisabled()
    {
        itemImage.color = Constants.COLOR_WHITE_TRANSPARENT;
    }

    public void ShowAsEnabled()
    {
        itemImage.color = Constants.COLOR_WHITE;
    }
}
