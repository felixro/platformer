using UnityEngine;
using System.Collections;

public class AttackPlayers : MonoBehaviour 
{
    public string[] tagsToAttack;
    public float distanceOfAttack = 15f;
    public float shotFrequency = 1f;
    public float shotSpeed = 1500f;

    public GameObject _shotPrefab;

    private bool _canFire = true;

    public void Update()
    {
        if (!_canFire)
        {
            return;
        }

        for (int i = 0; i < tagsToAttack.Length; i++)
        {
            GameObject target = GameObject.FindWithTag(tagsToAttack[i]);

            if (target != null && target.activeSelf)
            {
                float distance = Vector3.Distance (transform.position, target.transform.position);

                if (distance <= distanceOfAttack)
                {
                    FireShot(target.transform);
                }
            }
        }
    }

    private void FireShot(Transform target)
    {
        Vector3 relativePosition = target.position - transform.position;
        relativePosition.Normalize();

        GameObject projectile = (GameObject)Instantiate(_shotPrefab, transform.position, Quaternion.identity);
        Rigidbody2D body = projectile.GetComponent<Rigidbody2D>();

        Laser laser = projectile.GetComponent<Laser>();
        laser.setOwner(gameObject);

        body.AddForce(
            relativePosition * shotSpeed,
            ForceMode2D.Force
        );

        float angle = Mathf.Atan2(relativePosition.y,relativePosition.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        StartCoroutine(Fire(shotFrequency));

        laser.PlaySound();
    }

    IEnumerator Fire(float fireFrequency)
    {
        _canFire = false;
        yield return new WaitForSeconds(1f/fireFrequency);
        _canFire = true;
    }
}
