// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Networking;

// public class PuckSpawner : NetworkBehaviour {

// 	public GameObject puckPrefab;

//     public override void OnStartServer()
//     {
//         var pos = new Vector3 (0f, 0f, 0f);
// 		var rotation = Quaternion.Euler(0, 0, 0);

// 		var puck = (GameObject)Instantiate(puckPrefab, pos, rotation);
// 		NetworkServer.Spawn(puck);
//     }
// }
