using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	public float speed = 1f;
	public float offset = 0f;
	public Vector3 direction = Vector3.zero;

	private Rigidbody2D body;
	private Vector3 originalPosition;

	// Use this for initialization
	void Start () {
		originalPosition = transform.localPosition;
		body = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
//		transform.localPosition = originalPosition + direction * Mathf.Sin (Time.time * speed + offset * Mathf.PI);
		body.MovePosition (originalPosition + direction * Mathf.Sin (Time.time * speed + offset * Mathf.PI));
	}
}
