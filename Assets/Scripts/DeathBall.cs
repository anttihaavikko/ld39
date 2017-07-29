using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBall : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D coll) {
		AudioManager.Instance.PlayEffectAt(6, transform.position, 0.1f);
	}
}
