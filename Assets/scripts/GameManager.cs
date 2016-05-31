using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public Level level;

    void Start ()
    {
        BeginGame();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void BeginGame()
    {
        level.buildLevel();
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        BeginGame();
    }
}