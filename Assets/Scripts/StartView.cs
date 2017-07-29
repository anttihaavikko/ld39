using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartView : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) {
			AudioManager.Instance.melodyVolume = 0.5f;
			SceneManager.LoadSceneAsync ("Main");
		}
	}
}
