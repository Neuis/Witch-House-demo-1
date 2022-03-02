using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightField : MonoBehaviour
{
    private const int FIELD_ROWS = 6;
    private const int FIELD_COLUMNS = 6;
    private const float TILE_MOVE_TIME = 0.4f;
    private const string DEFAULT_FIRST_TILE_TYPE = "none";
    private const float STACK_INFO_WINDOW_SHOW_TIMEOUT = 1f;

    [SerializeField]
    private GameObject tilesContainer;
    [SerializeField]
    private GameObject circlesLayer;
    [SerializeField]
    private GameObject linesContainer;
    [SerializeField]
    private GameObject fieldCover;

    [SerializeField]
    private GatheringProgressWindow gatheringProgressWindow;
    [SerializeField]
    private GameObject warehouseItemsContainer;
    [SerializeField]
    private StackInfoWindow stackInfoWindow;

    private List<Tile> tilesMarked = new List<Tile>();
    private string firstTileType = DEFAULT_FIRST_TILE_TYPE;
    private Dictionary<int, int> tilesToAdd = null;
    private int numberOfTilesStartedMoving;
    private int numberOfTilesFinishedMoving;
    private bool gatheringProgressWindowShouldBeVisible;

    private GameObject[][] tilesGrid;
    private int[][] tilesFlagsGrid;

    private bool allTilesShouldBeEnabled = false;
    private List<WarehouseItem> whItemsList;
    private int _turnsMadeCount;
    private Color32 currentLinesAndCirclesColor;
    private Transform stackInfoWindowTransform;
    private List<TileInfo> availableTileInfos;
    private int stackBonusTilesAmount;
    private TileInfo stackBonusTileInfo;
    private int numberOfTilesToBeCreated;
    private bool firstTileIsPressed;
    private bool canStartStackInfoWindowTimoutCheck;

    private int prevNumOfMarkedTiles;
    private int actualTurnsMaxCount;
    private LocationInfo currentLocationInfo;

    private List<TileInfo> fieldTileInfosForSave;
    private Dictionary<string, List<string>> gatherablePairs;
    private Dictionary<string, int> bonusesRequiredTilesNumbers;
    private Dictionary<string, int> minNumberOfTilesToGather;

    private int stackRequiredAmount;
    private bool stackRequiredAmountShouldBeRecalculated;
    private float turnsPerStage;    //for example whole journey has 12 turns. journey divided in 3 stages. //each stage has different probability for particular tiles creation
    private int[] currentTileChances;

    private void Awake()
    {
        FillWarehouse();
        stackInfoWindowTransform = stackInfoWindow.transform;
    }

    void Start()
    {
        PrepareScreen();
    }

    public void PrepareScreen()
    {
        stackRequiredAmountShouldBeRecalculated = true;
        prevNumOfMarkedTiles = 0;
        PrepareAvailableTileInfos();
        gatherablePairs = ModifiersManager.GetGatherablePairs(availableTileInfos);
        bonusesRequiredTilesNumbers = ModifiersManager.GetBonus_RequiredTilesNumbers(availableTileInfos);
        minNumberOfTilesToGather = ModifiersManager.Get_MinNumberOfTilesToGather(availableTileInfos);
        currentLocationInfo = JSONReader.instance.GetLocationInfoById(User.instance.GetJourneyLocationId());
        actualTurnsMaxCount = User.instance.GetActualTurnsMaxCount();
        
        turnsPerStage = (float)(actualTurnsMaxCount) / (float)currentLocationInfo.updatedTilesChances.Count;

        allTilesShouldBeEnabled = false;
        UpdateTurnsMadeCount(User.instance.GetTurnsMade());
        stackBonusTilesAmount = 0;
        firstTileIsPressed = false;
        gatheringProgressWindowShouldBeVisible = false;
        canStartStackInfoWindowTimoutCheck = true;
        
        FillStartingTiles();
        ClearDrawings();
        gatheringProgressWindow.gameObject.SetActive(false);
        UpdateWarehouse();
        stackInfoWindow.HideWindow();

        //we need to save all tiles on their positions, otherwise User can start fighting...  
        //...and without making 1st move restart game - tiles will be generated again
        UpdateUserDataAfterTurnHasBeenMade();   
    }

    private void UpdateTurnsMadeCount(int newValue)
    {
        _turnsMadeCount = newValue;
        int currentStage = Mathf.FloorToInt(_turnsMadeCount / turnsPerStage);
        if (currentStage >= currentLocationInfo.updatedTilesChances.Count)
        {
            currentStage = currentLocationInfo.updatedTilesChances.Count - 1;
        }
        currentTileChances = currentLocationInfo.updatedTilesChances[currentStage].chances;
    }

    private void PrepareAvailableTileInfos()
    {
        availableTileInfos = User.instance.GetFightTiles();
        availableTileInfos.Add(JSONReader.instance.GetTileInfoById(Constants.TILE_ID_CURRENCY));
    }

    private List<Tile> GetAllTilesOfThisType(string tileType)
    {
        List<Tile> result = new List<Tile>();
        Tile tile;
        foreach (Transform child in tilesContainer.transform)
        {            
            tile = child.gameObject.GetComponent<Tile>();
            if (tile!=null && tile.GetInfo().tile_type == tileType)
            {
                result.Add(tile);
            }
        }
        return result;
    }

    private void FillWarehouse()
    {
        GameUtility.ClearEverythingInside(warehouseItemsContainer);
        whItemsList = new List<WarehouseItem>();
        List<WhItemData> resources = User.instance.GetJourneyWhResources();
        GameObject itemView;
        WarehouseItem whItem;
        foreach (WhItemData res in resources)
        {
            itemView = Instantiate(Prefabs.instance.itemPrefab);
            itemView.transform.SetParent(warehouseItemsContainer.transform, false);
            whItem = itemView.GetComponent<WarehouseItem>();
            whItem.SetInfo(res.GetInfo());
            whItemsList.Add(whItem);
        }
    }

    private void UpdateWarehouse()
    {
        foreach (WarehouseItem whItem in whItemsList)
        {
            whItem.UpdateItemCount();
        }
    }

    private void FillStartingTiles()
    {
        GameUtility.ClearEverythingInside(tilesContainer);
        List<TileInfo> savedTiles = User.instance.GetSavedTiles();
        int counter = 0;
        for (int i = 0; i < FIELD_ROWS; i++)
        {
            for (int j = 0; j < FIELD_COLUMNS; j++)
            {
                if (savedTiles != null)
                {
                    AddOneTile(j, i, savedTiles[counter]);
                    counter++;
                } else
                {
                    AddOneTile(j, i);
                }               
            }
        }
        UpdateFieldTilesList();
    }

    private void AddOneTile(int indexForXPos, int indexForYPos, TileInfo savedTileInfo = null)
    {
        GameObject newTile;
        Tile tile;
        newTile = Instantiate(Prefabs.instance.tilePrefab);
        newTile.transform.SetParent(tilesContainer.transform, false);
        newTile.transform.localPosition = new Vector2(FindXPosition(indexForXPos), FindYPosition(indexForYPos));
        tile = newTile.GetComponent<Tile>();
        if (savedTileInfo != null)
        {
            //this is used only if fight should show previously saved tiles, 
            //like if user paid to save field or exited game in the middle of the journey
            tile.SetInfo(User.instance.GetTileInfoByTileType(savedTileInfo.tile_type));
        } else if (ShouldCreateStackTile())
        {
            tile.SetInfo(stackBonusTileInfo);
            stackBonusTilesAmount--;
        } else
        {
            tile.SetInfo(GetAppropriateTileInfo());
        }        
        tile.SetColumn(indexForXPos);
        tile.SetRow(indexForYPos);
        numberOfTilesToBeCreated--;
    }

    private bool ShouldCreateStackTile()
    {
        bool result = false;
        if (stackBonusTilesAmount > 0)
        {            
            int rnd = (int)Random.Range(1, numberOfTilesToBeCreated);
            result = rnd <= stackBonusTilesAmount;
        }
        
        return result;
    }

    private TileInfo GetAppropriateTileInfo()
    {
        int rnd = Random.Range(0, 100)+1;
        int counter = 0;
        foreach (int chance in currentTileChances)
        {            
            if (rnd <= chance)
            {
                break;
            } else
            {
                rnd -= chance;
            }
            counter++;
        }
        return User.instance.GetTileInfoByTileType(currentLocationInfo.available_tile_types[counter]);
    }

    private int FindXPosition(int i)
    {
        return (i - FIELD_COLUMNS / 2) * Constants.TILE_SIZE;
    }

    private int FindYPosition(int j)
    {
        return (j - FIELD_ROWS / 2) * Constants.TILE_SIZE;
    }

    private void FollowMousePosition(Transform follower)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        follower.position = mousePosition;
    }
    public void OnFieldMouseDown()
    {
        stackRequiredAmountShouldBeRecalculated = true;
        CheckUnderneathTile();
        FollowMousePosition(stackInfoWindowTransform);
    }

    public void OnFieldMouseMove()
    {
        CheckUnderneathTile();
        FollowMousePosition(stackInfoWindowTransform);
    }

    public void OnFieldMouseUp()
    {
        prevNumOfMarkedTiles = 0;
        firstTileIsPressed = false;
        canStartStackInfoWindowTimoutCheck = true;
        stackBonusTilesAmount = FindStackBonusTilesAmount();
        ClearDrawings();
        CheckIfTurnWasSuccessful();
        MakeAllTilesEnabledAgain();
        StartCoroutine(HideGatheringProgressWindow());
        CheckIfShouldShowStackInfoWindow();
    }

    private IEnumerator HideGatheringProgressWindow()
    {
        tilesMarked.Clear();
        gatheringProgressWindow.RecheckUserInfoAndUpdateWindow();
        UpdateGatheringProgressWindow();
        gatheringProgressWindowShouldBeVisible = false;
        yield return new WaitForSeconds(TILE_MOVE_TIME);
        if (!gatheringProgressWindowShouldBeVisible)
        {
            gatheringProgressWindow.gameObject.SetActive(false);
        }
    }

    private void CheckIfTurnWasSuccessful()
    {
        if (tilesMarked.Count>0 && tilesMarked.Count >= GetMinimumNumberOfTilesToGather())
        {
            numberOfTilesToBeCreated = tilesMarked.Count;
            UpdateTurnsMadeCount(_turnsMadeCount + 1);
            StartGatheringProcess();            
        }
    }

    private void StartGatheringProcess()
    {
        GatherResources();
        UpdateWarehouse();
        AddNewTiles();        
        StartCoroutine(WaitForDeletionAndStartMovingTiles());
    }

    private void ShowGatheringProgressWindow()
    {
        if (tilesMarked.Count > 0)
        {
            WhItemData whData = User.instance.GetWhItemByTileType(tilesMarked[0].GetInfo().tile_type);

            //whData can be null for tiles that do not generate whresources, such as "currency" tile            
            if (whData != null) 
            {
                if (CheckIfAllMarkedTilesAreOfTheSameType())
                {
                    gatheringProgressWindow.SetOneItemInfo(whData, User.instance.GetTileRequiredAmountToFormWhItem(tilesMarked[0].GetInfo()));
                } else
                {
                    TileInfo secondInfo = FindMarkedTileWhichTypeIsNotTheSameAsTheFirstTile();
                    gatheringProgressWindow.SetTwoItemInfos(
                        whData, 
                        User.instance.GetTileRequiredAmountToFormWhItem(tilesMarked[0].GetInfo()),
                        User.instance.GetWhItemByTileType(secondInfo.tile_type), 
                        User.instance.GetTileRequiredAmountToFormWhItem(secondInfo)
                        );
                }                
                gatheringProgressWindow.gameObject.SetActive(true);
            }        
        }        
    }

    //there are some tiles that can be gathered together with tiles of another type
    private TileInfo FindMarkedTileWhichTypeIsNotTheSameAsTheFirstTile()
    {
        TileInfo result = null;
        if (tilesMarked.Count > 1)
        {
            string firstTileType = tilesMarked[0].GetInfo().tile_type;
            foreach (Tile tile in tilesMarked)
            {
                if (tile.GetInfo().tile_type != firstTileType)
                {
                    result = tile.GetInfo();
                    break;
                }
            }
        }        
        return result;
    }

    private void UpdateGatheringProgressWindow()
    {
        gatheringProgressWindowShouldBeVisible = true;
        if (CheckIfAllMarkedTilesAreOfTheSameType())
        {
            gatheringProgressWindow.SetOneItemAddedProgress(tilesMarked.Count);
        } else
        {
            int val = FindHowMuchOfTheFirstTileType();
            gatheringProgressWindow.SetTwoItemsAddedProgress(val, tilesMarked.Count - val);
        }        
    }

    private int FindHowMuchOfTheFirstTileType()
    {
        int result = 0;
        if (tilesMarked.Count > 0)
        {
            string firstTileType = tilesMarked[0].GetInfo().tile_type;
            foreach (Tile tile in tilesMarked)
            {
                if (tile.GetInfo().tile_type == firstTileType)
                {
                    result++;
                }
            }
        }

        return result;
    }

    private void MakeAllTilesEnabledAgain()
    {
        firstTileType = DEFAULT_FIRST_TILE_TYPE;
        allTilesShouldBeEnabled = true;
        foreach (Transform child in tilesContainer.transform)
        {
            ChangeTileAvailability(child.GetComponent<Tile>());
        }
        allTilesShouldBeEnabled = false;
    }

    private void GatherResources()
    {
        if (CheckIfAllMarkedTilesAreOfTheSameType())
        {
            User.instance.AddGatheredTilesByTileInfo(tilesMarked[0].GetInfo(), tilesMarked.Count);
        } else
        {
            TileInfo secondTileInfo = FindMarkedTileWhichTypeIsNotTheSameAsTheFirstTile();
            int firstTileAmount = FindHowMuchOfTheFirstTileType();
            User.instance.AddGatheredTilesByTileInfo(tilesMarked[0].GetInfo(), firstTileAmount);
            User.instance.AddGatheredTilesByTileInfo(secondTileInfo, tilesMarked.Count - firstTileAmount);
        }        

        tilesToAdd = new Dictionary<int, int>();
        for (int i = 0; i < FIELD_COLUMNS; i++)
        {
            tilesToAdd[i] = 0;
        }
        foreach (Tile tile in tilesMarked)
        {
            tilesToAdd[tile.GetColumnNumber()]++;
            tile.transform.SetParent(null);
            Destroy(tile.gameObject);
        }
        tilesMarked.Clear();
    }

    private IEnumerator WaitForDeletionAndStartMovingTiles()
    {
        //we need to wait because even if you called Destroy function, object will be destroyed only after one frame
        yield return new WaitForEndOfFrame();
        UpdateGrids();
        yield return null;
    }

    private void AddNewTiles()
    {
        //this function is executed after resources was gathered on the field. we add new tiles on top and move all tiles that need to be moved  down
        int numberOfTilesInColumn;
        foreach (int i in tilesToAdd.Keys)
        {
            numberOfTilesInColumn = tilesToAdd[i];
            for (int j=0;j< numberOfTilesInColumn; j++)
            {
                AddOneTile(i, j+FIELD_ROWS);
            }
        }
    }

    private void UpdateGrids()
    {
        tilesGrid = new GameObject[FIELD_COLUMNS][];
        tilesFlagsGrid = new int[FIELD_COLUMNS][];
        int i;
        int j;
        for (i = 0; i < tilesGrid.Length; i++)
        {
            tilesGrid[i] = new GameObject[FIELD_ROWS*2];
            tilesFlagsGrid[i] = new int[FIELD_ROWS * 2];
            for (j = 0; j < FIELD_ROWS * 2; j++)
            {
                tilesGrid[i][j] = null;
                tilesFlagsGrid[i][j] = 0;
            }
        }

        foreach (Transform child in tilesContainer.transform)
        {
            i = GetIndexFromXPos((int)child.localPosition.x);
            j = GetIndexFromYPos((int)child.localPosition.y);
            tilesGrid[i][j] = child.gameObject;
            tilesFlagsGrid[i][j] = 1;
        }
        MoveTilesDown();
    }

    private void MoveTilesDown()
    {
        numberOfTilesStartedMoving = 0;
        numberOfTilesFinishedMoving = 0;
        fieldCover.SetActive(true);

        int columnsCount = tilesFlagsGrid.Length;
        int rowsCount;
        for (int i = 0; i < columnsCount; i++) 
        {
            rowsCount = tilesFlagsGrid[i].Length;
            for (int j = 0; j < rowsCount; j++)
            {
                if (tilesFlagsGrid[i][j] == 0)
                {
                    for (int k = j + 1; k < rowsCount; k++)
                    {
                        if (tilesFlagsGrid[i][k] == 1)
                        {
                            tilesFlagsGrid[i][k] = 0;
                            tilesFlagsGrid[i][j] = 1;
                            MoveTileDown(tilesGrid[i][k], new Vector2(tilesGrid[i][k].transform.localPosition.x, FindYPosition(j)));
                            break;
                        }
                    }
                }
            }
        }
    }

    private void MoveTileDown(GameObject objToMove, Vector2 finalPosition)
    {
        numberOfTilesStartedMoving++;
        StartCoroutine(MoveTileToPosition(objToMove, finalPosition));
    }

    private IEnumerator MoveTileToPosition(GameObject objToMove, Vector2 finalPosition)
    {
        Vector2 startingPos = objToMove.transform.localPosition;
        float elapsedTime = 0;
        while (elapsedTime <= TILE_MOVE_TIME)
        {
            if (objToMove != null)
            {
                objToMove.transform.localPosition = Vector3.Lerp(startingPos, finalPosition, (elapsedTime / TILE_MOVE_TIME));
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (objToMove != null)
        {
            objToMove.GetComponent<Tile>().MakeItStop();
        }
        objToMove.transform.localPosition = finalPosition;

        CheckIfAllTilesAreOnPosition();
    }

    private int GetIndexFromXPos(int xPos)
    {
        return xPos / Constants.TILE_SIZE + FIELD_COLUMNS / 2;
    }

    private int GetIndexFromYPos(int yPos)
    {
        return yPos / Constants.TILE_SIZE + FIELD_ROWS / 2;
    }

    private void UpdateFieldTilesList()
    {
        Dictionary<int, TileInfo> tempDict = new Dictionary<int, TileInfo>();
        fieldTileInfosForSave = new List<TileInfo>();
        Transform transform = tilesContainer.transform;
        Tile tile;
        int i;
        int j;
        int tempUID;
        foreach (Transform child in transform)
        {          
            i = GetIndexFromXPos((int)child.localPosition.x);
            j = GetIndexFromYPos((int)child.localPosition.y);
            tile = child.GetComponent<Tile>();
            tempUID = j * FIELD_COLUMNS + i;
            tile.SetTestUID(tempUID);
            tempDict[tempUID] = tile.GetInfo();
        }
        int leng = transform.childCount;
        for (i = 0; i < leng; i++) 
        {
            fieldTileInfosForSave.Add(tempDict[i]);
        }
    }

    private void UpdateUserDataAfterTurnHasBeenMade()
    {
        UpdateFieldTilesList();
        User.instance.SetAndSaveNewJourneyInfo(fieldTileInfosForSave, _turnsMadeCount);
    }

    private void CheckIfAllTilesAreOnPosition()
    {
        numberOfTilesFinishedMoving++;
        if (numberOfTilesFinishedMoving == numberOfTilesStartedMoving)
        {
            ProceedAfterTilesStoppedMoving();
        }
    }

    private void ProceedAfterTilesStoppedMoving()
    {
        UpdateUserDataAfterTurnHasBeenMade();        
        fieldCover.SetActive(false);
    }
    private void OnFirstTilePressed()
    {
        if (!firstTileIsPressed)
        {
            firstTileIsPressed = true;
            if (tilesMarked.Count == 1)
            {
                firstTileType = tilesMarked[0].GetInfo().tile_type;
                foreach (Transform child in tilesContainer.transform)
                {
                    ChangeTileAvailability(child.GetComponent<Tile>());
                }

                if (StackTileCanBeProduced())
                {
                    SetStackWindowInfos();
                }                
            }
        }           
    }

    private void SetStackWindowInfos()
    {
        stackBonusTileInfo = GetTileInfoByType(tilesMarked[0].GetInfo().stack_tile_type);
        stackInfoWindow.SetRightItemInfo(stackBonusTileInfo);
        stackInfoWindow.SetLeftItemInfo(tilesMarked[0].GetInfo());
    }

    private bool StackTileCanBeProduced()
    {
        return tilesMarked[0].GetInfo().stack_tile_type.Length > 0;
    }

    private void ChangeTileAvailability(Tile tile)
    {
        if (allTilesShouldBeEnabled)
        {
            tile.MakeEnabled();
        } else if (!CheckIfTileShouldBeEnabled(tile.GetInfo()))
        {
            tile.MakeDisabled();
        }
    }

    private bool CheckIfTileShouldBeEnabled(TileInfo tInfo)
    {
        return tInfo.tile_type==tilesMarked[0].GetInfo().tile_type || gatherablePairs[firstTileType].Contains(tInfo.tile_type);
    }

    private void CheckUnderneathTile()
    {
        Vector2 vect = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(vect, Vector2.zero,Mathf.Infinity);
        if (hit && hit.collider != null)
        {
            if (CheckIfTileIsAvailable(hit.collider))
            {
                MarkOrUnmarkTile(hit.collider.gameObject);
                DrawLinesAndCircles();
                
                OnFirstTilePressed();
                CheckIfGatheringProgressWindowChanged();
                CheckIfShouldShowStackInfoWindow();
            }            
        }
    }

    private bool CheckIfAllMarkedTilesAreOfTheSameType()
    {
        bool result = true;
        if (tilesMarked.Count > 0)
        {
            string firstTileType = tilesMarked[0].GetInfo().tile_type;
            foreach (Tile tile in tilesMarked)
            {
                if (tile.GetInfo().tile_type != firstTileType)
                {
                    result = false;
                    break;
                }
            }
        } else
        {
            result = false;
        }
        return result;
    }

    private void CheckIfShouldShowStackInfoWindow()
    {
        if (tilesMarked.Count < 1 || !CheckIfAllMarkedTilesAreOfTheSameType())
        {
            stackInfoWindow.HideWindow();
        }
        else if (StackTileCanBeProduced() && FindStackRequiredAmount()>0)
        {
            if (tilesMarked.Count > 1)
            {
                canStartStackInfoWindowTimoutCheck = true;
                stackBonusTilesAmount = FindStackBonusTilesAmount();
                stackInfoWindow.UpdateAmount(stackBonusTilesAmount);
                stackInfoWindow.ChangeShowModeToSingle();
                stackInfoWindow.ShowWindow();
            }
            else if (tilesMarked.Count == 1)
            {
                if (stackInfoWindow.IsInSingleMode())
                {
                    stackInfoWindow.HideWindow();
                }
                if (canStartStackInfoWindowTimoutCheck)
                {
                    canStartStackInfoWindowTimoutCheck = false;
                    StartCoroutine(CheckIfDoubleStackInfoShouldBeShown());
                }

            }
        }
        else
        {
            stackInfoWindow.HideWindow();
        }
    }

    private IEnumerator CheckIfDoubleStackInfoShouldBeShown()
    {
        yield return new WaitForSeconds(STACK_INFO_WINDOW_SHOW_TIMEOUT);
        if (tilesMarked.Count == 1)
        {
            stackInfoWindow.ChangeShowModeToDouble();
            stackInfoWindow.ShowWindow();           
        }
    }

    private int FindStackBonusTilesAmount()
    {
        if (CheckIfAllMarkedTilesAreOfTheSameType())
        {
            int stackRequiredAmount = FindStackRequiredAmount();
            //stackRequiredAmount can be 0 if we click on CURRENCY tile
            if (stackRequiredAmount > 0)
            {
                return Mathf.FloorToInt(tilesMarked.Count / stackRequiredAmount);
            } else
            {
                return 0;
            }            
        } else
        {
            return 0;
        }        
    }

    private bool CheckIfTileIsAvailable(Collider2D targetTile)
    {        
        Vector2 targetTilePosition = targetTile.transform.localPosition;
        Tile tile = targetTile.GetComponent<Tile>();
        //checks if new tile is a neighbour of previous tile
        bool result;
        if (tilesMarked.Count > 0)
        {
            result = IsNeighbour(targetTilePosition) && tile.IsAvailable();
        } else
        {
            result = true;
        }
        
        return result;
    }

    private bool IsNeighbour(Vector2 newTile)
    {
        bool result;
        Vector2 lastAddedTile = tilesMarked[tilesMarked.Count - 1].transform.localPosition;
        float difX = Mathf.Abs(newTile.x - lastAddedTile.x);
        float difY = Mathf.Abs(newTile.y - lastAddedTile.y);
        result = (difX <= Constants.TILE_SIZE) && (difY <= Constants.TILE_SIZE);
        return result;
    }

    private void MarkOrUnmarkTile(GameObject obj)
    {
        Tile tile = obj.GetComponent<Tile>();
        if (tilesMarked.Contains(tile))
        {
            if (tilesMarked.Count > 1 && tilesMarked[tilesMarked.Count - 2] == tile)
            {
                tilesMarked.Remove(tilesMarked[tilesMarked.Count - 1]);
            }
        } else
        {
            tilesMarked.Add(tile);
        }
    }

    private void DrawLinesAndCircles()
    {
        ClearDrawings();
        FindCorrectLinesAndCirclesColor();
        GameObject newCircle;
        Tile tile;
        int stackRequiredAmount = FindStackRequiredAmount();
        bool starsAllowed = CheckIfAllMarkedTilesAreOfTheSameType();
        int leng = tilesMarked.Count;
        for (int i = 0; i < leng; i++)
        {
            tile = tilesMarked[i];
            if (starsAllowed && stackRequiredAmount > 0 && (i + 1) % stackRequiredAmount == 0) 
            {
                newCircle = Instantiate(Prefabs.instance.stackIndicatorPrefab);
            } else
            {
                newCircle = Instantiate(Prefabs.instance.circlePrefab);
                newCircle.GetComponent<Image>().color = currentLinesAndCirclesColor;
                
            }            
            newCircle.transform.SetParent(circlesLayer.transform, false);
            newCircle.transform.localPosition = new Vector2(tile.transform.localPosition.x+Constants.TILE_SIZE_HALF, tile.transform.localPosition.y + Constants.TILE_SIZE_HALF);
            
            if (i > 0)
            {
                DrawLineBetweenTwoPoints(circlesLayer.transform.GetChild(i - 1).localPosition, newCircle.transform.localPosition);
            }
        }
    }

    private int FindStackRequiredAmount()
    {
        int result = 0;
        if (stackRequiredAmountShouldBeRecalculated)
        {
            stackRequiredAmountShouldBeRecalculated = false;
            
            if (tilesMarked.Count > 0)
            {
                result = bonusesRequiredTilesNumbers[tilesMarked[0].GetInfo().tile_type];
            }
            stackRequiredAmount = result;
        } else
        {
            result = stackRequiredAmount;
        }

        return result;
    }

    private void FindCorrectLinesAndCirclesColor()
    {
        if (tilesMarked.Count >= GetMinimumNumberOfTilesToGather())
        {
            currentLinesAndCirclesColor = Constants.LINES_AND_CIRCLES_COLOR_ACTIVE;
        } else
        {
            currentLinesAndCirclesColor = Constants.LINES_AND_CIRCLES_COLOR_INACTIVE;
        }
    }

    private void ClearDrawings()
    {
        GameUtility.ClearEverythingInside(circlesLayer);
        GameUtility.ClearEverythingInside(linesContainer);
    }

    private void DrawLineBetweenTwoPoints(Vector3 point1, Vector3 point2)
    {
        GameObject newLine;
        if (point1.y != point2.y && point1.x != point2.x)
        {   //diagonal
            newLine = Instantiate(Prefabs.instance.linePrefabDiagonal);            
            if ((point1.x > point2.x && point1.y>point2.y) || (point1.x < point2.x && point1.y < point2.y))
            {
                newLine.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
            }
        } else
        {
            newLine = Instantiate(Prefabs.instance.linePrefabHorizontal);
            if (point1.x == point2.x)
            {   //vertical
                newLine.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }
        }

        newLine.transform.SetParent(linesContainer.transform, false);
        newLine.transform.localPosition = new Vector2((point1.x + point2.x) / 2, (point1.y + point2.y) / 2);
        newLine.GetComponent<Image>().color = currentLinesAndCirclesColor;
    }

    private int GetMinimumNumberOfTilesToGather()
    {
        return minNumberOfTilesToGather[tilesMarked[0].GetInfo().tile_type];
    }

    public TileInfo GetTileInfoByType(string tileType)
    {
        TileInfo result = null;
        foreach (TileInfo info in availableTileInfos)
        {
            if (info.tile_type == tileType)
            {
                result = info;
                break;
            }
        }
        return result;
    }

    private void CheckIfGatheringProgressWindowChanged()
    {
        if (prevNumOfMarkedTiles != tilesMarked.Count)
        {
            prevNumOfMarkedTiles = tilesMarked.Count;
            ShowGatheringProgressWindow();
            UpdateGatheringProgressWindow();
        }
    }

}
