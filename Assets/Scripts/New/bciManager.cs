using System;
using BCI2K.cs;
using System.Threading;
using UnityEngine;
using TMPro;


public class bciManager : MonoBehaviour
{
    public static BCI2K_OperatorConnection bci_Op = new BCI2K_OperatorConnection("ws://127.0.0.1:80");
    public static BCI2K_DataConnection bci_Source = new BCI2K_DataConnection("ws://127.0.0.1:20100");
    public int lengthOfLineRenderer = 500;    // Start is called before the first frame update
    int i = 0;
    public GameObject[] linez;
    public LineRenderer[] lineRenderer;
    bool doe = true;
    public float scaler;
    public float timeScale = .015f;
    public int trialCounter;
    bool detect = true;
    public void Awake()
    {
        bci_Op.operatorWS.Connect();
        bci_Op.startupSystem();
    }

    public void StartBCI2000()
    {
        bci_Op.start();
        GameObject canvas = GameObject.Find("Canvas");

        GameObject go = new GameObject();
        TextMeshPro te = go.AddComponent<TextMeshPro>();
        go.transform.SetParent(canvas.transform);
        go.transform.localPosition = new Vector3(-431.63f, -175.722f, 4.21f);
        go.transform.localScale = new Vector3(1, 1, 1) * .0457f;
        te.text = "Ch1";

        GameObject go2 = new GameObject();
        TextMeshPro te2 = go2.AddComponent<TextMeshPro>();
        go2.transform.SetParent(canvas.transform);
        go2.transform.localPosition = new Vector3(-431.63f, -175.889f, 4.21f);
        go2.transform.localScale = new Vector3(1, 1, 1) * .0457f;
        te2.text = "Ch2";
    }

    public void startExecutables()
    {
        bci_Op.startExecutable("SignalGenerator", "LogKeyboard=1");
        bci_Op.startExecutable("DummySignalProcessing", "");
        bci_Op.startExecutable("DummyApplication", "");
        bci_Op.setParameter("WSSourceServer", "*:20100");
    }

    public void Reset()
    {
        bci_Op.resetSystem();
    }

    public void ConnectToDataStream()
    {
        bci_Source.dataWS.Connect();
        bci_Source.onGenericSignal += () =>
        {
            Debug.Log(bci_Source.signal[0]);
        };
        bci_Source.onSignalProperties += () =>
        {
            Debug.Log(bci_Source.signalProps);
        };
    }
}




// linez = new GameObject[BCI2000.nChannels];
//                     lineRenderer = new LineRenderer[BCI2000.nChannels];
//                     for (int j = 0; j < BCI2000.nChannels; j++)
//                     {
//                         linez[j] = new GameObject();
//                         lineRenderer[j] = linez[j].AddComponent<LineRenderer>();
//                         lineRenderer[j].material = new Material(Shader.Find("Sprites/Default"));
//                         lineRenderer[j].widthMultiplier = 0.01f;
//                         lineRenderer[j].positionCount = lengthOfLineRenderer;
//                         lineRenderer[j].material.color = Color.red;
//                     }
//                 }
//                 if (BCI2000.signal[BCI2000.nElements * BCI2000.nChannels - 1] > 100 && detect == true)
//                 {
//                     detect = false;
//                     trialCounter = trialCounter + 1;
//                     print("Trial!");
//                     print(trialCounter);
//                 }
//                 if (BCI2000.signal[BCI2000.nElements * BCI2000.nChannels - 1] < 10)
//                 {
//                     detect = true;
//                 }

//                 for (int j = 0; j < BCI2000.nChannels; j++)
//                 {
//                     lineRenderer[j].SetPosition(i, new Vector3(i * timeScale + 1.25f, (BCI2000.signal[BCI2000.nElements * j] / scaler + j * 0.15f) - 2.0f, 4.0f));
//                 }
//                 i++;
//                 if (i == lengthOfLineRenderer) i = 0;
//             }
//         }