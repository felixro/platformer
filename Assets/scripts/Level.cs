using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Level : MonoBehaviour 
{
    public GameObject tilePrefab;

    private Tile[,] tiles;

    public int width = 10;
    public int height = 3;

    private float offset = 0.5f;

    public static string STORED_LEVEL_FILENAME = "levelData.dat";

    public void buildLevel()
    {
        if (File.Exists(Application.persistentDataPath + STORED_LEVEL_FILENAME))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + STORED_LEVEL_FILENAME, FileMode.Open);

            StoredLevel storedLevel = (StoredLevel) bf.Deserialize(fs);

            fs.Close();

            buildLevel(storedLevel._bitmap);
        }else
        {
            bool[,] bitmap = new bool[width,height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++) 
                {
                    bitmap[i,j] = true;
                }
            }

            buildLevel(bitmap);
        }


    }

    public void buildLevel(bool[,] bitmap)
    {
        tiles = new Tile[width, height];

        for (int i = 0; i< width; i++)
        {
            for (int j = 0; j < height; j++) 
            {
                GameObject g = Instantiate (tilePrefab, new Vector2(i * offset, -j * offset), Quaternion.identity) as GameObject;

                tiles[i,j] = g.GetComponent<Tile>();
                tiles[i,j].gameObject.SetActive(bitmap[i,j]);
            }
        }
    }

    public bool[,] GetBitmap()
    {
        bool[,] bitmap = new bool[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++) 
            {
                bitmap[i,j] = tiles[i,j].isActiveAndEnabled;
            }
        }

        return bitmap;
    }

    private void setNeighbours()
    {
        for (int i = 0; i< width; i++)
        {
            for (int j = 0; j < height; j++) 
            {
                Tile tile = tiles[i,j];

                if (i > 0)
                {
                    tile.setNeighbour(TileDirection.W, tiles[i-1,j]);

                    if (j > 0)
                    {
                        tile.setNeighbour(TileDirection.NW, tiles[i-1,j-1]);
                    }

                    if (j < height - 1)
                    {
                        tile.setNeighbour(TileDirection.SW, tiles[i-1,j+1]);
                    }
                }

                if (i < width - 1)
                {
                    tile.setNeighbour(TileDirection.E, tiles[i+1,j]);

                    if (j < height - 1)
                    {
                        tile.setNeighbour(TileDirection.SE, tiles[i+1,j+1]);
                    }
                }

                if (j > 0)
                {
                    tile.setNeighbour(TileDirection.N, tiles[i,j-1]);

                    if (i < width - 1)
                    {
                        tile.setNeighbour(TileDirection.NE, tiles[i+1,j-1]);
                    }
                }

                if (j < height - 1)
                {
                    tile.setNeighbour(TileDirection.S, tiles[i,j+1]);
                }
            }
        }
    }
}
