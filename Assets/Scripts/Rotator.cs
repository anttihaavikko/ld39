using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	public float speed = 1f;
	private float angle = 0f;

	private Rigidbody2D body;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		angle += speed;
//		transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, angle));
		body.MoveRotation(angle);

	}
}
