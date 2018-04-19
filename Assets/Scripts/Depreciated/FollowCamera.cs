using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

	public GameObject cam = null;

	private Vector3 positionOffset = Vector3.zero;
	// Use this for initialization
	void Start () {

		positionOffset = cam.transform.position + transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.position = cam.transform.position + positionOffset;


	}
}
