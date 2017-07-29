using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour {

	private float timer;
	private float dir;
	private float size;

	public float speed = 1f;
	public bool canBlink = true;

	// Use this for initialization
	void Start () {
		ResetTimer ();
		size = transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.timeScale < 0.1f) {
			return;
		}

		timer += dir * speed;

		if (timer >= 0) {
			transform.localScale = new Vector2 (transform.localScale.x, size - size * timer / 10f);
		}

		if (timer <= 0 && dir < 0) {
			ResetTimer ();
		}

		if (timer >= 10) {
			dir = -1f;
		}
	}

	void ResetTimer() {
		timer = Random.Range (-50, -200);
		dir = 1f;
	}
}
