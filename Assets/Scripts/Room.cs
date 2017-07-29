using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Focus() {
		Camera.main.GetComponent<RoomCamera>().target = new Vector3 (transform.position.x, transform.position.y, Camera.main.transform.position.z);
	}
}
