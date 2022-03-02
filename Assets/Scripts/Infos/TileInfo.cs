using System;

[Serializable]
public class TileInfo
{
    public int id;
    public string name;
    public int title_text_id = 0;
    public int description_text_id = 0;
    public int parentId;
    public string tile_type = "";
    public int priority;
    public string stack_tile_type = "";
    //public int required_stack_amount;
    //public int required_wh_amount;
    public int descendant_id = -1;
    public int number_to_gather_to_open_descendant = -1;
    public int number_to_gather_to_complete_research = -1;
    public RequiredResourceInfo[] research_required_resources = { };
    public int min_number_of_tiles_to_gather = Constants.DEFAULT_MIN_NUM_OF_TILES_TO_GATHER;
    public ModifierInfo[] modifiers = { };

    public int ancestor_id = -1;
    public int max_search_number = -1;

}
