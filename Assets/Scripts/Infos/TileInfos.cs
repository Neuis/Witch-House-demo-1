using System;
using System.Collections.Generic;

[Serializable]
public class TileInfos
{
    public TileInfo[] other_tiles;
    public TileInfo[] forest_tiles;
    public List<TileInfo> allTiles;

    public void ImproveTileInfos()
    {
        allTiles = new List<TileInfo>();
        allTiles.AddRange(other_tiles);
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
