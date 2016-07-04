using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour 
{
    void Destroy()
    {
        this.gameObject.SetActive(false);
    }
}
