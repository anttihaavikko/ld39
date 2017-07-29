using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour {

	public AudioSource audioSource;

	void Awake() {
		DontDestroyOnLoad (gameObject);
	}

	public void Play(AudioClip clip, float volume) {

		name = clip.name;

		audioSource.pitch = Random.Range (0.8f, 1.2f);
		audioSource.PlayOneShot (clip, AudioManager.Instance.volume * volume);

		Invoke ("DoDestroy", clip.length * 1.2f);
	}

	public void ChangeSpatialBlend(float blend, float min, float max) {
		audioSource.spatialBlend = blend;
		audioSource.minDistance = min;
		audioSource.maxDistance = max;
	}

	public void DoDestroy() {
		Destroy (gameObject);
	}
}
