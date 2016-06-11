using UnityEngine;
using System.Collections;

public class CloudMover : MonoBehaviour
{
    public GameObject[] cloudPrefabs;

    public float startPositionX = -30f;
    public float endPositionX = 30f;

    public float cloudHeightMax = 7f;
    public float cloudHeightMin = 4f;

    public float cloudStartPosMin = 0f;
    public float cloudStartPosMax = 30f;

    private GameObject[] clouds;

    void Start()
    {
        clouds = new GameObject[cloudPrefabs.Length];

        for (int i = 0; i < cloudPrefabs.Length; i++)
        {
            GameObject cloud = 
                Instantiate(
                    cloudPrefabs[i], 
                    new Vector3(
                        Random.Range(
                            cloudStartPosMin,
                            cloudStartPosMax
                        ),
                        Random.Range(
                            cloudHeightMin,
                            cloudHeightMax
                        ), 
                        2f
                    ), 
                    Quaternion.identity
                ) as GameObject;

            cloud.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(0.01f, 1f), 0f);

            Debug.Log(cloud);
            clouds[i] = cloud;
        }
    }

    void Update()
    {
        /*
        for (int i = 0; i < clouds.Length; i++)
        {
            Debug.Log(clouds[i]);
            Vector3 curPosition = clouds[i].transform.position;
            if (curPosition.x > endPositionX)
            {
                clouds[i].transform.position = new Vector3(startPositionX, curPosition.y);
            }
        }
        */
    }
}
