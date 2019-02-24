using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public Level level;
    private bool canUse = true;
    private Vector3 theSize;
    public QuitButton quitButton;

    // Start is called before the first frame update
    void Awake()
    {
        theSize = transform.localScale;
    }

    private void OnMouseUp()
    {
        if(canUse)
        {
            level.EndLevelAfter();
            canUse = false;

            Tweener.Instance.ScaleTo(transform, new Vector3(2f, 0f, 1f), 0.5f, 0f, TweenEasings.QuarticEaseOut);
            Tweener.Instance.ScaleTo(quitButton.transform, new Vector3(2f, 0f, 1f), 0.5f, 0.35f, TweenEasings.QuarticEaseOut);
            AudioManager.Instance.PlayEffectAt(6, transform.position, 1f);
            Invoke("QuitSound", 0.35f);
        }
    }

    void QuitSound()
    {
        AudioManager.Instance.PlayEffectAt(6, quitButton.transform.position, 1f);
    }

    private void OnMouseEnter()
    {
        if (canUse)
        {
            Tweener.Instance.ScaleTo(transform, theSize * 1.1f, 0.5f, 0f, TweenEasings.BounceEaseOut);
            AudioManager.Instance.PlayEffectAt(4, transform.position, 1.25f);
        }
    }

    private void OnMouseExit()
    {
        if (canUse)
        {
            Tweener.Instance.ScaleTo(transform, theSize, 0.5f, 0f, TweenEasings.BounceEaseOut);
            AudioManager.Instance.PlayEffectAt(4, transform.position, 1.25f);
        }
    }
}
