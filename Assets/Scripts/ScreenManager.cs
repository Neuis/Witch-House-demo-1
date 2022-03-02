using UnityEngine.SceneManagement;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance = null;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("ScreenManager instance created successfully");
        }
        else
        {
            Debug.Log("You are trying to create instance of ScreenManager but you already have one");
        }
    }
    public void OpenFightScreen()
    {        
        SceneManager.LoadScene("FightScene");
    }

    public void OpenStartingScreen()
    {        
        SceneManager.LoadScene("StartScene");
    }
}
