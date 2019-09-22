// #if !BESTHTTP_DISABLE_WEBSOCKET

// using System;
// using UnityEngine;
// using BestHTTP.WebSocket;
// using UnityEngine.UI;
// using System.Diagnostics;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine.XR;
// using System.IO;
// using System.Text;

// [Serializable]
// public class externalConfig
// {
//     public string ipAddress;
//     public string hand;
//     public string timePerTarget;
//     public int targetToStartAt;
//     public string SignalSource;
//     // We could also define other variables here.
// }

//     #endregion

//     public int ID;

//     public string ReadFile(string filename)
//     {
//         StreamReader fileReader = new StreamReader(filename, Encoding.Default);
//         string returnString;
//         using (fileReader)
//             returnString = fileReader.ReadToEnd();
//         return returnString;
//     }

//     private void Start()
//     {
//         configFile = Application.streamingAssetsPath + "/config.json";
//         print(Application.streamingAssetsPath);

//         if (configFile != null && File.Exists(configFile))
//         {
//             var tempStruct = JsonUtility.FromJson<externalConfig>(ReadFile(configFile));
//             print(tempStruct);
//             IPaddress = tempStruct.ipAddress;
//             hand = tempStruct.hand;
//         }
//         websockets = new WebSocket[4];
//         websockets[0] = mainWebsocket;
//         websockets[1] = filterWebsocket;
//         websockets[2] = sourceWebsocket;
//         websockets[3] = connectorWebsocket;
//         //closeButton.GetComponent<Button>().onClick.AddListener(OnDestroy);

//         //openMainWSButton.GetComponent<Button>().onClick.AddListener(delegate { openWS(mainWebsocket, "ws://"+ IPaddress+":80", 0); });
//         //openFilterWSButton.GetComponent<Button>().onClick.AddListener(delegate { openWS(filterWebsocket, "ws://" + IPaddress + ":20203", 1); });
//         //openSourceWSButton.GetComponent<Button>().onClick.AddListener(delegate { openWS(sourceWebsocket, "ws://" + IPaddress + ":20100", 2); });
//         //openConnectorWSButton.GetComponent<Button>().onClick.AddListener(delegate { openWS(connectorWebsocket, "ws://" + IPaddress + ":20323", 3); });
//         //sendMsgButton.GetComponent<Button>().onClick.AddListener(sendWSmsg);
//         //openBCIButton.GetComponent<Button>().onClick.AddListener(openBCI);
//         //switchWordButton.GetComponent<Button>().onClick.AddListener(switchSceneWord);
//         //switchMotorButton.GetComponent<Button>().onClick.AddListener(switchSceneMotor);
//         openWS(mainWebsocket, "ws://"+ IPaddress+":80", 0);
//     }
//     IEnumerator LoadStartVR(bool enabled)
//     {
//         yield return null;
//         XRSettings.enabled = enabled;
//     }
//     void switchSceneWord()
//     {
//         UnityEngine.SceneManagement.SceneManager.LoadScene("Hopkins",
//             UnityEngine.SceneManagement.LoadSceneMode.Additive);
//         switchMotorButton.gameObject.SetActive(false);
//         switchWordButton.gameObject.SetActive(false);
//         openBCIButton.gameObject.SetActive(false);
//         closeButton.gameObject.SetActive(false);
//         openMainWSButton.gameObject.SetActive(false);
//     }
//     void switchSceneMotor()
//     {
//         switchMotorButton.gameObject.SetActive(false);
//         switchWordButton.gameObject.SetActive(false);
//         openBCIButton.gameObject.SetActive(false);
//         closeButton.gameObject.SetActive(false);
//         openMainWSButton.gameObject.SetActive(false);
//         TaskManager.SetActive(true);

//     }

//     void openWS(WebSocket ws, string address, int IDent)
//     {
//         if (ws == null)
//         {
//             ID = IDent;
//             websockets[ID] = new WebSocket(new Uri(address));
//             websockets[ID].OnOpen += OnOpen;
//             websockets[ID].OnClosed += OnClosed;
//             websockets[ID].OnError += OnError;
//             websockets[ID].OnMessage += OnMessageReceived;
//             websockets[ID].OnBinary += OnBinaryMessageReceived;
//             websockets[ID].Open();
//             //setup retry?
//         }
//     }

