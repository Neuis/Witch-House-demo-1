using System;

[Serializable]
public class WhItemInfo
{
    public int id;
    public string name;
    public int title_text_id = 0;
    public string required_tile_type = "";
    public int sell_price = 0;
    public int buy_price = 0;
    public int food_value = 0;    

    private int myProperty = 4;
    // Declare MyProperty.
    public int MyProperty
    {
        get
        {
            return myProperty;
        }
        set
        {
            myProperty = value;
        }
    }
}
