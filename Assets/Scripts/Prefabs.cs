using UnityEngine;

public class Prefabs : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject circlePrefab;
    public GameObject stackIndicatorPrefab;
    public GameObject linePrefabHorizontal;
    public GameObject linePrefabDiagonal;
    public GameObject itemPrefab;

    public static Prefabs instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("PREFABS instance created successfully");
        }
        else
        {
            Debug.LogError("You are trying to create instance of PREFABS but you already have one");
        }
        DontDestroyOnLoad(gameObject);
    }
}
