using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dimmer : MonoBehaviour {

	private float duration;
	private float timer;
	private float durationInv;
	public Text text;

	private Image img;

	float targetAlpha, startAlpha;

	public void FadeOut(float dur) {
		duration = dur;
		durationInv = 1f / (duration != 0f ? duration : 1f );
		timer = 0f;

		targetAlpha = 0f;
		startAlpha = img.color.a;
	}

	public void FadeIn(float dur) {

		duration = dur;
		durationInv = 1f / (duration != 0f ? duration : 1f );
		timer = 0f;

		targetAlpha = 1f;
		startAlpha = img.color.a;
	}

	// Use this for initialization
	void Start () {
		img = GetComponent<Image> ();
		FadeOut (2f);
	}
	
	// Update is called once per frame
	void Update () {
		float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer * durationInv);
		timer += Time.deltaTime;

		img.color = new Color (0, 0, 0, alpha);
	}

	public void SetText(string txt) {
		text.text = txt;
	}
}
