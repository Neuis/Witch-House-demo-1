using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public static JSONReader instance = null;

    [SerializeField]
    private TextAsset tilesJson;
    [SerializeField]
    private TextAsset completeResourcesJson;
    [SerializeField]
    private TextAsset locationsJson;
    [SerializeField]
    private TextAsset localizationJson;
    [SerializeField]
    private TextAsset tileTypesJson;

    private TileInfos tileInfos;
    private WhItemInfos whItemInfos;
    private LocationInfos locationInfos;
    private LocalizationInfos localizationInfos;
    private TileTypeInfos tileTypeInfos;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("You are trying to create instance of JSONReader but you already have one");
        }
        DontDestroyOnLoad(gameObject);
        PrepareJson();
    }

    void PrepareJson()
    {
        tileInfos = JsonUtility.FromJson<TileInfos>(tilesJson.text);
        tileInfos.ImproveTileInfos();
        whItemInfos = JsonUtility.FromJson<WhItemInfos>(completeResourcesJson.text);
        locationInfos = JsonUtility.FromJson<LocationInfos>(locationsJson.text);
        locationInfos.PrepareLocationInfos();
        localizationInfos = JsonUtility.FromJson<LocalizationInfos>(localizationJson.text);
        tileTypeInfos = JsonUtility.FromJson<TileTypeInfos>(tileTypesJson.text);
    }

    public TileTypeInfos GetTileTypes()
    {
        return tileTypeInfos;
    }

    public TileTypeInfo GetTileTypeInfoByTileType(string tileType)
    {
        TileTypeInfo result = null;
        TileTypeInfo[] infos = tileTypeInfos.tile_types;
        foreach (TileTypeInfo info in infos)
        {
            if (info.tile_type == tileType)
            {
                result = info;
                break;
            }
        }
        return result;
    }

    public LocalizationInfo[] GetLocalizationInfos()
    {
        return localizationInfos.texts;
    }

    public LocalizationInfo GetLocalizationById(int localizationId)
    {
        LocalizationInfo result = null;
        LocalizationInfo[] localizations = localizationInfos.texts;
        foreach (LocalizationInfo info in localizations)
        {
            if (info.id == localizationId)
            {
                result = info;
                break;
            }
        }
        return result;
    }

    public TileInfo GetRandomTileInfo()
    {
        int rnd = Random.Range(0, tileInfos.forest_tiles.Length); 
        return GetTileInfoById(tileInfos.forest_tiles[rnd].id);
    }

    public LocationInfo[] GetLocationInfos()
    {
        return locationInfos.locations;
    }
    public LocationInfo GetLocationInfoById(int locationId)
    {
        LocationInfo result = null;
        LocationInfo[] locations = locationInfos.locations;
        foreach (LocationInfo info in locations)
        {
            if (info.id == locationId)
            {
                result = info;
                break;
            }
        }
        return result;
    }

    public TileInfo GetTileInfoByTileType(string tileType)
    {
        TileInfo result = null;
        TileInfo[] tiles = tileInfos.forest_tiles;
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

    public TileInfo GetTileInfoById(int id)
    {
        TileInfo result = null;
        List<TileInfo> tiles = tileInfos.allTiles;
        foreach (TileInfo info in tiles)
        {
            if (info.id == id)
            {
                result = info;
                break;
            }
        }
        return result;
    }

    public TileInfos GetTileInfos()
    {
        return tileInfos;
    }

    public WhItemInfo[] GetWhItemInfos()
    {
        return whItemInfos.complete_resources;
    }

    public WhItemInfo GetWhItemInfoById(int id)
    {
        WhItemInfo result = null;
        WhItemInfo[] resources = whItemInfos.complete_resources;
        foreach (WhItemInfo info in resources)
        {
            if (info.id == id)
            {
                result = info;
                break;
            }
        }
        return result;
    }

    public WhItemInfo GetWhItemInfoByTileType(string tileType)
    {
        WhItemInfo result = null;
        WhItemInfo[] resources = whItemInfos.complete_resources;
        foreach (WhItemInfo info in resources)
        {
            if (info.required_tile_type == tileType)
            {
                result = info;
                break;
            }
        }
        return result;
    }

    public List<WhItemInfo> GetFoodWhItems()
    {
        List<WhItemInfo> result = new List<WhItemInfo>();
        WhItemInfo[] resources = whItemInfos.complete_resources;
        foreach (WhItemInfo info in resources)
        {
            if (info.food_value > 0)
            {
                result.Add(info);
            }
        }
        return result;
    }
}
