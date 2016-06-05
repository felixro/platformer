using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public float gameFieldBoundary = -50f;
    public float respawnTime = 2f;
    public KeyboardManager keyboardManager;

    public Camera cameraPrefab;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Level level;

    private GameObject player;
    private GameObject enemy;
    private Camera camera;

    private bool isRespawning = false;

    void Start ()
    {
        player = Instantiate (playerPrefab, new Vector2(2f, 2f), Quaternion.identity) as GameObject;
        enemy = Instantiate (enemyPrefab, new Vector2(20f, 5f), Quaternion.identity) as GameObject;

        camera = Instantiate (cameraPrefab, new Vector2(20f, 5f), Quaternion.identity) as Camera;

        SmoothFollow smoothFollow = camera.GetComponent<SmoothFollow>();
        smoothFollow.setTarget(player.transform);

        BeginGame();
    }

    void Update ()
    {
        if (Input.GetKeyDown(keyboardManager.restartGameKey))
        {
            RestartGame();
        }

        if (!enemy.activeSelf && !isRespawning)
        {
            StartCoroutine(RespawnEnemy());
            isRespawning = true;
        }

        if (player.transform.position.y <= gameFieldBoundary)
        {
            resetPlayerPosition();
        }

        if (enemy.transform.position.y <= gameFieldBoundary)
        {
            resetEnemyPosition();
        }
    }

    private void BeginGame()
    {
        level.buildLevel();

        resetPlayerPosition();
        player.SetActive(true);

        resetEnemyPosition();
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        BeginGame();
    }

    private void resetPlayerPosition()
    {
        player.transform.position = new Vector2(2f, 1f);        
    }

    private void resetEnemyPosition()
    {
        enemy.transform.position = new Vector2(Random.Range(1f,30f), 1f);
        enemy.SetActive(true);
    }

    IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(respawnTime);

        resetEnemyPosition();
        isRespawning = false;
    }
}