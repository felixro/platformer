using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LevelBuilder : MonoBehaviour 
{
    public KeyboardManager keyboardManager;
    public Level level;

    private float colliderRadius = 1f;
    private bool isMainMenuShown = false;

	void Start () 
    {
        level.buildLevel();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(keyboardManager.mainMenuKey))
        {
            isMainMenuShown = !isMainMenuShown;

            return;
        }
        
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hit = Physics2D.CircleCastAll(worldPoint, colliderRadius, Vector2.zero);

        for (int i = 0; i < hit.Length; i++)
        {
            RaycastHit2D curHit = hit[i];
            if ( curHit.collider != null && Input.GetMouseButton(0) )
            {
                curHit.collider.gameObject.SetActive(false);
            }    
        }
	}

    public void SetRadiusSize(float size)
    {
        colliderRadius = size;
    }

    public void SaveLevel()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(Application.persistentDataPath + Level.STORED_LEVEL_FILENAME);

        StoredLevel storedLevel = new StoredLevel(level.GetBitmap());

        bf.Serialize(fs, storedLevel);
        fs.Close();
    }
}

[Serializable]
class StoredLevel
{
    public bool[,] _bitmap;

    public StoredLevel(bool[,] bitmap)
    {
        _bitmap = bitmap;
    }
}
