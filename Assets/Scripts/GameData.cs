using System.Collections.Generic;


[System.Serializable]
public class GameData
{
    public List<int> ChosenForestTiles { get; set; }
    public int Money { get; set; }
    public List<WhItemData> WhItems { get; set; }
    public List<TileInfo> ForestTiles { get; set; }

    public int CurrentLocationId { get; set; }
    public int TurnsMade { get; set; }
    public int TurnsMaxCount { get; set; }

    public GameData(List<WhItemData> userResources, int money, List<int> chosenForestTiles)
    {        
        WhItems = userResources;
        Money = money;
        ChosenForestTiles = chosenForestTiles;

        CurrentLocationId = 0;
    }
}
