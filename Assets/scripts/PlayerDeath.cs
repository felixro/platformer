﻿using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour 
{
    public GameObject deadPlayerPrefab;

    public bool IsRespawning
    {
        get
        {
            return isRespawning;
        }

        set
        {
            isRespawning = value;
        }
    }

    public float splatterForce = 5.0f;

    private bool isRespawning = false;

    public void die()
    {
        // if the player held the flag, drop it
        FlagHandler flagHandler = GetComponentInChildren<FlagHandler>();

        if (flagHandler != null)
        {
            flagHandler.dropFlag();
        }

        gameObject.SetActive(false);
        isRespawning = false;

        GetComponent<WeaponHandler>().ResetWeapon();

        GameObject deathPrefab = Instantiate (deadPlayerPrefab, transform.position, Quaternion.identity) as GameObject;
        Transform deathTransform = deathPrefab.transform;

        deathTransform.SetParent(this.transform);
        deathTransform.localPosition = new Vector3(0f,0f,1f);
        deathTransform.SetParent(null);

        DeadPiece[] pieces = deathPrefab.GetComponentsInChildren<DeadPiece>();
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].explode();
        }
    }
}
