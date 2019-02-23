using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public bool spelling = true;
    public bool noTouch = false;

    public float checkDelay = 5f;
    public Letter[] letters;

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
        Cursor.visible = false;

        if(spelling)
            Invoke("CheckLetters", checkDelay);

        if (noTouch)
            Invoke("CheckTouch", checkDelay);
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

        DoCheck(allGood, "CheckLetters");
    }

    void DoCheck(bool ok, string method)
    {
        if (!ok || FollowMouse.Instance.holding)
        {
            Invoke(method, checkDelay);
        }
        else
        {
            levelSelector.ChangeLevel();
        }
    }

    void CheckTouch()
    {
        bool allGood = true;
        for (int i = 0; i < letters.Length; i++)
        {
            if (letters[i].touchingBad)
            {
                allGood = false;
                Debug.Log(letters[i] + " is touching");
            }
        }

        DoCheck(allGood, "CheckTouch");
    }
}
