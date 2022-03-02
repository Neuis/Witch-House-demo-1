using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GatheringProgress_ItemView : MonoBehaviour
{
    [SerializeField]
    private WarehouseItem whItemView;
    [SerializeField]
    private Slider progressBar;
    [SerializeField]
    private RectTransform unchangeableProgress;
    [SerializeField]
    private TextMeshProUGUI progressText;
    [SerializeField]
    private RectTransform progressbarFillArea;
    [SerializeField]
    private float maxprw;

    private float maxProgressWidth;
    private WhItemData _info;
    private int maxProgressValue;
    private int currentUnchangeableProgressValue;
    private int currentAddedProgressValue = 0;

    public void SetItemInfo(WhItemData info, int maxValue)
    {
        maxProgressWidth = progressbarFillArea.rect.width;
        _info = info;
        maxProgressValue = maxValue;
        currentUnchangeableProgressValue = _info.GetCurrentRawResourceAmount();
        whItemView.SetInfo(_info.GetInfo());
        progressBar.maxValue = maxProgressValue;
        float previousProgress = currentUnchangeableProgressValue * maxProgressWidth / maxProgressValue;
        if (previousProgress > maxProgressWidth)
        {
            previousProgress = maxProgressWidth;
        }
        unchangeableProgress.sizeDelta = new Vector2(previousProgress, 0);
    }

    private void UpdateProgress()
    {
        int currentValue = currentUnchangeableProgressValue + currentAddedProgressValue;
        progressText.text = currentValue + "/" + maxProgressValue;
        if (currentValue > maxProgressValue)
        {
            currentValue = maxProgressValue;
        }
        progressBar.value = currentValue;
    }

    public void SetAddedProgress(int val)
    {
        currentAddedProgressValue = val;
        UpdateProgress();
    }

    public void RecheckUserInfoAndUpdateWindow()
    {
        if (_info != null)
        {
            SetItemInfo(_info, maxProgressValue);
        }
    }
}
