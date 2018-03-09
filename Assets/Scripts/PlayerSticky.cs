using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSticky : MonoBehaviour {
	private Rigidbody rb;
	private Rigidbody rp;
	public GameObject Ball;
	public GameObject Paddle;

	void OnCollisionEnter (Collision collision)	{
		rb = Ball.GetComponent <Rigidbody>();
		if (collision.collider.tag == "Ball") {
			rb.isKinematic = true;
		}
	}
}
