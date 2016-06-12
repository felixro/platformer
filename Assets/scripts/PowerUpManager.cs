using UnityEngine;
using System.Collections;

public class PowerUpManager : MonoBehaviour
{
    public Powerup[] powerupPrefabs;

    [Range(1,20)]
    public float jumpUp = 10f;

    [Range(1,20)]
    public float powerupTime = 5f;

    [Range(20,100)]
    public float speedUp = 20f;

    public float powerUpSpawnLeft = 1.0f;
    public float powerUpSpawnRight = 80.0f;
    public float powerUpSpawnHeight = 10.0f;

    public int maxPowerUps = 5;

    void FixedUpdate()
    {
        if (GameObject.FindGameObjectsWithTag ("PowerUp").Length < maxPowerUps)
        {
            spawnPowerUp();
        }
    }

    private void spawnPowerUp()
    {
        Instantiate (
            powerupPrefabs[Random.Range(0, powerupPrefabs.Length)], 
            new Vector2(
                Random.Range(powerUpSpawnLeft, powerUpSpawnRight), 
                powerUpSpawnHeight
            ), 
            Quaternion.identity
        );
    }
}
