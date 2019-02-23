﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class EffectCamera : MonoBehaviour {

	public Material transitionMaterial;

	private float cutoff = 1f, targetCutoff = 1f;
	private float prevCutoff = 1f;
	private float cutoffPos = 0f;
	private float transitionTime = 0.5f;

	private PostProcessingBehaviour filters;
	private float chromaAmount = 0f;
	private float chromaSpeed = 0.1f;

	private float shakeAmount = 0f, shakeTime = 0f;

	private Vector3 originalPos;

    private float zoomDir = -0.2f;

    private Camera cam;

	void Start() {
		filters = GetComponent<PostProcessingBehaviour>();
        originalPos = transform.localPosition;
//		Invoke ("StartFade", 0.5f);

        cam = Camera.main;
	}

	void Update() {
		cutoffPos += Time.fixedDeltaTime / transitionTime;
		cutoffPos = (cutoffPos > 1f) ? 1f : cutoffPos;
		cutoff = Mathf.Lerp (prevCutoff, targetCutoff, cutoffPos);
//		transitionMaterial.SetFloat ("_Cutoff", cutoff);

		// chromatic aberration update
		if (filters) {
			chromaAmount = Mathf.MoveTowards (chromaAmount, 0, Time.deltaTime * 0.5f);
			ChromaticAberrationModel.Settings g = filters.profile.chromaticAberration.settings;
			g.intensity = chromaAmount;
			filters.profile.chromaticAberration.settings = g;
		}

		if (shakeTime > 0f) {
			shakeTime -= Time.deltaTime;
            transform.localPosition = originalPos + new Vector3 (Random.Range (-shakeAmount, shakeAmount), Random.Range (-shakeAmount, shakeAmount), 0);
		} else {
            transform.localPosition = originalPos;
		}

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + zoomDir, 5, 10);
	}

	void StartFade() {
		Fade (false, 0.5f);
	}

//	void OnRenderImage(RenderTexture src, RenderTexture dst) {
//		if (transitionMaterial) {
//			Graphics.Blit (src, dst, transitionMaterial);
//		}
//	}

	public void Fade(bool show, float delay) {
		targetCutoff = show ? 1.1f : -0.1f;
		prevCutoff = show ? -0.1f : 1.1f;
		cutoffPos = 0f;
		transitionTime = delay;

		// AudioManager.Instance.PlayEffectAt (12, Vector3.zero, 0.2f);
	}

	public void Chromate(float amount, float speed) {
		chromaAmount = amount;
		chromaSpeed = speed;
	}

	public void Shake(float amount, float time) {
		shakeAmount = amount;
		shakeTime = time;
	}

	public void BaseEffect(float mod = 1f) {
		Shake (0.04f * mod, 0.075f * mod);
		Chromate (0.5f * mod, 0.1f * mod);
	}

    public void DoZoom()
    {
        zoomDir = -zoomDir;
        Invoke("EndZoom", 1f);
    }

    void EndZoom()
    {
        zoomDir = -zoomDir;
    }
}
