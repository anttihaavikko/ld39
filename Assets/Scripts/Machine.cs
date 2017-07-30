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

	private Vector3 areaSize;

	// Use this for initialization
	void Start () {

		screen.transform.localScale = new Vector3 (transform.localScale.x, 1, 1);

		if (defaultLetter) {
			SpawnLetter (defaultLetter, false);
		}

		loadingSprite = screen.sprite;

		if (area) {
			areaSize = area.localScale;
		}
	}

	void Update() {
		if (area) {
			area.localScale = Vector3.MoveTowards(area.localScale, areaSize, 0.05f);
		}
	}

	public bool SpawnLetter(GameObject letter) {
		return SpawnLetter (letter, true);
	}

	public bool SpawnLetter(GameObject letter, bool doSound) {
		if(area) {

			area.localScale *= 1.075f;

			correct = (letter.name == targetLetter);

			if (doSound) {

				Camera.main.GetComponent<RoomCamera>().Shake (0.05f, 0.05f);

				AudioManager.Instance.PlayEffectAt (14, area.position, 0.5f);

				if (correct) {

					Camera.main.GetComponent<RoomCamera>().Chromate (0.07f);

					AudioManager.Instance.PlayEffectAt (8, area.position, 0.5f);
					EffectManager.Instance.AddEffect (5, transform.position + Vector3.up * 2.5f);
				} else {
					AudioManager.Instance.PlayEffectAt (7, area.position, 0.5f);
//					EffectManager.Instance.AddEffect (4, area.position);
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

			return true;
		}

		return false;
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