//     void sendWSmsg()
//     {
//         print(mainWebsocket.IsOpen);
//         if (websockets[ID] != null && websockets[ID].IsOpen)
//         {
//             websockets[ID].Send("E 1 " + txt2send.text);
//         }
//     }

//     void OnDestroy()
//     {
//         //mainWebsocket.Close();
//         //filterWebsocket.Close();
//         //sourceWebsocket.Close();
//         //connectorWebsocket.Close();
//     }

//     private void decodeGenericSignal(byte[] message)
//     {
//         byte signalType = message[2];
//         var nChannels = message[3];
//         //nChannels += message[4] //if #channels > 255
//         var nElements = message[5];
//         //nElements += message[6] //if #elements > 255

//         //print(signalType);  //0: int16, 1: float24, 2: float32, 3: int32
//         //print(nChannels);
//         //print(nElements);
//         byte[] signalArray = new byte[message.Length - 7];
//         Array.Copy(message, 7, signalArray, 0, message.Length - 7);

//         signal.Clear();
//         for (int i = 0; i < nChannels * nElements; i++)
//         {
//             byte[] newArr = new byte[4];
//             Array.Copy(signalArray, (4 * i), newArr, 0, 4);
//             float myFloat = BitConverter.ToSingle(newArr, 0);
//             signal.Add(myFloat);
//         }
//     }

//     private void decodeStateFormat(byte[] message)
//     {
//         var msg = System.Text.Encoding.ASCII.GetString(message).Split('\n');
//         foreach (var mess in msg)
//         {
//             if (mess != "")
//             {
//                 stateName.Add(mess.Split(' ')[0]);
//                 bitWidth.Add(int.Parse(mess.Split(' ')[1]));
//                 defaultValue.Add(int.Parse(mess.Split(' ')[2]));
//                 byteLocation.Add(int.Parse(mess.Split(' ')[3]));
//                 bitLocation.Add(int.Parse(mess.Split(' ')[4]));
//             }
//         }

//         for (int i=0;i<stateName.Count; i++)
//         {
//             vecOrder.Add(stateName[i], byteLocation[i] * 8 + bitLocation[i]);
//             stateOrder.Add(stateName[i], bitWidth[i]);
//         }
//         stateFormat = vecOrder.ToList();
//         stateVecOrder = stateOrder.ToList();

//         //Sort the list based on key values
//         stateFormat.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
//     }

//     private void decodeSignalProperties(byte[] message)
//     {
//         string strMessage = System.Text.Encoding.ASCII.GetString(message);
//         strMessage = strMessage.Replace("{", " { ");
//         strMessage = strMessage.Replace("}", " } ");


//         var msg = strMessage.Split(' ').ToList();
// ;
//         for (int i = 0; i < msg.Count; i++)
//         {
//             //if (string.IsNullOrWhiteSpace(msg[i]))
//             //{
//             //    msg.Remove(msg[i]);
//             //}
//         }


//         signalName = msg[0];

//         var count = 1;
//         for (int i = 0; i < msg.Count; i++)
//         {
//             if (msg[i] == "{")
//             {
//                 i++;
//                 while (msg[i] != "}")
//                 {
//                     if (count == 1)
//                     {
//                         signalChannels.Add(msg[i]);
//                     }
//                     else if(count == 2)
//                     {
//                         signalElements.Add(msg[i]);
//                     }
//                     i++;
//                 }
//                 count++;
//             }
//         }
//         signalProps = new string[6 * signalChannels.Count];

//         for (int i = 0; i < signalChannels.Count; i++)
//         {
//             signalProps[i*6] = signalChannels[i];
//             signalProps[i * 6 + 1] = signalElements[(i * 4) + i + 0];
//             signalProps[i * 6 + 2] = signalElements[(i * 4) + i + 1];
//             signalProps[i * 6 + 3] = signalElements[(i * 4) + i + 2];
//             signalProps[i * 6 + 4] = signalElements[(i * 4) + i + 3];
//             signalProps[i * 6 + 5] = signalElements[(i * 4) + i + 4];
//         }
//     }

//     private void decodeStateVector(byte[] message)
//     {
//         //for (int i = 1; i < message.Length; i++)
//         //{
//         //    print(message[i]);
//         //}
//         //        int i = 1;
//         //List<byte> a = new List<byte>();
//         //while (message[i] != 0)
//         //{
//         //    a.Add(message[i]);
//         //    i++;
//         //}

