using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public Sprite cursorSprite, handSprite, grabSprite;
    public SpriteRenderer sprite;
    public bool hovering, holding;

    private Camera cam;

    private static FollowMouse instance = null;
    public static FollowMouse Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;    
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mp = Input.mousePosition;
        mp.z = 10f;
        transform.position = cam.ScreenToWorldPoint(mp);

        if(hovering || holding)
        {
            sprite.sprite = holding ? grabSprite : handSprite;
        }
        else
        {
            sprite.sprite = cursorSprite;
        }
    }
}
