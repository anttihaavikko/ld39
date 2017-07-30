using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class StartCamera : MonoBehaviour {

	private float shakeAmount = 0;
	private float shakeTime = 0;
	private Vector3 directionalShake = Vector3.zero;

	private VignetteAndChromaticAberration chroma;

	// Use this for initialization
	void Start () {
		chroma = GetComponent<VignetteAndChromaticAberration> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, Vector3.zero, 2f);

		directionalShake = Vector3.MoveTowards (directionalShake, Vector3.zero, Time.deltaTime);

		if (shakeTime > 0) {
			shakeTime -= Time.deltaTime;
			transform.position += new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);
		}

		transform.position += directionalShake;

		chroma.chromaticAberration = Mathf.MoveTowards (chroma.chromaticAberration, 0, 1f);
	}

	public void Shake(float amount, float duration) {
		shakeAmount = amount;
		shakeTime = duration;
	}

	public void Shake(Vector3 dir) {
		directionalShake = dir;
	}

	public void Chromate(float amount) {
		chroma.chromaticAberration = Screen.width / 7 * amount;
	}
}
