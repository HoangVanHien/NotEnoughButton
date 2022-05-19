using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnter : Collidable
{
    private bool ableToEnter;
    private bool ableToPlay = false;
    public string levelName;
    public string prepLevelName;
    public Sprite unplayable;
    public int levelPointNeeded = 0;
    public int heartPointNeeded = 0;
    public int goldPointNeeded = 0;
    public bool prevLevelWonNeeded = true;


    protected override void Start()
    {
        base.Start();
        if (!GameManager.instance.LevelIsExist(levelName))//is there a scene with this name
        {
            ableToPlay = false;
        }
        else
        {
            ableToPlay = GameManager.instance.LevelIsPlayable(levelName);//check whether this level has been opened or not
            //first check
            if (!ableToPlay) OpenThisLevelCheck();//check if this level has completed all required request to be opened

        }
        //recheck
        if (ableToPlay)
        {
            int levelPointCount = GameManager.instance.GetLevelPoint(levelName);
            for (int i = 0; i < levelPointCount; i++)
            {
                if (i == 3) break;
                transform.GetChild(i).gameObject.SetActive(true);
            }
            ableToEnter = (GameObject.Find("Player").transform.position != transform.position);
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().sprite = unplayable;
            enabled = false;
            return;
        }
    }

    protected override void Update()
    {
        //player stand on this
        if (!ableToEnter)
        {
            ableToEnter = (GameObject.Find("Player").transform.position != transform.position);
            return;
        }
        base.Update();
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (ableToEnter && coll.transform.parent.name == "Player")
        {
            if (coll.transform.parent.GetComponentInParent<MoveControll>().MoveAble())
            {
                GameManager.instance.SaveLevel();
                GameManager.instance.LoadLevel(levelName);
            }
        }
    }

    private void OpenThisLevelCheck()
    {
        ableToPlay = true;
        if (GameManager.instance.GetLevelPointTotal() < levelPointNeeded)// have enough level point
        {
            ableToPlay = false;
            return;
        }
        if (GameManager.instance.GetHeartPointTotal() < heartPointNeeded)// have enough heart point
        {
            ableToPlay = false;
            return;
        }
        if (GameManager.instance.GetGoldPointTotal() < goldPointNeeded)// have enough gold point
        {
            ableToPlay = false;
            return;
        }
        if (prevLevelWonNeeded && !GameManager.instance.LevelIsWon(prepLevelName))//havent win the required level
        {
            ableToPlay = false;
            return;
        }
        GameManager.instance.NewLevelAdd(levelName);
    }
}
