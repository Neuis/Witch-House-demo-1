using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameUtility : MonoBehaviour
{
    public static GameUtility instance = null;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        MakeInstance();
    }

    private void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("GAME UTILITY instance created successfully");
        }
        else
        {
            Debug.LogError("You are trying to create instance of GAME UTILITY but you already have one");
        }
    }

    public static void ClearEverythingInside(GameObject objToClear)
    {
        foreach (Transform child in objToClear.transform)
        {
            Destroy(child.gameObject);
        }
        objToClear.transform.DetachChildren();
    }

    public static void ClearEverythingInside(Transform objToClear)
    {
        foreach (Transform child in objToClear)
        {
            Destroy(child.gameObject);
        }
        objToClear.DetachChildren();
    }

    public static GameObject GetGameObjectByTag(string neededTag)
    {
        GameObject result = GameObject.FindGameObjectWithTag(neededTag);
        return result;
    }

    public static int CurrentTime()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int result = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        return result;
    }

    public static Sprite GetImageByPath(string imgPath)
    {
        return Resources.Load<Sprite>(imgPath);
    }

    public static Sprite GetImage(TileInfo tInfo)
    {
        return Resources.Load<Sprite>(Constants.TILES_PATH + tInfo.name + "_280x280");
    }

    public static Sprite GetImage(WhItemInfo whInfo)
    {
        return Resources.Load<Sprite>(Constants.ITEMS_PATH + whInfo.name + "_280");
    }

    public static Sprite GetCurrencyImage(int currencyId)
    {
        return Resources.Load<Sprite>(Constants.CURRENCY_PATH + currencyId);
    }

    public static int GetWhItemTilesNumberByTileInfo(TileInfo tInfo)
    {
        TileTypeInfo ttInfo = JSONReader.instance.GetTileTypeInfoByTileType(tInfo.tile_type);
        return ttInfo.number_to_create_wh_res;
    }

    public static int GetBonusTilesNumberByTileInfo(TileInfo tInfo)
    {
        TileTypeInfo ttInfo = JSONReader.instance.GetTileTypeInfoByTileType(tInfo.tile_type);
        if (ttInfo != null)
        {
            return ttInfo.number_to_create_bonus;
        } else
        {
            return 0;
        }
    }

    public static void MakeImageTransparent(Image targetImage)
    {
        Color tempColor = targetImage.color;
        tempColor.a = 0.4f;
        targetImage.color = tempColor;
    }

    public static void MakeImageNotTransparent(Image targetImage)
    {
        Color tempColor = targetImage.color;
        tempColor.a = 1f;
        targetImage.color = tempColor;
    }

    public static bool CheckIfLocationContainsTileType(LocationInfo lInfo, string tileType)
    {
        return GetLocationTileIndex(lInfo, tileType) >= 0;
    }
    public static int GetLocationTileIndex(LocationInfo lInfo, string tileType)
    {
        return Array.IndexOf(lInfo.available_tile_types, tileType);
    }

    public static LocationInfo GetLocationInfoByTileType(string tileType)
    {
        LocationInfo result = null;
        LocationInfo[] locations = JSONReader.instance.GetLocationInfos();
        foreach (LocationInfo lInfo in locations)
        {
            if (CheckIfLocationContainsTileType(lInfo, tileType))
            {
                result = lInfo;
                break;
            }
        }
        return result;
    }

    public static T DeepCopy<T>(T item)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        formatter.Serialize(stream, item);
        stream.Seek(0, SeekOrigin.Begin);
        T result = (T)formatter.Deserialize(stream);
        stream.Close();
        return result;
    }

    public static long GetCurrentTime()
    {
        return DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    public static string ConvertSecondsToTime(long seconds)
    {
        return TimeSpan.FromSeconds(seconds).Hours + ":" + TimeSpan.FromSeconds(seconds).Minutes + ":" + TimeSpan.FromSeconds(seconds).Seconds;
    }

}
