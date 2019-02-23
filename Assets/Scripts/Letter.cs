using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{
    public HingeJoint2D joint;

    private Camera cam;
    private Rigidbody2D body;
    private Vector3 previousPoint = Vector3.zero;
    private Vector2 dir;
    private float mass;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public bool touchingBad = false;

    private Level level;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        body = GetComponent<Rigidbody2D>();
        mass = body.mass;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        level = GetComponentInParent<Level>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mp = Input.mousePosition;
        mp.z = 10f;
        Vector3 mouseInWorld = cam.ScreenToWorldPoint(mp);

        dir = mouseInWorld - previousPoint;
        previousPoint = mouseInWorld;
    }

    private void OnMouseDown()
    {
        Vector3 mp = Input.mousePosition;
        mp.z = 10f;
        Vector3 mouseInWorld = cam.ScreenToWorldPoint(mp);

        EffectManager.Instance.AddEffect(1, mouseInWorld);
        EffectManager.Instance.AddEffect(2, mouseInWorld);

        joint.anchor = transform.InverseTransformPoint(mouseInWorld);
        joint.enabled = true;
        body.mass = mass * 2f;
        FollowMouse.Instance.holding = true;

        Cursor.visible = false;

        touchingBad = false;
    }

    private void OnMouseDrag()
    {
    }

    private void OnMouseUp()
    {
        body.mass = mass;
        joint.enabled = false;
        body.AddForce(dir * 100f, ForceMode2D.Impulse);
        FollowMouse.Instance.holding = false;
    }

    private void OnMouseEnter()
    {
        FollowMouse.Instance.hovering = true;
    }

    private void OnMouseExit()
    {
        FollowMouse.Instance.hovering = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bad")
        {
            touchingBad = true;
        }

        if (collision.relativeVelocity.magnitude > 3f)
        {
            EffectManager.Instance.AddEffect(2, collision.contacts[0].point);
        }

        if (collision.relativeVelocity.magnitude > 5f)
        {
            EffectManager.Instance.BaseEffect(0.1f * collision.relativeVelocity.magnitude);
        }

        if (collision.relativeVelocity.magnitude > 10f)
        {
            Explode(collision.relativeVelocity.magnitude);
        }
    }

    private void Explode(float magnitude)
    {
        EffectManager.Instance.BaseEffect(0.1f * magnitude);
        EffectManager.Instance.AddEffect(0, transform.position);
        EffectManager.Instance.AddEffect(2, transform.position);
        EffectManager.Instance.AddEffect(3, transform.position);
        EffectManager.Instance.AddEffect(4, transform.position);
        EffectManager.Instance.AddEffect(5, transform.position);
        Invoke("Respawn", 4f);
        Invoke("MarkSpawn", 3.5f);
        gameObject.SetActive(false);
        OnMouseUp();

        level.ResetCheck();
    }

    void MarkSpawn()
    {
        EffectManager.Instance.AddEffect(1, originalPosition);
    }

    private void Respawn()
    {
        Vector3 targetScale = transform.localScale;
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        EffectManager.Instance.AddEffect(0, transform.position);
        Tweener.Instance.ScaleTo(transform, targetScale, 0.4f, 0f, TweenEasings.BounceEaseOut);
    }
}
