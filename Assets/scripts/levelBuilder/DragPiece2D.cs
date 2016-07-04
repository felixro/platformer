using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class DragPiece2D : MonoBehaviour 
{
    private Vector3 screenPoint;
    private Vector3 offset;

    void Update()
    {
        transform.localRotation = Quaternion.identity;
    }

    void OnMouseDown() 
    {
        Vector3 position = 
            Camera.main.ScreenToWorldPoint(
                new Vector3(
                    Input.mousePosition.x, 
                    Input.mousePosition.y, 
                    screenPoint.z
                )
            );

        offset = gameObject.transform.position - position;
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }
}