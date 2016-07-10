using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour 
{
    public float explosionDelayInSec = 10f;
    public float force = 2000f;
    public int destructionStrength = 3;

    private float destroyDelay = 0.5f;
    private GameObject _owner;

    private int curDestroyed = 0;

    public void setOwner(GameObject owner)
    {
        _owner = owner;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        checkCollision(other);
    }

    void Start()
    {
        StartCoroutine(ExecuteAfterTime(explosionDelayInSec));
    }
        
	void Update () 
    {
        transform.rotation = Quaternion.identity;
	}

    public void PlaySound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
    }

    private void checkCollision(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (! other.gameObject.name.Equals(_owner.name))
            {
                PlayerDeath playerDeath = other.GetComponentInParent<PlayerDeath>();
                if (playerDeath != null)
                {
                    playerDeath.die();
                }
            }
        }
            
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (curDestroyed >= destructionStrength)
            {
                Explode();
                return;
            }

            other.gameObject.SetActive(false);
            curDestroyed++;
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        Explode();
    }

    private void Explode()
    {
        Destroy(this.gameObject, destroyDelay);
    }
}