using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

	void DoFootstep() {
		AudioManager.Instance.PlayEffectAt(10, transform.position, 0.5f);
	}
}
