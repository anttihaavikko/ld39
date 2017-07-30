using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject deathBallPrefab;

	private List<GameObject> balls;

	public Machine[] endMachines;
	public Sprite[] machineNumbers;

	public Dimmer dimmer;
	public Image colorizer;

	private bool hasEnded = false;

	public float colorHue, colorValue = 0f;

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

//		colorizer.color = Color.HSVToRGB (colorHue, 0.2f, colorValue);
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

		AudioManager.Instance.melodyVolume = 0f;

		Invoke ("EnableEnd", 1.5f);
	}

	void EnableEnd() {
		hasEnded = true;
	}
}
