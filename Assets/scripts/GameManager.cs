﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    public PowerUpManager powerupManager;

    public CanvasRenderer scoreboard;

    public Text player1Score;
    public Text player2Score;

    public float gameFieldBoundary = -50f;
    public float respawnTime = 2f;
    public KeyboardManager keyboardManager;

    public Camera player1CameraPrefab;
    public Camera player2CameraPrefab;

    public GameObject player1Prefab;
    public GameObject player2Prefab;

    public Transform player1Base;
    public Transform player2Base;

    public Level level;

    private GameObject player1;
    private PlayerDeath player1Death;

    private GameObject player2;
    private PlayerDeath player2Death;

    private Camera player1Camera;
    private Camera player2Camera;

    private float leftSpawnPoint = 1f;
    private float rightSpawnPoint = 30f;

    private const string CONSTANT_OBJECTS_NAME = "ConstantObjects";

    void Start()
    {
        BeginGame();
    }

    void Update ()
    {
        if (Input.GetKeyDown(keyboardManager.restartGameKey))
        {
            RestartGame();
        }

        if (player1 != null)
        {
            if (!player1.activeSelf && !player1Death.IsRespawning)
            {
                StartCoroutine(RespawnPlayer(player1));
                player1Death.IsRespawning = true;

                increasePlayerScore(Player.TWO, 1);
            }    

            if (player1.transform.position.y <= gameFieldBoundary)
            {
                increasePlayerScore(Player.TWO, 1);
                resetPlayerPosition(player1);
            }
        }

        if (player2 != null)
        {
            if (!player2.activeSelf && !player2Death.IsRespawning)
            {
                StartCoroutine(RespawnPlayer(player2));
                player2Death.IsRespawning = true;

                increasePlayerScore(Player.ONE, 1);
            }
                
            if (player2.transform.position.y <= gameFieldBoundary)
            {
                increasePlayerScore(Player.ONE, 1);
                resetPlayerPosition(player2);
            }
        }
    }

    public void BeginGame()
    {
        cleanupGame();

        {
            player1 = Instantiate (player1Prefab, Vector2.one, Quaternion.identity) as GameObject;
            player1Death = player1.GetComponent<PlayerDeath>();

            player1Camera = Instantiate (player1CameraPrefab, new Vector2(20f, 5f), Quaternion.identity) as Camera;
            SmoothFollow player1Follow = player1Camera.GetComponent<SmoothFollow>();
            player1Follow.setTarget(player1.transform);
        }

        {
            player2 = Instantiate (player2Prefab, Vector2.one, Quaternion.identity) as GameObject;
            player2Death = player2.GetComponent<PlayerDeath>();

            player2Camera = Instantiate (player2CameraPrefab, new Vector2(20f, 5f), Quaternion.identity) as Camera;
            SmoothFollow player2Follow = player2Camera.GetComponent<SmoothFollow>();
            player2Follow.setTarget(player2.transform);
        }

        level.buildLevel(true);

        resetPlayerPosition(player1);
        resetPlayerPosition(player2);

        resetGameScore();

        powerupManager.shouldSpawnPowerups = true;
    }

    private void cleanupGame()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) 
        {
            if (o.transform.root.name.Contains(CONSTANT_OBJECTS_NAME))
            {
                continue;
            }

            Destroy(o);
        }
    }

    public void increasePlayerScore(Player curPlayer, int scorePoints)
    {
        int curScorePlayer1 = int.Parse(player1Score.text);
        int curScorePlayer2 = int.Parse(player2Score.text);

        int updatedScorePlayer1 = curScorePlayer1;
        int updatedScorePlayer2 = curScorePlayer2;

        if (curPlayer == Player.ONE)
        {
            updatedScorePlayer1 += scorePoints;
        }else
        {
            updatedScorePlayer2 += scorePoints;
        }

        player1Score.fontStyle = FontStyle.Normal;
        player2Score.fontStyle = FontStyle.Normal;

        if (updatedScorePlayer1 > updatedScorePlayer2)
        {
            player1Score.color = Color.yellow; 
            player2Score.color = Color.white;
            player1Score.fontStyle = FontStyle.Bold;
        }else if (updatedScorePlayer1 < updatedScorePlayer2)
        {
            player1Score.color = Color.white; 
            player2Score.color = Color.yellow;  
            player2Score.fontStyle = FontStyle.Bold;
        }else
        {
            player1Score.color = Color.white; 
            player2Score.color = Color.white;
        }

        player1Score.text = (updatedScorePlayer1).ToString();
        player2Score.text = (updatedScorePlayer2).ToString();    
    }

    private void RestartGame()
    {
        StopAllCoroutines();

        GameObject[] remainsObjects = GameObject.FindGameObjectsWithTag("Remains");
        for (int i = 0; i < remainsObjects.Length; i++)
        {
            Destroy(remainsObjects[i]);
        }

        BeginGame();
    }

    private void resetPlayerPosition(GameObject player)
    {
        if (player.CompareTag("Player1"))
        {
            player.transform.position = player1Base.transform.position;
        }else if (player.CompareTag("Player2"))
        {
            player.transform.position = player2Base.transform.position;
        }
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.SetActive(true);
    }

    private void resetGameScore()
    {
        player1Score.fontStyle = FontStyle.Normal;
        player2Score.fontStyle = FontStyle.Normal;

        player1Score.color = Color.white; 
        player2Score.color = Color.white;

        player1Score.text = "0";
        player2Score.text = "0";
    }

    IEnumerator RespawnPlayer(GameObject player)
    {
        yield return new WaitForSeconds(respawnTime);

        resetPlayerPosition(player);

        PlayerDeath playerDeath = player.GetComponent<PlayerDeath>();

        playerDeath.IsRespawning = false;

        player.GetComponent<PlayerController>().resetStats();
    }
}