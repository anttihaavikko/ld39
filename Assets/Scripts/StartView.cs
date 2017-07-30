using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartView : MonoBehaviour {

	private StartCamera cam;
	private bool starting = false;
	private AsyncOperation loading;

	public Transform startText, logoImage;
	public Vector3 startSize, logoSize;

	void Start() {
		Cursor.visible = false;
		cam = Camera.main.GetComponent<StartCamera> ();
		startSize = Vector3.one;
		logoSize = Vector3.one;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}

		if (Input.anyKeyDown && !starting) {

			starting = true;

			AudioManager.Instance.PlayEffectAt (8, Vector3.zero);

			cam.Shake (0.1f, 0.1f);
			cam.Chromate (0.2f);

			AudioManager.Instance.melodyVolume = 0.5f;
			loading = SceneManager.LoadSceneAsync ("Main");
			loading.allowSceneActivation = false;
			Invoke ("EnableStart", 1f);

			startSize = new Vector3 (2f, 0f, 1f);
			logoSize = new Vector3 (1.2f, 1.2f, 1f);
		}

		startText.localScale = Vector3.MoveTowards (startText.localScale, startSize, 0.1f);
		logoImage.localScale = Vector3.MoveTowards (logoImage.localScale, logoSize, 0.01f);
	}

	void EnableStart() {
		loading.allowSceneActivation = true;
	}
}
