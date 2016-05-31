using UnityEngine;
using System.Collections;

public class DeadPiece : MonoBehaviour 
{
    public void explode()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * 50f, ForceMode2D.Force);   
    }
}
