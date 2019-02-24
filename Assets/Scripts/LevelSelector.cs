using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {

	private int current;
    private GameObject prev;
    public Transform camHolder;


	// Use this for initialization
	void Start () {

		int idx = 0;

		// find currently active level
		foreach(Transform child in transform) {
			if (child.gameObject.activeSelf) {
				current = idx;
                break;
			}

			idx++;
		}

		ActivateLevel (current);
	}

	public void ActivateLevel(int level, bool delayed = false) {

		if (level == -1) {
			return;
		}

        prev = transform.GetChild(current).gameObject;

		// deactivate current level
        if(delayed) {
            Invoke("DeactivatePrevious", 2f);   
        } else {
            DeactivatePrevious();
        }

		current = level;

        // activate next level
        var next = transform.GetChild(current);
		next.gameObject.SetActive (true);

        var p = new Vector3(next.position.x, next.position.y, -10f);
        Tweener.Instance.MoveTo(camHolder, p, 0.7f, 0f, TweenEasings.QuadraticEaseInOut);

        Level l = next.GetComponent<Level>();
        l.Activate(this);
	}

    void DeactivatePrevious() {
        //Manager.Instance.ClearDemons();

        if(prev) 
            prev.SetActive(false);
    }

	void Update() {
        if(Application.isEditor) {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                ChangeLevel();
            }

            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                ChangeLevel(-1);
            }   
        }

        if (Input.GetKeyUp(KeyCode.Escape) && Application.platform != RuntimePlatform.WebGLPlayer)
            Application.Quit();
	}

	public void ChangeLevel(int dir = 1) {
        Camera.main.GetComponent<EffectCamera>().DoZoom();
        Invoke(dir == 1 ? "NextLevel" : "PreviousLevel", 0.5f);
    }

    private void NextLevel()
    {
        ActivateLevel(current + 1, true);
    }

    private void PreviousLevel()
    {
        ActivateLevel(current - 1, true);
    }
}
