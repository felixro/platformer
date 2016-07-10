using UnityEngine;
using System.Collections;

public class WeaponHandler : MonoBehaviour {

    public GameObject _bombPrefab;
    public GameObject _laserPrefab;

    public WeaponType _weaponType;

    public void Fire(Vector2 force, int movementDirection)
    {
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
    }
}
