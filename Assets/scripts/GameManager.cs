using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
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
    }

    private void BeginGame()
    {
        level.buildLevel();

        player.transform.position = new Vector2(2f, 1f);
        player.SetActive(true);

        spawnEnemy();
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        BeginGame();
    }

    private void spawnEnemy()
    {
        enemy.transform.position = new Vector2(Random.Range(1f,30f), 1f);
        enemy.SetActive(true);
    }

    IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(respawnTime);

        spawnEnemy();
        isRespawning = false;
    }
}