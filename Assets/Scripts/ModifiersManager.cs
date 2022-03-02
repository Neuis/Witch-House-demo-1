using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiersManager : MonoBehaviour
{
    public const string TYPE_CREATE_WH_RES = "create_wh_res";           //changes number of tiles requierd to create one wh item 
    public const string TYPE_CREATE_BONUS = "create_bonus";             //changes number of tiles required to create bonus tile
    public const string TYPE_ADDITIONAL_TURNS = "additional_turns";     //gives free turns to gather tiles of certain type
    public const string TYPE_GATHER_TOGETHER = "gather_together";       //allows to gather tiles of different types together (but bonus tiles will be disabled)
    public const string TYPE_MIN_GATHER_NUMBER = "min_gather_number";   //changes min. number of tiles required for gathering (by default, you need at least 3 tiles being gathered together)

    private static int FindModifierValue(string modifierType, ModifierInfo[] modifiers)
    {
        int result = 0;
        if (modifiers != null)
        {
            foreach (ModifierInfo mInfo in modifiers)
            {
                if (mInfo.type == modifierType)
                {
                    result += mInfo.value;
                }
            }
        }        
        return result;
    }
    public static int GetWhItem_RequiredTilesNumber(TileInfo tInfo) {
        int result = GameUtility.GetWhItemTilesNumberByTileInfo(tInfo);

        //check tile modifiers
        result += FindModifierValue(TYPE_CREATE_WH_RES, tInfo.modifiers);

        return result;
    }

    public static int GetBonus_RequiredTilesNumber(TileInfo tInfo)
    {
        int result = GameUtility.GetBonusTilesNumberByTileInfo(tInfo);

        //check tile modifiers
        result += FindModifierValue(TYPE_CREATE_BONUS, tInfo.modifiers);
        return result;
    }

    public static int GetFreeGatherTurns(TileInfo tInfo)
    {
        int result = FindModifierValue(TYPE_ADDITIONAL_TURNS, tInfo.modifiers);
        return result;
    }

    public static int GetMinGatherNumberOfTiles(TileInfo tInfo)
    {
        int result = Constants.DEFAULT_MIN_NUM_OF_TILES_TO_GATHER + FindModifierValue(TYPE_MIN_GATHER_NUMBER, tInfo.modifiers);
        return result;
    }

    public static Dictionary<string,List<string>> GetGatherablePairs(List<TileInfo> chosenTiles)
    {
        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

        foreach (TileInfo tInfo in chosenTiles)
        {
            result[tInfo.tile_type] = new List<string>();
        }

        foreach (TileInfo tInfo in chosenTiles)
        {
            foreach (ModifierInfo mInfo in tInfo.modifiers)
            {
                if (mInfo.type == TYPE_GATHER_TOGETHER)
                {
                    result[mInfo.target_type].Add(tInfo.tile_type);
                    result[tInfo.tile_type].Add(mInfo.target_type);
                }
            }
        }

        return result;
    }

    public static Dictionary<string,int> GetBonus_RequiredTilesNumbers(List<TileInfo> availableTiles)
    {
        Dictionary<string, int> result = new Dictionary<string, int>();

        foreach (TileInfo tInfo in availableTiles)
        {
            result[tInfo.tile_type] = GetBonus_RequiredTilesNumber(tInfo);
        }

        return result;
    }

    public static Dictionary<string, int> Get_MinNumberOfTilesToGather(List<TileInfo> availableTiles)
    {
        Dictionary<string, int> result = new Dictionary<string, int>();

        foreach (TileInfo tInfo in availableTiles)
        {
            result[tInfo.tile_type] = GetMinGatherNumberOfTiles(tInfo);
        }

        return result;
    }

    public static Dictionary<string, int> Get_FreeTileMoves()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();
        List<TileInfo> availableTiles = User.instance.GetFightTiles();

        //tiles modifiers
        foreach (TileInfo tInfo in availableTiles)
        {
            result[tInfo.tile_type] = FindModifierValue(TYPE_ADDITIONAL_TURNS, tInfo.modifiers);
        }

        return result;
    }
}
