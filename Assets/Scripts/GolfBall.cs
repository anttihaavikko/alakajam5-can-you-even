using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBall : MonoBehaviour
{
    private Vector3 originalPos;
    public Level level;
    public Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "GolfGoal")
        {
            level.EndLevelAfter();
            AudioManager.Instance.PlayEffectAt(15, transform.position, 5f);
        }

        if(collision.tag == "GolfReset")
        {
            Invoke("Reset", 2f);
        }
    }

    private void Reset()
    {
        AudioManager.Instance.PlayEffectAt(17, transform.position, 5f);
        AudioManager.Instance.PlayEffectAt(14, transform.position, 1f);
        AudioManager.Instance.PlayEffectAt(15, transform.position, 2f);
        AudioManager.Instance.PlayEffectAt(19, originalPos, 0.5f);
        AudioManager.Instance.PlayEffectAt(17, originalPos, 5f);
        AudioManager.Instance.PlayEffectAt(0, originalPos, 1f);
        EffectManager.Instance.AddEffect(1, transform.position);
        EffectManager.Instance.AddEffect(0, originalPos);
        transform.position = originalPos;
        body.velocity = Vector2.zero;
        body.angularVelocity = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.relativeVelocity.magnitude > 0.5f)
        {
            AudioManager.Instance.PlayEffectAt(17, collision.contacts[0].point, Mathf.Min(collision.relativeVelocity.magnitude, 4f));
            AudioManager.Instance.PlayEffectAt(16, collision.contacts[0].point, 0.5f * Mathf.Min(collision.relativeVelocity.magnitude, 4f));
        }

        if (collision.relativeVelocity.magnitude > 3f)
        {
            EffectManager.Instance.AddEffect(2, collision.contacts[0].point);
        }
    }
}
