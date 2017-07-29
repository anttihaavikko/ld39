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
	public string targetLetter;
	public bool correct = false;

	public int respawns = 10;

	public bool beeping = false;

	// Use this for initialization
	void Start () {

		screen.transform.localScale = new Vector3 (transform.localScale.x, 1, 1);

		if (defaultLetter) {
			SpawnLetter (defaultLetter, false);
		}

		loadingSprite = screen.sprite;
	}

	public void SpawnLetter(GameObject letter) {
		SpawnLetter (letter, true);
	}

	public void SpawnLetter(GameObject letter, bool doSound) {
		if(area) {

			correct = (letter.name == targetLetter);

			if (doSound) {
				if (correct) {
					AudioManager.Instance.PlayEffectAt (8, transform.position, 0.5f);
				} else {
					AudioManager.Instance.PlayEffectAt (7, transform.position, 0.5f);
				}
			}

			if (correct) {
				GameManager.Instance.CheckForEnd ();
			}

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

		if (respawns < 10) {
			return;
		}

		if (!beeping) {
			beeping = true;
			AudioManager.Instance.PlayEffectAt(2, transform.position, 0.25f);
			Invoke ("EnableBeep", 2.5f);
		}

		screen.sprite = readyScreen;
	}

	void EnableBeep() {
		beeping = false;
	}

	public void Deactivate() {

		if (respawns < 10) {
			return;
		}

		screen.sprite = loadingSprite;
	}

	public void ChangeScreen() {
		AudioManager.Instance.PlayEffectAt (9, transform.position, 0.25f);
		screen.sprite = screenToShow;
	}

	public void ShowNumber() {
		AudioManager.Instance.PlayEffectAt (9, transform.position, 0.25f);
		screen.sprite = GameManager.Instance.machineNumbers [respawns];
		Debug.Log (respawns + " spawns left");
	}
}
