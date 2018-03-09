//copied from http://bhattakash.com/local-network-discovery-using-unet/ on 8/7/2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class CustomNetworkDiscovery : NetworkDiscovery {
	
	public static CustomNetworkDiscovery Instance;

	public Action<string, string> onServerDetected;

	void OnServerDetected(string fromAddress, string data) {
		if (onServerDetected != null) {
			onServerDetected.Invoke(fromAddress, data);
		}
	}

	void Awake() {
		if (Instance == null) {
			Instance = this;
		}
	}


	void Start() {
		InitializeNetworkDiscovery();
	}

	public bool InitializeNetworkDiscovery() {
		return Initialize();
	}

	public void StartBroadcasting() {
		StartAsServer();
	}

	public void ReceiveBraodcast() {
		StartAsClient();
	}

	public void StopBroadcasting() {
		StopBroadcast();
	}

	public void SetBroadcastData(string broadcastPayload) {
		broadcastData = broadcastPayload;
	}

	public override void OnReceivedBroadcast(string fromAddress, string data) {
		OnServerDetected(fromAddress.Split(':')[3], data);
	}
}