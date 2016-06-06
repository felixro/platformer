using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public float gameFieldBoundary = -50f;
    public float respawnTime = 2f;
    public KeyboardManager keyboardManager;

    public Camera player1CameraPrefab;
    public Camera player2CameraPrefab;

    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public Level level;

    private GameObject player1;
    private PlayerDeath player1Death;

    private GameObject player2;
    private PlayerDeath player2Death;

    private Camera player1Camera;
    private Camera player2Camera;

    private float leftSpawnPoint = 1f;
    private float rightSpawnPoint = 30f;

    void Start ()
    {
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

        BeginGame();
    }

    void Update ()
    {
        if (Input.GetKeyDown(keyboardManager.restartGameKey))
        {
            RestartGame();
        }
            
        if (!player1.activeSelf && !player1Death.IsRespawning)
        {
            StartCoroutine(RespawnPlayer(player1));
            player1Death.IsRespawning = true;
        }

        if (!player2.activeSelf && !player2Death.IsRespawning)
        {
            StartCoroutine(RespawnPlayer(player2));
            player2Death.IsRespawning = true;
        }

        if (player1.transform.position.y <= gameFieldBoundary)
        {
            resetPlayerPosition(player1);
        }

        if (player2.transform.position.y <= gameFieldBoundary)
        {
            resetPlayerPosition(player2);
        }
    }

    private void BeginGame()
    {
        level.buildLevel();

        resetPlayerPosition(player1);
        resetPlayerPosition(player2);
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
        player.transform.position = new Vector2(Random.Range(leftSpawnPoint,rightSpawnPoint), 1f);  
        player.SetActive(true);
    }

    IEnumerator RespawnPlayer(GameObject player)
    {
        yield return new WaitForSeconds(respawnTime);

        player.transform.position = new Vector2(Random.Range(leftSpawnPoint,rightSpawnPoint), 1f);
        player.SetActive(true);

        PlayerDeath playerDeath = player.GetComponent<PlayerDeath>();

        playerDeath.IsRespawning = false;
    }
}