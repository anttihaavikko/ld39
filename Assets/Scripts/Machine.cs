using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour {

	public Transform area;
	public GameObject defaultLetter;

	public SpriteRenderer screen;
	public Sprite screenToShow;
	public float showDelay = 1f;

	// Use this for initialization
	void Start () {

		screen.transform.localScale = new Vector3 (transform.localScale.x, 1, 1);

		if (defaultLetter) {
			SpawnLetter (defaultLetter);
		}
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
		if (screenToShow) {
			Invoke ("ChangeScreen", showDelay);
		}
	}

	public void ChangeScreen() {
		screen.sprite = screenToShow;
	}
}
