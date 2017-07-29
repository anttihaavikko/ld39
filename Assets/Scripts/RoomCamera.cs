using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamera : MonoBehaviour {

	public Vector3 target;
	static Camera backgroundCam;

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
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, target, 2f);
	}
}
