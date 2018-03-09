using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class BallLimitingVel : MonoBehaviour {
	public float MaxVelocity = 25f;
	public float MinVelocity = 15f;
	public GameObject Ball;
	private Rigidbody rb;	

	void FixedUpdate () {
		rb = Ball.GetComponent <Rigidbody> ();
		if (Mathf.Abs(rb.velocity.x) <= 1f){
			Vector3 v = new Vector3 (Random.Range (-20f, 20f), 0f, 0f);
			rb.AddForce (v);
		}
		if (Mathf.Abs(rb.velocity.z) <= 1f) {
			Vector3 z = new Vector3 (0f, 0f, Random.Range (-20f, 20f));
			rb.AddForce (z);
		}
		if (rb.velocity.sqrMagnitude > MaxVelocity) {
			rb.velocity *= 0.99f;
		}
		if (rb.velocity.sqrMagnitude < MinVelocity) {
			rb.velocity *= 1.01f;
		}
	}
}
