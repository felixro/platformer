using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour 
{
    public int segments = 100;

    private Vector3 mousePosition;
    private LineRenderer line;
    private float _size = 5f;

    void Start ()
    {
        line = GetComponent<LineRenderer>();

        line.SetVertexCount (segments + 1);
    }

	void Update () 
    {
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        drawCircleAroundMousePosition();
	}

    public void setCircleSize(float size)
    {
        _size = size;
    }

    private void drawCircleAroundMousePosition()
    {
        DrawCircle(mousePosition.x, mousePosition.y, _size);
    }

    private void DrawCircle(float xa, float ya, float radius) 
    {
        float x,y;
        float angle = 0f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * radius + xa;
            y = Mathf.Cos (Mathf.Deg2Rad * angle) * radius + ya;

            line.SetPosition (i, new Vector3(x,y,0f));

            angle += (360f / segments);
        }
    }
}
