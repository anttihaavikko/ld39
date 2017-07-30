using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartView : MonoBehaviour {

	void Start() {
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}

		if (Input.anyKeyDown) {
			AudioManager.Instance.melodyVolume = 0.5f;
			SceneManager.LoadSceneAsync ("Main");
		}
	}
}
