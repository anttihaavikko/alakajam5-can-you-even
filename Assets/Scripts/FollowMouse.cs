using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Camera cam;

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
    }
}
