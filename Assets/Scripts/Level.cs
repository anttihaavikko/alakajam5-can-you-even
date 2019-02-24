using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public bool spelling = true;
    public bool noTouch = false;
    public bool survival = false;

    public float checkDelay = 5f;
    public Letter[] letters;

    private LevelSelector levelSelector;
    private string methodName = null;


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

        Invoke("EndHighpass", 1f);
        AudioManager.Instance.targetPitch = 1f;

        if (spelling)
            methodName = "CheckLetters";

        if (noTouch)
            methodName = "CheckTouch";

        if (survival)
            methodName = "CheckAlive";

        if (methodName != null)
            Invoke(methodName, checkDelay);
    }

    void EndHighpass()
    {
        AudioManager.Instance.Highpass(false);
    }

    public void ResetCheck()
    {
        if(methodName != null)
        {
            CancelInvoke(methodName);
            Invoke(methodName, checkDelay);
        }
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

        DoCheck(allGood, methodName);
    }

    void DoCheck(bool ok, string method)
    {
        if (!ok || FollowMouse.Instance.holding)
        {
            Invoke(method, checkDelay);
        }
        else
        {
            EndLevel();
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
            }
        }

        DoCheck(allGood, methodName);
    }

    void CheckAlive()
    {
        DoCheck(true, methodName);
    }

    public void EndLevelAfter()
    {
        Invoke("EndLevel", 2f);
    }

    void EndLevel()
    {
        AudioManager.Instance.PlayEffectAt(18, transform.position, 4f);
        AudioManager.Instance.PlayEffectAt(20, transform.position, 1.5f);
        Invoke("DoEnd", 1f);

        AudioManager.Instance.Highpass(true);
        AudioManager.Instance.targetPitch = 1.2f;
    }

    void DoEnd()
    {
        levelSelector.ChangeLevel();
    }
}
