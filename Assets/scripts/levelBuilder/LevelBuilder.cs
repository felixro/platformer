using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LevelBuilder : MonoBehaviour 
{
    public MouseManager mouseManager;
    public KeyboardManager keyboardManager;
    public Level level;

    public Rigidbody2D flag;
    public Rigidbody2D basePlayer1;
    public Rigidbody2D basePlayer2;
    public Rigidbody2D enemy;

    private float colliderRadius = 1f;
    private bool isMainMenuShown = false;

    private bool destroyTerrain = true;
    private bool moveFlag = false;
    private bool placeEnemy = false;

	void Start () 
    {
        level.buildLevel(true);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(keyboardManager.mainMenuKey))
        {
            isMainMenuShown = !isMainMenuShown;

            return;
        }

        if (destroyTerrain)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hit = Physics2D.CircleCastAll(worldPoint, colliderRadius, Vector2.zero);

            mouseManager.setCircleSize(colliderRadius);

            for (int i = 0; i < hit.Length; i++)
            {
                RaycastHit2D curHit = hit[i];
                if ( curHit.collider != null 
                        && curHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")
                        && Input.GetMouseButton(0) )
                {
                    curHit.collider.gameObject.SetActive(false);
                }    
            }
        }else if (moveFlag)
        {
            mouseManager.setCircleSize(0.1f);
        }else if (placeEnemy)
        {
            
        }
	}
        
    public void DestroyTerrain()
    {
        destroyTerrain = true;
        moveFlag = false;
        placeEnemy = false;
    }

    public void MoveFlag()
    {
        destroyTerrain = false;
        moveFlag = true;
        placeEnemy = false;
    }

    public void PlaceEnemy()
    {
        destroyTerrain = false;
        moveFlag = false;
        placeEnemy = true;
    }

    public void SetRadiusSize(float size)
    {
        colliderRadius = size;
    }

    public void SaveLevel()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(Application.persistentDataPath + Level.STORED_LEVEL_FILENAME);

        StoredLevel storedLevel = 
            new StoredLevel(
                level.GetBitmap(), 
                flag.transform.localPosition,
                basePlayer1.transform.localPosition,
                basePlayer2.transform.localPosition,
                enemy.transform.localPosition
            );

        bf.Serialize(fs, storedLevel);
        fs.Close();
    }

    public void ResetLevel()
    {
        level.buildLevel(false);
        flag.transform.position = new Vector3(10f, 10f, 0f);
        basePlayer1.transform.position = new Vector3(15f, 10f, 0f);
        basePlayer2.transform.position = new Vector3(22f, 10f, 0f);
        enemy.transform.position = new Vector3(29f, 10f, 0f);
    }
}

[Serializable]
class StoredLevel
{
    public bool[,,] _bitmap;
    public Position _flagPosition;
    public Position _basePlayer1Position;
    public Position _basePlayer2Position;
    public Position _enemyPosition;

    public StoredLevel(
        bool[,,] bitmap, 
        Vector3 flagPosition,
        Vector3 basePlayer1Position,
        Vector3 basePlayer2Position,
        Vector3 enemyPosition
    )
    {
        _bitmap = bitmap;
        _flagPosition = new Position(flagPosition.x, flagPosition.y);
        _basePlayer1Position = new Position(basePlayer1Position.x, basePlayer1Position.y);
        _basePlayer2Position = new Position(basePlayer2Position.x, basePlayer2Position.y);
        _enemyPosition = new Position(enemyPosition.x, enemyPosition.y);
    }
}

[Serializable]
class Position
{
    public float _x, _y;

    public Position(float x, float y)
    {
        _x = x;
        _y = y;
    }
}