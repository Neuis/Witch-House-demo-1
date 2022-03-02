using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class User : MonoBehaviour
{
    private const string userDataFileName = "/wh_save2.dat";

    public static User instance = null;

    private GameData userData = null;

    public delegate void CurrencyChanger();
    public event CurrencyChanger OnCurrencyChanged;

    private List<TileInfo> chosenForestTiles;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            LoadUserData();
            ProceedAfterUserDataLoaded();
        }
        else
        {
            Debug.LogError("You are trying to create instance of USER but you already have one");
        }
        DontDestroyOnLoad(gameObject);
    }

    private void ProceedAfterUserDataLoaded()
    {
        SetJourneyLocationId(userData.CurrentLocationId);
        chosenForestTiles = new List<TileInfo>();
        foreach (int index in userData.ChosenForestTiles)
        {
            chosenForestTiles.Add(JSONReader.instance.GetTileInfoById(index));
        }
    }

    private void SaveUserData()
    {
        string destination = Application.persistentDataPath + userDataFileName;
        FileStream file;
        if (File.Exists(destination))
        {
            file = File.OpenWrite(destination);
        }
        else
        {
            file = File.Create(destination);
        }

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, userData);
        file.Close();
    }

    private void LoadUserData()
    {
        string destination = Application.persistentDataPath + userDataFileName;
        FileStream file;
        if (File.Exists(destination))
        {
            file = File.OpenRead(destination);
        } else
        {
            Debug.Log("I can't find user data file with this destination: "+destination);
            Debug.Log("This is probably new User. We should create new file for him");
            CreateNewUserData();
            return;
        }
        BinaryFormatter bf = new BinaryFormatter();
        try
        {
            userData = (GameData)bf.Deserialize(file);         
        }
        catch
        {
            Debug.LogError("I CAN'T LOAD USER DATA. I CREATED NEW EMPTY USER");
            file.Close();
            CreateNewUserData();
            return;
        }
        file.Close();
    }

    private int GetCurrencyTileMoneyAmount()
    {
        return Constants.DEFAULT_CURRENCY_TILE_MONEY_AMOUNT; 
    }

    public void AddGatheredTilesByTileInfo(TileInfo tInfo, int amount)
    {
        if (tInfo.id == Constants.TILE_ID_CURRENCY)
        {
            AddMoney(GetCurrencyTileMoneyAmount());
        } else
        {
            WhItemData resource = GetWhItemByTileType(tInfo.tile_type);
            int rawResourceCount = amount + resource.GetCurrentRawResourceAmount();
            int tilesRequiredAmount = GetTileRequiredAmountToFormWhItem(tInfo);
            int addedItemCount = Mathf.FloorToInt(rawResourceCount / tilesRequiredAmount);
            int newRawResourceAmount = rawResourceCount % tilesRequiredAmount;
            resource.SetRawResourceAmount(newRawResourceAmount);
            resource.AddAmount(addedItemCount);
        }        
    }

    private void CreateNewUserData()
    {
        string destination = Application.persistentDataPath + userDataFileName;
        FileStream file = File.Create(destination);
        file.Close();
        List<WhItemData> resources = new List<WhItemData>();
        WhItemInfo[] allResources = JSONReader.instance.GetWhItemInfos();
        WhItemData resData;
        for (int i = 0; i < allResources.Length; i++)
        {
            resData = new WhItemData();
            resData.SetInfo(allResources[i]);
            resources.Add(resData);
        }

        userData = new GameData(resources, 400, CreateStartingForestTiles());
        SaveUserData();
    }

    private List<int> CreateStartingForestTiles()
    {
        List<int> result = new List<int> { 
            Constants.STARTING_FLOWER_TILE_ID,
            Constants.STARTING_TREE_TILE_ID,
            Constants.SECOND_STARTING_MUSHROOM_TILE_ID,
            Constants.STARTING_BERRY_TILE_ID,
            Constants.STARTING_SPIDER_TILE_ID,
            Constants.STARTING_VEGETABLE_TILE_ID
        };
        return result;
    }

    public int GetMoney()
    {
        return userData.Money;
    }

    public void AddWhItem(int whItemId, int amount = 1)
    {
        GetWhItemById(whItemId).AddAmount(amount);
    }

    private void AddMoney(int val)
    {
        userData.Money += val;
        OnCurrencyChanged?.Invoke();
    }

    public List<WhItemData> GetWhItems()
    {
        return userData.WhItems;
    }

    public List<WhItemData> GetJourneyWhResources()
    {
        LocationInfo lInfo = GetCurrentJourneyLocation();
        List<WhItemData> result = new List<WhItemData>();
        List<WhItemData> resources = GetWhItems();
        foreach (WhItemData whiData in resources)
        {
            if (whiData.GetInfo()!=null &&
                GameUtility.CheckIfLocationContainsTileType(lInfo, whiData.GetInfo().required_tile_type))
            {
                result.Add(whiData);
            }
        }
        return result;
    }

    public WhItemData GetWhItemByTileType(string tileType)
    {
        WhItemData result = null;
        List<WhItemData> resources = userData.WhItems;
        foreach (WhItemData res in resources)
        {
            if (res.GetRequiredTileType()==tileType)
            {
                result = res;
                break;
            }
        }
        return result;
    }

    public WhItemData GetWhItemById(int id)
    {
        WhItemData result = null;
        List<WhItemData> resources = userData.WhItems;
        foreach (WhItemData res in resources)
        {
            if (res.GetId() == id) 
            {
                result = res;
                break;
            }
        }
        return result;
    }


    public int GetTileRequiredAmountToFormWhItem(TileInfo tInfo)
    {
        return ModifiersManager.GetWhItem_RequiredTilesNumber(tInfo);
    }

    public List<TileInfo> GetFightTiles()
    {
        return chosenForestTiles;
    }

    public TileInfo GetTileInfoByTileType(string tileType)
    {
        TileInfo result = null;
        List<TileInfo> tiles = GetFightTiles();
        foreach (TileInfo info in tiles)
        {
            if (info.tile_type == tileType)
            {
                result = info;
                break;
            }
        }
        return result;
    }

    public List<LocationInfo> GetAvailableLocationInfos()
    {
        List<LocationInfo> result = new List<LocationInfo>();
        LocationInfo[] locs = JSONReader.instance.GetLocationInfos();
        result.AddRange(locs);
        return result;
    }

    public void SetActualTurnsMaxCount(int val)
    {
        userData.TurnsMaxCount = val;
    }

    public int GetActualTurnsMaxCount()
    {
        return userData.TurnsMaxCount;
    }

    public void SetJourneyLocationId(int val)
    {
        userData.CurrentLocationId = val;
    }

    public int GetJourneyLocationId()
    {
        return userData.CurrentLocationId;
    }

    private LocationInfo GetCurrentJourneyLocation()
    {
        return JSONReader.instance.GetLocationInfoById(GetJourneyLocationId());
    }
    public int GetPossibleTurnsMaxCount(int locationId)
    {
        return Constants.FOREST_DEFAULT_TURNS;
    }

    public void StartJourney(int journeyLocationId, int turns)
    {   
        SetActualTurnsMaxCount(turns);
        SetJourneyLocationId(journeyLocationId);
        SetTurnsMade(0);
        SaveUserData();
        ScreenManager.instance.OpenFightScreen();
    }

    public void SetTurnsMade(int val)
    {
        userData.TurnsMade = val;
    }

    public int GetTurnsMade()
    {
        return userData.TurnsMade;
    }

    public List<TileInfo> GetSavedTiles()
    {
        return userData.ForestTiles;
    }

    public void SetJourneyTiles(List<TileInfo> tilesList)
    {
        userData.ForestTiles = tilesList;
    }

    public void JourneyEnded()
    {
        SetJourneyLocationId(0);
        SetTurnsMade(0);
    }

    public void SetAndSaveNewJourneyInfo(List<TileInfo> tilesToSave, int turnsToSave)
    {
        SetTurnsMade(turnsToSave);
        SetJourneyTiles(tilesToSave);
        SaveUserData();
    }

    public void OnPlayBtnClick()
    {
        if (GetJourneyLocationId() == 0)
        {
            StartJourney(Constants.LOCATION_ID_FOREST, GetPossibleTurnsMaxCount(Constants.LOCATION_ID_FOREST));
        }
        ScreenManager.instance.OpenFightScreen();
    }
}
