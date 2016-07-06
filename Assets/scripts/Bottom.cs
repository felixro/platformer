using UnityEngine;
using System.Collections;

public class Bottom : MonoBehaviour 
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerDeath playerDeath = other.GetComponentInParent<PlayerDeath>();
            if (playerDeath != null)
            {
                playerDeath.die();
            }
        }
        else if (other.gameObject.CompareTag("Flag"))
        {
            FlagHandler flagHandler = other.gameObject.GetComponent<FlagHandler>();
            flagHandler.resetPosition();
        }
        else
        {
            // anything else gets destroyed
            Destroy(other.gameObject);
        }
    }
}
