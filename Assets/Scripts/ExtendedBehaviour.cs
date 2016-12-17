using UnityEngine;
using System.Collections;
using System;

public class ExtendedBehaviour : MonoBehaviour {  // http://answers.unity3d.com/answers/733831/view.html

	public void Wait(float seconds, Action action){
			StartCoroutine(_wait(seconds, action));
		}
	 
	IEnumerator _wait(float time, Action callback){
			yield return new WaitForSeconds(time);
			callback();
		}
}
