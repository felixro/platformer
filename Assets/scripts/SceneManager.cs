using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour 
{
    public void ExitGame()
    {
        Application.Quit();
    }

    public void loadScene(string scene)
    {
        Application.LoadLevel(scene);
    }
}