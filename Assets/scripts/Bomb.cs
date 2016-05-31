using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour 
{
    public int maxTouchCount = 5;

    private Tile lastTileTouched;
    private bool isExploding = false;
    private float destroyDelay = 0.5f;

    void OnTriggerEnter2D(Collider2D other)
    {
        checkPlayerCollision(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        checkPlayerCollision(other);
    }

	void Update () 
    {
        transform.rotation = Quaternion.identity;
        if (maxTouchCount == 0)
        {
            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.isTrigger = true;
            circleCollider.radius = 5f;

            ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < particleSystems.Length; i++)
            {
                if (particleSystems[i].name == "Explosion")
                {
                    particleSystems[i].Play();
                    isExploding = true;

                    break;
                }
            }
        }

        if (maxTouchCount < 0)
        {
            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.radius = 0.5f;
            circleCollider.isTrigger = false;
            Destroy(this.gameObject, destroyDelay);
        }
	}

    private void checkPlayerCollision(Collider2D other)
    {
        if (isExploding && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerDeath playerDeath = other.GetComponentInParent<PlayerDeath>();
            if (playerDeath != null)
            {
                playerDeath.die();
            }
        }
    }

    public void touchedGround(Tile touchedTile)
    {
        maxTouchCount--;
        lastTileTouched = touchedTile;
    }
}
