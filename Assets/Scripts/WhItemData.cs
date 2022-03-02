using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WhItemData
{
    private int _id;
    private int _currentRawResourceAmount = 0;
    private int _amount = 0;
    private int _reservedAmount = 0;
    public void SetInfo(WhItemInfo info)
    {
        _id = info.id;
    }

    public WhItemInfo GetInfo()
    {
        return JSONReader.instance.GetWhItemInfoById(_id);
    }

    public int GetId()
    {
        return _id;
    }

    public void SetAmount(int val)
    {
        _amount = val;
    }

    public void AddAmount(int val)
    {
        _amount += val;
    }

    public int GetAmount()
    {
        return _amount-_reservedAmount;
    }

    public void IncreaseReservedAmount()
    {
        _reservedAmount++;
    }

    public void ClearReserve()
    {
        _reservedAmount = 0;
    }

    public void ApplyReserve()
    {
        AddAmount(-_reservedAmount);
        _reservedAmount = 0;
    }

    public void SetRawResourceAmount(int val)
    {
        _currentRawResourceAmount = val;
    }
    
    public int GetCurrentRawResourceAmount()
    {
        return _currentRawResourceAmount;
    }

    public string GetRequiredTileType()
    {
        return GetInfo().required_tile_type;
    }
}
