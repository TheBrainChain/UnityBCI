using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class littlemoverscript : MonoBehaviour {

	public BCIClass BCI_instance = new BCIClass();

	private Rigidbody rb;

	public float speed = 30f;

	void Start () {
		rb = this.GetComponent<Rigidbody> ();
		rb.position = new Vector3 (-45f, 1, 0);
		BCI_instance.receiveThread = new Thread (() => BCI_instance.receiveData (BCI_instance.receivePort));
		BCI_instance.receiveThread.IsBackground = true;
		BCI_instance.receiveThread.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		ProcessInputs ();	
	}



void ProcessInputs()
{
	
	float moveVertical = Input.GetAxis ("Vertical");
//	Vector3 movement = new Vector3 (0.0f, 0.0f, moveVertical * 20f);
		Vector3 movement = new Vector3 (0.0f, 0.0f, BCI_instance.SignalCode2*speed);
	rb.velocity = movement;
		print (movement);

}
	

	//rb.velocity = new Vector3(0f,0f,BCIClass.SignalCode*Time.deltaTime);

	void OnDestroy()
	{
		BCI_instance.client.Close ();
	}

}