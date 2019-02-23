using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public Level level;
    private bool canUse = true;
    private Vector3 theSize;

    // Start is called before the first frame update
    void Awake()
    {
        theSize = transform.localScale;

        if (Application.platform == RuntimePlatform.WebGLPlayer)
            gameObject.SetActive(false);
    }

    private void OnMouseUp()
    {
        if (canUse)
        {
            Application.Quit();
        }
    }

    private void OnMouseEnter()
    {
        if (canUse)
            Tweener.Instance.ScaleTo(transform, theSize * 1.1f, 0.5f, 0f, TweenEasings.BounceEaseOut);
    }

    private void OnMouseExit()
    {
        if (canUse)
            Tweener.Instance.ScaleTo(transform, theSize, 0.5f, 0f, TweenEasings.BounceEaseOut);
    }

}
