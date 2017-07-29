using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	public Transform[] deathBalls;
	public Vector3[] deathBallDirections;

	public void Focus() {
		Camera.main.GetComponent<RoomCamera>().target = new Vector3 (transform.position.x, transform.position.y, Camera.main.transform.position.z);

		GameManager.Instance.ClearDeathBalls ();

		if (deathBalls.Length > 0) {
			for (int i = 0; i < deathBalls.Length; i++) {
				GameManager.Instance.AddDeathBall (deathBalls [i].position, deathBallDirections[i]);
			}
		}
	}
}
