using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour 
{
    public float explosionDelayInSec = 2f;
    public float explosionRadius = 5f;
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

    void Start()
    {
        StartCoroutine(ExecuteAfterTime(explosionDelayInSec));
    }
        
	void Update () 
    {
        transform.rotation = Quaternion.identity;
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
            
        if (isExploding && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            other.gameObject.SetActive(false);
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
        circleCollider.radius = explosionRadius;

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

        Destroy(this.gameObject, destroyDelay);
    }
}
