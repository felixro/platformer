using UnityEngine;
using System.Collections;

public class FlagHandler : MonoBehaviour 
{
    private GameObject flagHolder = null;

    private Vector3 localPosition;
    private Quaternion localRotation;

    private Vector3 defaultPosition;

    void Update()
    {
        if (flagHolder != null)
        {
            int movementDirection = flagHolder.GetComponent<PlayerController>().getMovementDirection();

            if (movementDirection == 1)
            {
                localPosition = new Vector3(-5f, 2f, 2f);
                localRotation = Quaternion.Euler(0f, 180f, -45f);
            }else if (movementDirection == -1)
            {
                localPosition = new Vector3(5f, 2f, 2f);
                localRotation = Quaternion.Euler(0f, 0f, -45f);
            }
        }

        if (localPosition != null && localRotation != null && flagHolder != null)
        {
            transform.parent = flagHolder.transform;
            transform.localPosition = localPosition;
            transform.localRotation = localRotation;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            flagHolder = other.gameObject;
        }
    }

    public void setDefaultPosition(Vector2 position)
    {
        defaultPosition = new Vector3(position.x, position.y, 2f);
        transform.localPosition = position;
    }

    public void dropFlag()
    {
        flagHolder = null;
        transform.parent = null;        
    }

    public void resetPosition()
    {
        flagHolder = null;
        transform.parent = null;

        transform.position = defaultPosition;
        transform.rotation = Quaternion.identity;
    }
}
