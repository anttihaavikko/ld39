using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParts : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float force = 1.5f;
		Rigidbody2D[] bodies = GetComponentsInChildren<Rigidbody2D> ();

		for (int i = 0; i < bodies.Length; i++) {
			bodies [i].AddForce (new Vector2 (Random.Range (-force, force), Random.Range (-force, force)), ForceMode2D.Impulse);
			bodies [i].AddTorque (Random.Range(-7f, 7f));
		}
	}
}
