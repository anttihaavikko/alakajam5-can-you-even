using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudHolder : MonoBehaviour
{
    public Transform[] clouds;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var c in clouds)
        {
            var x = Random.Range(-5f, 5f);
            var y = Random.Range(-1.5f, 3f);
            c.localPosition = new Vector3(x, y, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
