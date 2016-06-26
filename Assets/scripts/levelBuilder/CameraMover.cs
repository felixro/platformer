using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour 
{
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode downKey = KeyCode.DownArrow;

    public float speed = 10f;

    public float zoomSpeed = 10f;
    public float minOrthographicSize = 1.0f;
    public float maxOrthographicSize = 40.0f;

    private float orthographicSize;

    void Start() 
    {
        orthographicSize = Camera.main.orthographicSize;
    }

	void Update () 
    {
        if (Input.GetKey(leftKey))
        {
            Camera.main.transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (Input.GetKey(rightKey))
        {
            Camera.main.transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if (Input.GetKey(upKey))
        {
            Camera.main.transform.position += Vector3.up * speed * Time.deltaTime;
        }

        if (Input.GetKey(downKey))
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
        }

        float scroll = Input.GetAxis ("Mouse ScrollWheel");
        if (scroll != 0.0f) 
        {
            orthographicSize -= scroll * zoomSpeed * Time.deltaTime;
            orthographicSize = Mathf.Clamp (orthographicSize, minOrthographicSize, maxOrthographicSize);

            Camera.main.orthographicSize = orthographicSize;
        }

        //Camera.main.orthographicSize = orthographicSize * Time.deltaTime;//Mathf.MoveTowards (Camera.main.orthographicSize, orthographicSize, smoothSpeed * Time.deltaTime);
	}
}
