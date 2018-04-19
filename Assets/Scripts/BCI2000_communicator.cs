using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCI2000_communicator : MonoBehaviour {

    BCI2000_init initBCI2000 = new BCI2000_init("127.0.0.1", 55404);

	// Use this for initialization
	public void runBCI2000()
    {
        initBCI2000.configureBCI2000Session("SignalGenerator", "DummySignalProcessing", "DummyApplication", "CGC", "127.0.0.1",55404);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
