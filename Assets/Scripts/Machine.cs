using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour {

	public Transform area;
	public GameObject defaultLetter;

	public SpriteRenderer screen;
	public Sprite readyScreen;
	public Sprite screenToShow;
	private Sprite loadingSprite;

	public float showDelay = 1f;

	public bool isCheckpoint = true;

	// Use this for initialization
	void Start () {

		screen.transform.localScale = new Vector3 (transform.localScale.x, 1, 1);

		if (defaultLetter) {
			SpawnLetter (defaultLetter);
		}

		loadingSprite = screen.sprite;
	}

	public void SpawnLetter(GameObject letter) {
		if(area) {
			Debug.Log ("Spawning " + letter.name);

			// destroy previous
			foreach(Transform child in area) {
				Destroy(child.gameObject);
			}

			GameObject l = Instantiate (letter, area);
			l.transform.localScale = Vector3.one;
			l.transform.localPosition = Vector3.zero;
		}
	}

	public void Activate() {

		screen.sprite = readyScreen;

		if (screenToShow) {
			Invoke ("ChangeScreen", showDelay);
		}
	}

	public void Deactivate() {
		screen.sprite = loadingSprite;
	}

	public void ChangeScreen() {
		screen.sprite = screenToShow;
	}
}
