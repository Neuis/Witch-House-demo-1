using System;
using System.Collections.Generic;

[Serializable]
public class TileChanceInfo
{
    public TileInfo[] other_tiles;
    public TileInfo[] forest_tiles;
    public TileInfo[] ruins_tiles;
    public List<TileInfo> ruins_tiles_list;
    public List<TileInfo> allTiles;
    public List<TileInfo> allTilesExceptForest;

    public void ImproveTileInfos()
    {
        ruins_tiles_list = new List<TileInfo>();
        ruins_tiles_list.AddRange(ruins_tiles);

        allTilesExceptForest = new List<TileInfo>();
        allTilesExceptForest.AddRange(other_tiles);
        allTilesExceptForest.AddRange(ruins_tiles);

        allTiles = new List<TileInfo>();
        allTiles.AddRange(allTilesExceptForest);
        allTiles.AddRange(forest_tiles);


        TileInfo tempInfo;
        foreach (TileInfo tInfo in forest_tiles)
        {
            tempInfo = GetForestTileById(tInfo.descendant_id);
            if (tempInfo != null)
            {
                tempInfo.ancestor_id = tInfo.id;
                tempInfo.max_search_number = tInfo.number_to_gather_to_open_descendant;
            }
        }
    }

    private TileInfo GetForestTileById(int tileId)
    {
        TileInfo result = null;
        foreach(TileInfo tInfo in forest_tiles)
        {
            if (tInfo.id == tileId)
            {
                result = tInfo;
                break;
            }
        }
        return result;
    }
}
