using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants 
{
    public static List<string> TILE_TYPES_FOREST = new List<string> { "flower", "tree", "mushroom", "berry", "spider", "vegetable" };
    public static Vector3 BUTTON_PRESSED_SCALE = new Vector3(0.9f, 0.9f, 1f);
    public static Vector3 BUTTON_NOT_PRESSED_SCALE = new Vector3(1f, 1f, 1f);

    public const int TILE_SIZE = 88;
    public const int TILE_SIZE_HALF = TILE_SIZE / 2;
    public const int DEFAULT_MIN_NUM_OF_TILES_TO_GATHER = 3;
    public const float TILE_PUSH_FORCE = 10f;
    public const string DIALOGUE_TEACHER = "teacher";
    public const string DIALOGUE_PLAYER = "player";
    public const string DIALOGUE_FROG = "frog";
    public const string DIALOGUE_HUNTER = "hunter";
    public const string TILES_PATH = "tiles/";
    public const string ITEMS_PATH = "items/";
    public const string ROOMS_PATH = "rooms/";
    public const string TOOLS_PATH = "tools/";
    public const string WORKERS_PATH = "workers/";
    public const string CURRENCY_PATH = "currency/";
    public const string LOCATIONS_PATH = "locations/";
    public const string PREMIUM_SHOP_PATH = "shop/";
    public const string TUTORIAL_PATH = "tutorial/";
    public static int[] STARTING_ROOM_IDS = { 1, 6 };
    public const int ROOM_HEIGHT = 270;
    public const int ITEM_COUNT_BG_PADDING = 10;
    public static Color32 COLOR_WHITE = new Color32(255, 255, 255, 255);
    public static Color32 COLOR_WHITE_TRANSPARENT = new Color32(255, 255, 255, 100);
    public static Color32 COLOR_AVAILABLE_GREEN = new Color32(58, 179, 60, 255);
    public static Color32 COLOR_UNAVAILABLE_RED = new Color32(179, 58, 60, 255);
    //public static Color32 LINES_AND_CIRCLES_COLOR_INACTIVE = new Color32(100, 100, 100, 255);
    //public static Color32 LINES_AND_CIRCLES_COLOR_ACTIVE = new Color32(220, 110, 20, 255);

    public static Color32 LINES_AND_CIRCLES_COLOR_INACTIVE = new Color32(100, 100, 100, 155);
    //public static Color32 LINES_AND_CIRCLES_COLOR_ACTIVE = new Color32(255, 255, 255, 255);
    public static Color32 LINES_AND_CIRCLES_COLOR_ACTIVE = new Color32(140, 200, 255, 255);

    public static Color32 BUTTON_COLOR_SOUL_BOTTLE = new Color32(100, 155, 195, 255);
    public static Color32 BUTTON_COLOR_SOUL_CRYSTAL = new Color32(150, 100, 185, 255);
    public static Color32 BUTTON_COLOR_DEFAULT = new Color32(100, 185, 100, 255);
    public static Color32 ENERGY_CELL_COLOR_EMPTY = new Color32(23, 37, 50, 255);
    public static Color32 ENERGY_CELL_COLOR_WARNING = new Color32(235, 190, 15, 255);
    public static Color32 ROOM_PREVIEW_COLOR_HIGHLIGHT = new Color32(220, 150, 100, 255);
    public static Color32 FIGHT_BG_COLOR_FOREST = new Color32(47, 77, 69, 255);//(10, 15, 35, 255);
    public static Color32 FIGHT_BG_COLOR_RUINS = new Color32(54, 54, 54, 255);

    public const int TILE_RARITY_COMMON = 1;
    public const int TILE_RARITY_UNCOMMON = 2;
    public const int TILE_RARITY_MERGE_ONLY = 3;    

    public const int RUINS_FOOD_ENERGY_FOR_SOULS = 10;
    public const int SAVE_TILES_COST = 2;

    public const int TILE_ID_CURRENCY = 0;

    public const int STARTING_FLOWER_TILE_ID = 2;
    public const int STARTING_TREE_TILE_ID = 1;
    public const int STARTING_MUSHROOM_TILE_ID = 3;
    public const int SECOND_STARTING_MUSHROOM_TILE_ID = 16;
    public const int STARTING_BERRY_TILE_ID = 4;
    public const int STARTING_SPIDER_TILE_ID = 5;
    public const int STARTING_VEGETABLE_TILE_ID = 6;

    public const int LOCATION_ID_FOREST = 1;
    public const int LOCATION_ID_RUINS = 2;

    public const int NAVIGATE_BTN_ROOMS_ID = 1;
    public const int NAVIGATE_BTN_MARKET_ID = 2;
    public const int NAVIGATE_BTN_ENCYCLOPEDIA_ID = 3;
    public const int NAVIGATE_BTN_PORTAL_ID = 4;
    public const int NAVIGATE_BTN_TOOLS_ID = 5;
    public const int NAVIGATE_BTN_WORKERS_ID = 6;

    public const int FOREST_DEFAULT_TURNS = 999;
    public const int FOREST_ADDITIONAL_TURNS = 2;
    public const int RUINS_DEFAULT_TURNS = 10;
    public const int RUINS_ADDITIONAL_TURNS = 10;

    public const int ROOM_MAIN_ID = 1;
    public const int ROOM_WORKERS_ID = 2;
    public const int ROOM_WORKSHOP_ID = 3;
    public const int ROOM_ENERGY_ID = 4;
    public const int ROOM_KITCHEN_ID = 5;
    public const int ROOM_PORTAL_ID = 6;
    public const int ROOM_MARKET_ID = 9;

    public const int PORTAL_LVL_OPEN_ADDITIONAL_FOREST_TILES = 2;
    public const int PORTAL_LVL_OPEN_RUINS_LOCATION = 3;
    public const int WORKERS_ROOM_LVL_BTN_NOTIFICATION = 1;
    public const int MAIN_ROOM_LVL_TILES_SAVE_ENABLED = 2;

    public const int VESSELS_NUMBER_ADDED_AFTER_ROOM_BUILT = 2;
    public const int CURRENCY_SOULS_ID = 1;
    public const int CURRENCY_SOUL_STONES_ID = 2;
    public const int CURRENCY_RUB_ID = 3;
    public const int CURRENCY_USD_ID = 4;
    public const int TOOLS_MAX_AMOUNT = 5;
    public const int WH_RESOURCE_VESSEL_ID = 0;
    public const int TILES_REQUIRED_AMOUNT_TO_FORM_A_STACK = 8;
    public const int TILES_REQUIRED_AMOUNT_TO_FORM_CURRENCY_STACK = 8;
    public const string TOOL_ACTION_TYPE_GATHER = "gather";
    public const string WORKER_ABILITY_GATHER_WH_ITEM_FASTER = "gather_wh_item_faster";
    public const string CURRENCY_STACK_TILE_TYPE = "currency";
    public const int DEFAULT_CURRENCY_TILE_MONEY_AMOUNT = 40;
    public const float ROOM_TITLE_FIELD_WIDTH_DEFAULT_WITHOUT_ADDITION = 150;
    public const float ROOM_TITLE_FIELD_WIDTH_ADDITION = 30;
    public const int TURNS_LEFT_TO_SHOW_WARNING = 3;
    
    public const string TILE_TYPE_VEGETABLE = "vegetable";
    public const string TILE_TYPE_SPIDER = "spider";

}
