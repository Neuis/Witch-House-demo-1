using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LocationInfo
{
    public int id;
    public string name;
    public int title_text_id = 0;
    public string[] available_tile_types;
    public List<LocationTileChancesInfo> tiles_chances;
    public int start_journey_cost;

    public List<LocationTileChancesInfo> updatedTilesChances;

    public void PrepareLocationInfo()
    {
        updatedTilesChances = GameUtility.DeepCopy(tiles_chances);
    }

    public void UpdateTilesChances(List<LocationTileChancesInfo> newVal)
    {
        updatedTilesChances = GameUtility.DeepCopy(newVal);
    }

    public void ShowLocationChances()
    {        
        string st = "";
        foreach (LocationTileChancesInfo ltcInfo in updatedTilesChances)
        {
            foreach(int val in ltcInfo.chances)
            {
                st = st + val + " ";
            }
            st += "\n";
        }
        Debug.Log("CHANCES ARE: \n"+st);
    }

}
