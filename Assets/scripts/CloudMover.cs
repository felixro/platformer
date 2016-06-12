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

    public float cloudSpeedMin = 0.05f;
    public float cloudSpeedMax = 0.5f;

    [Range(1, 20)]
    public int cloudiness = 1;

    private GameObject[] clouds;

    void Start()
    {
        clouds = new GameObject[cloudPrefabs.Length * cloudiness];

        for (int i = 0; i < cloudPrefabs.Length; i++)
        {
            for (int j = 0; j < cloudiness; j++) 
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

                cloud.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(cloudSpeedMin, cloudSpeedMax), 0f);
                float scaleFactor = Random.Range(1.0f, 5.0f);
                cloud.transform.localScale = new Vector2(scaleFactor, scaleFactor);

                clouds[i * j + j] = cloud;                
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < clouds.Length; i++)
        {
            if (clouds[i])
            {
                Vector3 curPosition = clouds[i].transform.position;
                if (curPosition.x > endPositionX)
                {
                    clouds[i].transform.position = new Vector3(startPositionX, curPosition.y);
                }
            }
        }
    }
}
