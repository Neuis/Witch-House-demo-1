using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Image bgImg;
    [SerializeField]
    private Image darkCover;
    [SerializeField]
    private Text rowText;
    [SerializeField]
    private Text columnText;
    [SerializeField]
    private Text uidText;

    private TileInfo info;

    private bool isDisabled = false;
    private int _columnNumber = -1;
    private int _rowNumber = -1;
    private bool _isMoving = false;
    private int _testUID = -1;

    public void SetInfo(TileInfo newInfo)
    {
        info = newInfo;
        bgImg.sprite = GameUtility.GetImage(newInfo);
        MakeEnabled();
    }

    public void MakeDisabled()
    {
        isDisabled = true;
        darkCover.enabled = true;
    }

    public bool IsAvailable()
    {
        return !isDisabled;
    }

    public void MakeEnabled()
    {
        isDisabled = false;
        darkCover.enabled = false;
    }

    public TileInfo GetInfo()
    {
        return info;
    }

    public void SetColumn(int columnNumber)
    {
        //i need _columnNumber because for me it is easier to find what position should be set to NEW tile after THIS tile was deleted
        _columnNumber = columnNumber;
        columnText.text = _columnNumber.ToString();
    }

    public void SetRow(int rowNumber)
    {
        _rowNumber = rowNumber;
        rowText.text = _rowNumber.ToString();
    }

    public int GetColumnNumber()
    {
        return _columnNumber;
    }

    public int GetRowNumber()
    {
        return _columnNumber;
    }

    public void MakeItMove()
    {
        _isMoving = true;
    }

    public bool IsMoving()
    {
        return _isMoving;
    }

    public void MakeItStop()
    {
        _isMoving=false;
    }

    public void SetTestUID(int uid)
    {
        _testUID = uid;
        uidText.text = _testUID.ToString();
    }

    public int GetTestUID()
    {
        return _testUID;
    }
}
