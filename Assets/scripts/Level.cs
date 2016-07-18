using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Level : MonoBehaviour 
{
    public GameObject tileLeftPrefab;
    public GameObject tileRightPrefab;
    public GameObject tileTopPrefab;
    public GameObject tileBottomPrefab;

    public GameObject redFlag;
    public GameObject player1Base;
    public GameObject player2Base;
    public GameObject enemy;

    private Tile[,,] tiles;

    public int width = 10;
    public int height = 3;

    private float offset = 1f;

    public static string STORED_LEVEL_FILENAME = "level.v3.dat";

    public void buildLevel(bool loadFromFile)
    {
        if (loadFromFile && File.Exists(Application.persistentDataPath + STORED_LEVEL_FILENAME))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + STORED_LEVEL_FILENAME, FileMode.Open);

            StoredLevel storedLevel = (StoredLevel) bf.Deserialize(fs);

            FlagHandler flagHandler = redFlag.GetComponent<FlagHandler>();

            flagHandler.setDefaultPosition(
                new Vector2(
                    storedLevel._flagPosition._x, 
                    storedLevel._flagPosition._y
                )
            );
                    
            player1Base.transform.localPosition = 
                new Vector2(
                    storedLevel._basePlayer1Position._x, 
                    storedLevel._basePlayer1Position._y
                );

            player2Base.transform.localPosition = 
                new Vector2(
                    storedLevel._basePlayer2Position._x, 
                    storedLevel._basePlayer2Position._y
                );

            enemy.transform.localPosition = 
                new Vector2(
                    storedLevel._enemyPosition._x, 
                    storedLevel._enemyPosition._y
                );

            fs.Close();

            buildLevel(storedLevel._bitmap);
        }else
        {
            bool[,,] bitmap = new bool[width,height,4];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++) 
                {
                    for (int k = 0; k < 4; k++) 
                    {
                        bitmap[i,j,k] = true;
                    }
                }
            }

            buildLevel(bitmap);
        }
    }

    public void buildLevel(bool[,,] bitmap)
    {
        cleanTiles();

        tiles = new Tile[width, height, 4];

        for (int i = 0; i< width; i++)
        {
            for (int j = 0; j < height; j++) 
            {
                GameObject leftTile = Instantiate (tileLeftPrefab, new Vector2(i * offset, -j * offset), Quaternion.identity) as GameObject;
                GameObject rightTile = Instantiate (tileRightPrefab, new Vector2(i * offset, -j * offset), Quaternion.identity) as GameObject;
                GameObject topTile = Instantiate (tileTopPrefab, new Vector2(i * offset, -j * offset), Quaternion.identity) as GameObject;
                GameObject bottomTile = Instantiate (tileBottomPrefab, new Vector2(i * offset, -j * offset), Quaternion.identity) as GameObject;

                tiles[i,j,0] = leftTile.GetComponent<Tile>();
                tiles[i,j,1] = rightTile.GetComponent<Tile>();
                tiles[i,j,2] = topTile.GetComponent<Tile>();
                tiles[i,j,3] = bottomTile.GetComponent<Tile>();

                for (int k = 0; k < 4; k++) 
                {
                    tiles[i,j,k].gameObject.SetActive(bitmap[i,j,k]);
                }
            }
        }
    }

    public bool[,,] GetBitmap()
    {
        bool[,,] bitmap = new bool[width, height, 4];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++) 
            {
                for (int k = 0; k < 4; k++) 
                {
                    bitmap[i, j, k] = tiles[i, j, k].isActiveAndEnabled;    
                }
            }
        }

        return bitmap;
    }

    private void cleanTiles()
    {
        if (tiles == null)
        {
            return;
        }

        for (int i = 0; i< width; i++)
        {
            for (int j = 0; j < height; j++) 
            {
                for (int k = 0; k < 4; k++) 
                {
                    Destroy(tiles[i,j,k].gameObject);
                }
            }
        }
    }
}
