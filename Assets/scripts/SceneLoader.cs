using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour 
{
    public static string MAIN_MENU = "main_menu";

    public void ExitGame()
    {
        Application.Quit();
    }

    public static void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void LoadSceneInst(string scene)
    {
        LoadScene(scene);
    }
}