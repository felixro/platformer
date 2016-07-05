using UnityEngine;
using System.Collections;

public class BaseHandler : MonoBehaviour
{
    public GameManager _gameManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            FlagHandler[] flagHandler = other.gameObject.GetComponentsInChildren<FlagHandler>();
            if (flagHandler == null || flagHandler.Length < 1)
            {
                return;
            }

            Player player;
            if (other.gameObject.CompareTag("Player1"))
            {
                player = Player.ONE;
            }else if (other.gameObject.CompareTag("Player2"))
            {
                player = Player.TWO;
            }else
            {
                throw new System.ArgumentException("Unknown player tag: " + other.gameObject.tag);    
            }

            if (name == "Player1 Base" && player == Player.ONE
                || name == "Player2 Base" && player == Player.TWO
            )
            {
                flagHandler[0].resetPosition();
                _gameManager.increasePlayerScore(player, 5);
            }
        }
    }
}
