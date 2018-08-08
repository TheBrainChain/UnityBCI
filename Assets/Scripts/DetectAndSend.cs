using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DetectAndSend : MonoBehaviour {

    public int collisionTarget;
    bci2k BCI2K;
    private string timePerTarget;
    private GameObject[] Targets;
    private int[,] TargetRules;
    private int[] Target1Rules;
    private int[] Target2Rules;
    private int[] Target3Rules;
    private int[] Target4Rules;
    private int[] Target5Rules;
    private int[] Target6Rules;
    private int[] Target7Rules;
    private int[] Target8Rules;
    private int[] hit;
    private List<int> history;
    public float totalHits = 0;
    int controllerIndex;
    int startTarget;
    private Rigidbody rb;
    targetChase logger;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == collisionTarget.ToString())
        {
            history.Add(int.Parse(collision.gameObject.name));
            totalHits += 1;
            
            hit[int.Parse(collision.gameObject.name)] += 1;
            logger.writeToLogFile("StimChange! : " + collisionTarget, System.DateTime.Now);
            StartCoroutine(newTarget(collision.gameObject));
        }
    }

    IEnumerator newTarget(GameObject colliderName)
    {
        colliderName.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        int collisionOptions = Random.Range(0, 3);
        print(history[history.Count - 1]);

        //print(TargetRules[int.Parse(colliderName.name), 0]);
        if (TargetRules[int.Parse(colliderName.name), collisionOptions] != history[history.Count-1])
        {
            collisionTarget = TargetRules[int.Parse(colliderName.name), collisionOptions];
        }
        else
        {
            collisionOptions = Random.Range(0, 3);
            collisionTarget = TargetRules[int.Parse(colliderName.name), collisionOptions];
        }

        yield return new WaitForSeconds(1f);

        colliderName.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        Targets[collisionTarget].GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        float percentage = (hit[int.Parse(colliderName.name)] / totalHits);
        BCI2K.websockets[0].Send(
            "E 1 " +
            BCI2K.setState("SelectedTarget", collisionTarget)
        );
        logger.writeToLogFile("Total hits: " + totalHits, System.DateTime.Now);

    }

    void Start () {
        BCI2K = GameObject.Find("BCI2000Manager").GetComponent<bci2k>();
        history = new List<int>();

        if (BCI2K.configFile != null && File.Exists(BCI2K.configFile))
        {
            var tempStruct = JsonUtility.FromJson<externalConfig>(BCI2K.ReadFile(BCI2K.configFile));
            timePerTarget = tempStruct.timePerTarget;
            startTarget = tempStruct.targetToStartAt;
        }

        controllerIndex = (int)gameObject.GetComponent<SteamVR_TrackedObject>().index;
        logger = GameObject.Find("TaskManager").GetComponent<targetChase>();
        TargetRules = new int[8, 3];
        hit = new int[8];
        TargetRules[0, 0] = 1;
        TargetRules[0, 1] = 3;
        TargetRules[0, 2] = 4;
        TargetRules[1, 0] = 0;
        TargetRules[1, 1] = 2;
        TargetRules[1, 2] = 5;
        TargetRules[2, 0] = 1;
        TargetRules[2, 1] = 3;
        TargetRules[2, 2] = 6;
        TargetRules[3, 0] = 0;
        TargetRules[3, 1] = 2;
        TargetRules[3, 2] = 7;
        TargetRules[4, 0] = 0;
        TargetRules[4, 1] = 5;
        TargetRules[4, 2] = 7;
        TargetRules[5, 0] = 1;
        TargetRules[5, 1] = 4;
        TargetRules[5, 2] = 6;
        TargetRules[6, 0] = 2;
        TargetRules[6, 1] = 5;
        TargetRules[6, 2] = 7;
        TargetRules[7, 0] = 3;
        TargetRules[7, 1] = 4;
        TargetRules[7, 2] = 6;

        Targets = new GameObject[8];
        for (int i = 0; i < 8; i++)
        {
            Targets[i] = GameObject.Find("Targets_Cube").transform.GetChild(i).gameObject;
        }
        collisionTarget = startTarget;
        Targets[collisionTarget].GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        BCI2K.websockets[0].Send(
            "E 1 " +
            BCI2K.setState("SelectedTarget", collisionTarget)
        );
    }
    private void Update()
    {
        logger.writeToLogFile("Controller velocity: " + SteamVR_Controller.Input(controllerIndex).velocity*100, System.DateTime.Now);
        if (Input.GetKeyDown("up"))
        {
            GameObject.Find("TaskManager").transform.localPosition += new Vector3(0, .01f, 0);
        }
        if (Input.GetKeyDown("down"))
        {
            GameObject.Find("TaskManager").transform.localPosition += new Vector3(0, -.01f, 0);
        }
        if (Input.GetKeyDown("left"))
        {
            GameObject.Find("TaskManager").transform.localPosition += new Vector3(-.01f, 0, 0);
        }
        if (Input.GetKeyDown("right"))
        {
            GameObject.Find("TaskManager").transform.localPosition += new Vector3(.01f, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            GameObject.Find("TaskManager").transform.localPosition += new Vector3(0, 0, .01f);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            GameObject.Find("TaskManager").transform.localPosition += new Vector3(0, 0, -.01f);
        }
    }
}
