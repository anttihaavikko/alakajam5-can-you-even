using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	public float speed = 1f;
	private float angle = 0f;
    private float offset;

    private void Start()
    {
        offset = Random.Range(0f, 360f);
    }

    // Update is called once per frame
    void Update () {
		angle += speed * Time.deltaTime * 60f;
		transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, angle + offset));
	}

    public void ChangeSpeed(float s) {
        speed = s;
    }
}
