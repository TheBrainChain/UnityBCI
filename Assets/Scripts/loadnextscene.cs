using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class loadnextscene : MonoBehaviour {


	public void intro()
	{

		SceneManager.LoadScene ("Setup");
	}
	public void motorImagery()
	{
		SceneManager.LoadScene ("MotorImagery");
	}
	public void drawer()
	{
		SceneManager.LoadScene ("PinchDrawDemo");
	}
	public void PushTarget()
	{
		SceneManager.LoadScene ("PushTarget");
	}

}
