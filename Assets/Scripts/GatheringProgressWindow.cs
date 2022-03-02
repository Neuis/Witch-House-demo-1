using UnityEngine;


public class GatheringProgressWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject oneItemContainer;
    [SerializeField]
    private GameObject twoItemsContainer;

    [SerializeField]
    private GatheringProgress_ItemView oneItem_progressView;
    [SerializeField]
    private GatheringProgress_ItemView twoItems_topProgressView;
    [SerializeField]
    private GatheringProgress_ItemView twoItems_bottomProgressView;

    private bool isOneItem = true;

    public void SetOneItemInfo(WhItemData info, int maxValue)
    {
        twoItemsContainer.SetActive(false);
        isOneItem = true;
        oneItem_progressView.SetItemInfo(info, maxValue);
        oneItemContainer.SetActive(true);
    }

    public void SetTwoItemInfos(WhItemData topItemInfo, int topItemMaxValue, WhItemData botItemInfo, int botItemMaxValue)
    {
        oneItemContainer.SetActive(false);
        isOneItem = false;
        twoItems_topProgressView.SetItemInfo(topItemInfo, topItemMaxValue);
        twoItems_bottomProgressView.SetItemInfo(botItemInfo, botItemMaxValue);
        twoItemsContainer.SetActive(true);
    }

    public void RecheckUserInfoAndUpdateWindow()
    {
        if (isOneItem)
        {
            oneItem_progressView.RecheckUserInfoAndUpdateWindow();
        }
        else
        {
            twoItems_topProgressView.RecheckUserInfoAndUpdateWindow();
            twoItems_bottomProgressView.RecheckUserInfoAndUpdateWindow();
        }
    }

    public void SetOneItemAddedProgress(int val)
    {
        oneItem_progressView.SetAddedProgress(val);
    }

    public void SetTwoItemsAddedProgress(int topItemValue, int botItemValue)
    {
        twoItems_topProgressView.SetAddedProgress(topItemValue);
        twoItems_bottomProgressView.SetAddedProgress(botItemValue);
    }

}
