using UnityEngine;
using System.Collections;
using System;

public class WeaponHandler : MonoBehaviour {

    public GameObject _bombPrefab;
    public GameObject _laserPrefab;

    public WeaponType _weaponType;

    public float laserShotFrequency = 2f;
    public float bombShotFrequency = 1f;

    private bool _canFire = true;

    public void ResetWeapon()
    {
        _canFire = true;
    }

    public void Fire(Vector2 force, int movementDirection)
    {
        if (!_canFire)
        {
            return;
        }

        if (_weaponType == WeaponType.BOMB)
        {
            FireBomb(force);
        }
        else if (_weaponType == WeaponType.LASER)
        {
            FireLaser(movementDirection);
        }
    }

    public void setWeaponType(WeaponType type)
    {
        _weaponType = type;
    }

    private void FireBomb(Vector2 force)
    {
        GameObject bombObject = Instantiate (_bombPrefab, transform.position, Quaternion.identity) as GameObject;

        Bomb bomb = bombObject.GetComponent<Bomb>();

        bomb.setOwner(gameObject);

        Rigidbody2D body = bombObject.GetComponent<Rigidbody2D>();
        body.AddForce(
            force,
            ForceMode2D.Force
        );

        StartCoroutine(Fire(bombShotFrequency));
    }

    private void FireLaser(int movementDirection)
    {
        Vector3 bulletExitPosition = GetComponentInChildren<Weapon>().GetBulletExitPosition();

        GameObject laserObject = Instantiate (_laserPrefab, bulletExitPosition, Quaternion.identity) as GameObject;

        Laser laser = laserObject.GetComponent<Laser>();

        laser.setOwner(gameObject);

        Rigidbody2D body = laserObject.GetComponent<Rigidbody2D>();

        if (movementDirection == 0)
        {
            movementDirection = 1;
        }

        body.AddForce(
            new Vector2(
                laser.force * movementDirection, 
                0f
            ),
            ForceMode2D.Force
        );  

        laser.PlaySound();

        StartCoroutine(Fire(laserShotFrequency));
    }

    public void SwitchWeapon()
    {
        int nrOfWeapons = Enum.GetValues(typeof(WeaponType)).Length;
        int curWeaponType = (int)_weaponType;
        int nextWeaponType = (curWeaponType + 1) % nrOfWeapons;

        _weaponType = (WeaponType)nextWeaponType;
    }

    IEnumerator Fire(float fireFrequency)
    {
        _canFire = false;
        yield return new WaitForSeconds(1f/fireFrequency);
        _canFire = true;
    }
}
