using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour {
	private AudioSource audioSrc;
	private Button audioBtn;

	// Use this for initialization
	void Start () {
		audioSrc = GetComponent<AudioSource> ();
		audioBtn = GetComponent<Button> ();
	}

	public void ToggleSound () {
		EventSystem.current.SetSelectedGameObject (null, null); // So that the button isn't triggered again by the space key
		if (audioSrc.isPlaying ) {
			audioSrc.Stop();
			audioBtn.image.sprite = Resources.LoadAll<Sprite> ("SoundButton")[1];

		} else {
			audioSrc.Play();
			audioBtn.image.sprite  = Resources.LoadAll<Sprite> ("SoundButton")[0];

		}
	}
}