//         var zeroInd = 1;
//         List<byte> stateVectorLength = new List<byte>();
//         List<byte> subsStateVectors = new List<byte>();

//         for (int i = 1; i<message.Length;i++)
//         {
//             print("" + i + ": "+ message[i]);
//            while(message[i] != 0 && zeroInd < 3)
//             {
//                 print("" + i + ": " + message[i]);

//                 if (zeroInd == 1)
//                 {
//                     stateVectorLength.Add(message[i]);
//                 }
//                 else {
//                     subsStateVectors.Add(message[i]);
//                 }
//                 i++;
//             }
//             zeroInd++;
//         }

//         print(System.Text.Encoding.ASCII.GetString(stateVectorLength.ToArray()));       //56        //wtf do these mean?
//         print(System.Text.Encoding.ASCII.GetString(subsStateVectors.ToArray()));        //101

//     }

//     void OnBinaryMessageReceived(WebSocket ws, byte[] message)
//     {
//         if (message[0] == 3)
//         {
//             //Runs once upon open to give you the byte location and bit width of the states.
//             decodeStateFormat(message);
//             //print(stateFormat[0].Key);
//             //print(stateFormat[0].Value);
//             //print(stateVecOrder[0].Key);
//             //print(stateVecOrder[0].Value);
//         }
//         else if (message[0] == 4)
//         {
//             if (message[1] == 1)
//             {
//                 decodeGenericSignal(message);
//                 print(signal[0]); //First Channel
//                 //print(signal[0+sampleBlockSize] //SecondChannel



//             }
//             if (message[1] == 3)
//             {
//                 decodeSignalProperties(message);
//                 //print("Channel: " + signalProps[0] + " " + "Offset: " + signalProps[1]);

//             }
//             else {
//                 //UnityEngine.Debug.Log("This supplement is not currently supported");
//             }
//         }
//         else if (message[0] == 5)
//         {
//             //decodeStateVector(message);
//         }
//         else
//         {
//             //UnityEngine.Debug.Log("Unsupported descriptor");
//         }
//     }

//     void OnMessageReceived(WebSocket ws, string message)
//     {
//         //if (message[0].ToString().StartsWith("O"))
//         //{
//         //    if (txt2send.text == "List Parameter Stimuli")
//         //    {
//         //        print(message);
//         //        int stimAmt = int.Parse(message.Substring(message.IndexOf("}") + 1, 4));
//         //        char[] separators = new char[] { ' ' };
//         //        print(stimAmt);
//         //        //for (int i = 0; i < stimAmt; i++)
//         //        //{
//         //        //    Words.Add(message.Split(separators, StringSplitOptions.RemoveEmptyEntries)[i + 11]);
//         //        //    print(Words[i]);
//         //        //}
//         //    }
//         //    if (message[2].ToString().StartsWith("2"))
//         //    {
//         //        stim = (message.Substring(3, message.Length - 3));
//         //    }
//         //    if (message[2].ToString().StartsWith("1"))
//         //    {
//         //        stimCode = int.Parse(message.Substring(3, message.Length - 3));
//         //    }
//         //    //else
//         //    //{
//         //    //    print(message);
//         //    //}
//         //}
//     }

//     void OnClosed(WebSocket ws, UInt16 code, string message)
//     {
//         print(string.Format("-WebSocket closed! Code: {0} Message: {1}\n", code, message));
//         mainWebsocket = null;
//         sourceWebsocket = null;
//         filterWebsocket = null;
//         connectorWebsocket = null;
//     }

//     void OnError(WebSocket ws, Exception ex)
//     {
//         string errorMsg = string.Empty;
//         if (!ws.IsOpen)
//         {
//             print("A");
//             ws.Open();
//         }

//         print(string.Format("-An error occured: {0}\n", (ex != null ? ex.Message : "Unknown Error " + errorMsg)));
//         mainWebsocket = null;
//     }

//     void OnOpen(WebSocket ws)
//     {
//         print("Websocket is open!");
//         switchSceneMotor();

//     }

//     public void openBCI()
//     {
//         ProcessStartInfo PSI = new ProcessStartInfo("START.bat");
//         PSI.WorkingDirectory = "E:\\Projects\\Hopkins\\GIT\\GIT_bci2000web";
//         Process.Start(PSI);
//     }
// }

// #endif