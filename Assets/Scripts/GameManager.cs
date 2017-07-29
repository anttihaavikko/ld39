using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public GameObject deathBallPrefab;

	private List<GameObject> balls;

	public Machine[] endMachines;
	public Sprite[] machineNumbers;

	public Dimmer dimmer;

	private bool hasEnded = false;

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

	void Update() {
		if (hasEnded && Input.anyKeyDown) {
			SceneManager.LoadSceneAsync ("Start");
		}
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

	public void CheckForEnd() {

		for (int i = 0; i < endMachines.Length; i++) {
			if (!endMachines [i].correct) {
				return;
			}
		}
			
		ShowEnd ("THE END");
	}

	public void ShowEnd(string text) {
		dimmer.SetText (text);
		dimmer.FadeIn (1f);

		Invoke ("EnableEnd", 1.5f);
	}

	void EnableEnd() {
		hasEnded = true;
	}
}
