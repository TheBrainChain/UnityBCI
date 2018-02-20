using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class scene_Manager : MonoBehaviour {

	static scene_Manager Instance;

	void Start () {
		if (Instance != null) {
			GameObject.Destroy (gameObject);
		} else {
			GameObject.DontDestroyOnLoad (gameObject);
			Instance = this;
		}
	}

	//Make into a single function
	public void intro()
	{
		SceneManager.LoadScene ("IntroScene");
	}
	public void motorImagery()
	{
		SceneManager.LoadScene ("MotorImagery");
	}
	public void threeD()
	{
		SceneManager.LoadScene ("3D_3Subj");
	}
}
