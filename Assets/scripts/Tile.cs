using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour 
{
    private Tile[] neighbours = new Tile[8];

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            GetComponent<BoxCollider2D>().isTrigger = false;

            Invoke("Destroy", 0.05f);

            other.GetComponent<Bomb>().touchedGround(this);
        }else if (other.gameObject.layer == LayerMask.NameToLayer("DeadPlayer"))
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

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
