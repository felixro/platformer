using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour 
{
    public PowerUpType type;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController playerController = other.GetComponentInParent<PlayerController>();

            playerController.performPowerUpAction(
                type
            );

            Destroy(gameObject);
        }
    }
}
