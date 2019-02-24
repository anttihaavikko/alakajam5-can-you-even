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
    private bool holding = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    public bool touchingBad = false;

    private Face face;

    private Level level;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        body = GetComponent<Rigidbody2D>();
        mass = body.mass;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;
        level = GetComponentInParent<Level>();
        face = GetComponentInChildren<Face>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mp = Input.mousePosition;
        mp.z = 10f;
        Vector3 mouseInWorld = cam.ScreenToWorldPoint(mp);

        dir = mouseInWorld - previousPoint;
        previousPoint = mouseInWorld;

        if(touchingBad)
            face.Emote(Face.Emotion.Sad);

        var hits = Physics2D.BoxCastAll(transform.position, new Vector2(0.1f, 0.3f), 0f, Vector2.zero);

        if (hits.Length > 1)
            Explode(2f);

    }

    private void OnMouseDown()
    {
        holding = true;
        Vector3 mp = Input.mousePosition;
        mp.z = 10f;
        Vector3 mouseInWorld = cam.ScreenToWorldPoint(mp);

        EffectManager.Instance.AddEffect(1, mouseInWorld);
        EffectManager.Instance.AddEffect(2, mouseInWorld);

        AudioManager.Instance.PlayEffectAt(17, transform.position, 2.5f);
        AudioManager.Instance.PlayEffectAt(14, transform.position, 1f);

        joint.anchor = transform.InverseTransformPoint(mouseInWorld);
        joint.enabled = true;
        body.mass = mass * 2f;
        FollowMouse.Instance.holding = true;

        Cursor.visible = false;

        touchingBad = false;

        face.Emote(Face.Emotion.Sneaky);
    }

    private void OnMouseDrag()
    {
    }

    private void OnMouseUp()
    {
        holding = false;
        body.mass = mass;
        joint.enabled = false;
        body.AddForce(dir * 100f, ForceMode2D.Impulse);
        FollowMouse.Instance.holding = false;
        AudioManager.Instance.PlayEffectAt(17, transform.position, 2.5f);
        AudioManager.Instance.PlayEffectAt(15, transform.position, 1f);
        face.Emote(Face.Emotion.Happy);
    }

    private void OnMouseEnter()
    {
        FollowMouse.Instance.hovering = true;
        AudioManager.Instance.PlayEffectAt(4, transform.position, 1f);
    }

    private void OnMouseExit()
    {
        FollowMouse.Instance.hovering = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bad" && !holding)
        {
            touchingBad = true;
        }

        if(collision.relativeVelocity.magnitude > 0.5f)
        {
            AudioManager.Instance.PlayEffectAt(17, collision.contacts[0].point, Mathf.Min(collision.relativeVelocity.magnitude, 4f));
            AudioManager.Instance.PlayEffectAt(16, collision.contacts[0].point, 0.5f * Mathf.Min(collision.relativeVelocity.magnitude, 4f));

            face.Emote(Face.Emotion.Shocked, Face.Emotion.Default, 0.75f);
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
        holding = false;

        AudioManager.Instance.PlayEffectAt(2, transform.position, 1.5f);
        AudioManager.Instance.PlayEffectAt(3, transform.position, 1.5f);
        AudioManager.Instance.PlayEffectAt(21, transform.position, 1.5f);
        AudioManager.Instance.PlayEffectAt(7, transform.position, 1.5f);

        EffectManager.Instance.BaseEffect(0.2f * magnitude);

        EffectManager.Instance.AddEffect(0, transform.position);
        EffectManager.Instance.AddEffect(2, transform.position);
        EffectManager.Instance.AddEffect(3, transform.position);
        EffectManager.Instance.AddEffect(4, transform.position);
        EffectManager.Instance.AddEffect(5, transform.position);

        float diff = Random.Range(-0.5f, 0.5f);

        Invoke("Respawn", 4f + diff);
        Invoke("MarkSpawn", 3.5f + diff);
        gameObject.SetActive(false);
        OnMouseUp();

        level.ResetCheck();

        AudioManager.Instance.targetPitch = 0.8f;
        Invoke("ResetPitch", 1f);
    }

    void ResetPitch()
    {
        AudioManager.Instance.targetPitch = 1f;
    }

    void MarkSpawn()
    {
        EffectManager.Instance.AddEffect(1, originalPosition);
        AudioManager.Instance.PlayEffectAt(10, transform.position, 1f);
    }

    private void Respawn()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        transform.localScale = originalScale;
        EffectManager.Instance.AddEffect(0, transform.position);
        AudioManager.Instance.PlayEffectAt(14, transform.position, 1f);
        AudioManager.Instance.PlayEffectAt(20, transform.position, 0.7f);
        Tweener.Instance.ScaleTo(transform, originalScale, 0.4f, 0f, TweenEasings.BounceEaseOut);
        Invoke("Brag", 0.5f);
    }

    void Brag()
    {
        face.Emote(Face.Emotion.Brag);
    }
}
