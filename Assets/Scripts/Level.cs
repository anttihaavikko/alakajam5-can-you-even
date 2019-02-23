using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Letter[] letters;
    private float checkDelay = 5f;
    private LevelSelector levelSelector;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(LevelSelector ls)
    {
        levelSelector = ls;
        Invoke("CheckLetters", checkDelay);
        Cursor.visible = false;
    }

    void CheckLetters()
    {
        bool allGood = true;
        for(int i = 1; i < letters.Length; i++)
        {
            if (letters[i].transform.position.x <= letters[i - 1].transform.position.x)
            {
                allGood = false;
            }
        }

        if(!allGood || FollowMouse.Instance.holding)
        {
            Invoke("CheckLetters", checkDelay);
        }
        else
        {
            Debug.Log("All good!");
            levelSelector.ChangeLevel();
        }
    }
}
