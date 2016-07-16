using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{
    private Vector3 localPosition;
    private Vector3 localScale;

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        int movementDirection = playerController.getMovementDirection();

        if (movementDirection == 1)
        {
            localPosition = new Vector3(5.7f, -0.7f, 0f);
            localScale = new Vector3(5f, 5f, 1f);
        }else if (movementDirection == -1)
        {
            localPosition = new Vector3(-5.7f, -0.7f, 0f);
            localScale = new Vector3(-5f, 5f, 1f);
        }

        transform.localPosition = localPosition;
        transform.localScale = localScale;
    }

    public Vector3 GetBulletExitPosition()
    {
        return transform.GetChild(0).position;
    }
}
