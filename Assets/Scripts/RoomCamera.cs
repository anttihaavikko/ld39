using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class RoomCamera : MonoBehaviour {

	public Vector3 target;
	static Camera backgroundCam;

	private float shakeAmount = 0;
	private float shakeTime = 0;
	private Vector3 directionalShake = Vector3.zero;

	private VignetteAndChromaticAberration chroma;

	// Use this for initialization
	void Start () {
		target = transform.position;

		float currentAspectRatio = (float)Screen.width / Screen.height;

		float inset = 1.0f - 1/currentAspectRatio;
		Camera.main.rect = new Rect(inset/2, 0.0f, 1.0f-inset, 1.0f);

		if (!backgroundCam) {
			// Make a new camera behind the normal camera which displays black; otherwise the unused space is undefined
			backgroundCam = new GameObject("BackgroundCam", typeof(Camera)).GetComponent<Camera>();
			backgroundCam.depth = int.MinValue;
			backgroundCam.clearFlags = CameraClearFlags.SolidColor;
			backgroundCam.backgroundColor = Color.black;
			backgroundCam.cullingMask = 0;
		}

		chroma = GetComponent<VignetteAndChromaticAberration> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, target, 2f);

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
