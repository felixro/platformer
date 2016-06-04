using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour 
{
    private Tile[] neighbours = new Tile[8];

    public void setNeighbour(TileDirection direction, Tile neighbour)
    {
        neighbours[(int)direction] = neighbour;
    }

    void Destroy()
    {
        this.gameObject.SetActive(false);
    }

    public void destroyNeighbours()
    {
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i] != null)
            {
                neighbours[i].gameObject.SetActive(false);
            }
        }
    }
}
