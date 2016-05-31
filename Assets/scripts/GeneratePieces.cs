using UnityEngine;
using System.Collections;

public class GeneratePieces : MonoBehaviour 
{
    public GameObject piecePrefab;

	// Use this for initialization
	void Awake () 
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++) 
            {
                GameObject piece = Instantiate (piecePrefab, transform.position, Quaternion.identity) as GameObject;
                Transform pieceTransform = piece.transform;

                pieceTransform.SetParent(this.transform);
                pieceTransform.localPosition = new Vector3(i * 0.2f, j * 0.2f, 1f);
            }
        }
	}
}
