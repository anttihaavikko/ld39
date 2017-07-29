using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject deathBallPrefab;

	private List<GameObject> balls;

	private static GameManager instance = null;
	public static GameManager Instance {
		get { return instance; }
	}

	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}

		balls = new List<GameObject> ();
	}

	public void ClearDeathBalls() {
		for (int i = 0; i < balls.Count; i++) {
			Destroy (balls [i]);
		}

		balls.Clear ();
	}

	public void AddDeathBall(Vector3 pos, Vector3 dir) {

		GameObject ball = Instantiate (deathBallPrefab, pos, Quaternion.identity);
		Rigidbody2D ballBody = ball.GetComponent<Rigidbody2D> ();
		ballBody.AddForce (dir, ForceMode2D.Impulse);
		balls.Add (ball);
	}
}
