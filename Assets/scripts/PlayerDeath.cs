using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour 
{
    public GameObject deadPlayerPrefab;

    public float splatterForce = 5.0f;

    public void die()
    {
        this.gameObject.SetActive(false);

        GameObject deathPrefab = Instantiate (deadPlayerPrefab, transform.position, Quaternion.identity) as GameObject;
        Transform deathTransform = deathPrefab.transform;

        deathTransform.SetParent(this.transform);
        deathTransform.localPosition = new Vector3(0f,0f,1f);
        deathTransform.SetParent(null);

        DeadPiece[] pieces = deathPrefab.GetComponentsInChildren<DeadPiece>();
        Debug.Log(pieces.Length);

        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].explode();
        }
    }
}
