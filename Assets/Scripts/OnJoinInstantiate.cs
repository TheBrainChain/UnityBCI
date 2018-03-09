/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnJoinInstantiate : MonoBehaviour {

	public GameObject[] PrefabsToInstantiate;

	private Rigidbody rb;
	private MeshRenderer mr;
	private float xPos = 45;

	public void OnJoinedRoom(){
		if (this.PrefabsToInstantiate != null) {
			foreach (GameObject o in this.PrefabsToInstantiate) {
				Debug.Log ("Instantiating: " + o.name);



				if (PhotonNetwork.room.PlayerCount == 1) {
					PhotonNetwork.Instantiate (o.name, Vector3 (-xPos, 0.5f, 0f), Quaternion.identity, 0);
					mr = o.GetComponent<MeshRenderer> ();
					mr.material.color = Color.blue;
				} 
				else {
					PhotonNetwork.Instantiate (o.name, Vector3 (xPos, 0.5f, 0f), Quaternion.identity, 0);
					mr = o.GetComponent<MeshRenderer> ();
					mr.material.color = Color.green;
				}
		}
	}
}*/
