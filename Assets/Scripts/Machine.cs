using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour {

	public Transform area;
	public GameObject defaultLetter;

	// Use this for initialization
	void Start () {
		if (defaultLetter) {
			SpawnLetter (defaultLetter);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
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
}
