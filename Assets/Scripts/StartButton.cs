using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public Level level;
    private bool canUse = true;
    private Vector3 theSize;
    public QuitButton quitButton;

    public Transform[] logoParts;

    // Start is called before the first frame update
    void Awake()
    {
        theSize = transform.localScale;

        if (Application.platform == RuntimePlatform.WebGLPlayer)
            transform.position = new Vector3(transform.position.x, -1.64f, 0f);
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

            for(int i = 0; i < logoParts.Length; i++)
            {
                Tweener.Instance.ScaleTo(logoParts[3 - i], new Vector3(2f, 0f, 1f), 0.5f, 0.35f * 2 + i * 0.3f, TweenEasings.QuarticEaseOut);
                AudioManager.Instance.PlayEffectAtAfterDelay(6, logoParts[3 - i].position, 1f, 0.35f * 2 + i * 0.3f);
            }
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
