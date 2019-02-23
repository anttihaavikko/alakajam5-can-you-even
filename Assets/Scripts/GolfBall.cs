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
        }

        if(collision.tag == "GolfReset")
        {
            Invoke("Reset", 2f);
        }
    }

    private void Reset()
    {
        EffectManager.Instance.AddEffect(1, transform.position);
        EffectManager.Instance.AddEffect(0, originalPos);
        transform.position = originalPos;
        body.velocity = Vector2.zero;
        body.angularVelocity = 0f;
    }
}
