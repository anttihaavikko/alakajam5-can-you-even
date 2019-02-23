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

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        body = GetComponent<Rigidbody2D>();
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

        joint.anchor = transform.InverseTransformPoint(mouseInWorld);
        joint.enabled = true;
    }

    private void OnMouseDrag()
    {
    }

    private void OnMouseUp()
    {
        joint.enabled = false;
        body.AddForce(dir * 100f, ForceMode2D.Impulse);
    }
}
