using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackInfoWindow : MonoBehaviour
{
    [SerializeField]
    private CountableView oneItemCountableView;
    [SerializeField]
    private GameObject oneItemContainer;
    [SerializeField]
    private GameObject twoItemsContainer;
    [SerializeField]
    private CountableView leftItemCountableView;
    [SerializeField]
    private CountableView rightItemCountableView;

    public void SetLeftItemInfo(TileInfo tileInfo)
    {
        leftItemCountableView.itemImage.sprite = GameUtility.GetImage(tileInfo);
        leftItemCountableView.itemCountText.text = ModifiersManager.GetBonus_RequiredTilesNumber(tileInfo).ToString(); //tileInfo.required_stack_amount.ToString();
        leftItemCountableView.UpdateAmountBackground();
        rightItemCountableView.itemCountText.text = "1";
        rightItemCountableView.UpdateAmountBackground();
    }
    
    public void SetRightItemInfo(TileInfo tileInfo)
    {
        rightItemCountableView.itemImage.sprite = oneItemCountableView.itemImage.sprite = GameUtility.GetImage(tileInfo);
    }

    public void SetId(int tileId)
    {

    }

    public void UpdateAmount(int val)
    {
        oneItemCountableView.itemCountText.text = val.ToString();
        oneItemCountableView.UpdateAmountBackground();
    }

    public void HideWindow()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowWindow()
    {
        this.gameObject.SetActive(true);
    }

    public void ChangeShowModeToDouble()
    {
        oneItemContainer.SetActive(false);
        twoItemsContainer.SetActive(true);
    }

    public void ChangeShowModeToSingle()
    {
        oneItemContainer.SetActive(true);
        twoItemsContainer.SetActive(false);
    }

    public bool IsInSingleMode()
    {
        return oneItemContainer.activeSelf;
    }


        
}
