using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WarehouseItem : CountableView
{
    private WhItemInfo _info;

    public void SetInfo (WhItemInfo info)
    {
        _info = info;
        itemImage.sprite = GameUtility.GetImage(info);
        UpdateItemCount();
    }

    public void UpdateItemCount()
    {
        int amount = User.instance.GetWhItemById(_info.id).GetAmount();
        itemCountText.text = amount.ToString();
        UpdateAmountBackground();
    } 

    public void SetId (int id)
    {
        WhItemInfo tempInfo = JSONReader.instance.GetWhItemInfoById(id);
        SetInfo(tempInfo);
    }

    public int GetId()
    {
        return _info.id;
    }
}
