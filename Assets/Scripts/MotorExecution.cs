using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;
using System.Threading;

public class MotorExecution : MonoBehaviour 
{

	public GameObject ball;
	private LeapServiceProvider provider;
	public GameObject target1;
	public GameObject target2;
	public GameObject target3;
	public GameObject target4;
	public GameObject target5;
	public List<GameObject> targets = new List<GameObject>();

    private IEnumerator coroutine;


    public void moveBallWithLeap()
    {
        Frame currentFrame = provider.CurrentFrame;
      //  Debug.Log(currentFrame.Serialize);

        if (currentFrame.Hands.Count > 1)
            {
            List<Hand> hands = currentFrame.Hands;

            Hand hand1 = hands[0];
            Hand hand2 = hands[1];

            float pitch1 = hand1.Direction.Pitch;
            float pitch2 = hand2.Direction.Pitch;

            //Debug.Log(hand1.PalmPosition);

            if ((hand1.GrabStrength < .9f && hand1.GrabStrength > .4f) || (hand2.GrabStrength < .9f && hand2.GrabStrength > .8f))
            {
                if (hand1.GrabStrength > .8f && hand2.GrabStrength > .8f)
                {
                    ball.transform.Translate(0, 10 * Time.deltaTime, 0);
                }
                else if (hand1.IsRight && hand1.GrabStrength > 0.8f)
                {
                    ball.transform.Translate(10 * Time.deltaTime, 0, 0);
                }
                else if (hand2.IsLeft && hand2.GrabStrength > 0.8f)
                {
                    ball.transform.Translate(-10 * Time.deltaTime, 0, 0);
                }
                else if (hand2.IsRight && hand2.GrabStrength > 0.8f)
                {
                    ball.transform.Translate(10 * Time.deltaTime, 0, 0);
                }
                else if (hand1.IsLeft && hand1.GrabStrength > 0.8f)
                {
                    ball.transform.Translate(-10 * Time.deltaTime, 0, 0);
                }
            }
            if (pitch1 > 2.5f && pitch2 > 2.5f)
            {
                ball.transform.Translate(0, -5 * Time.deltaTime, 0);
            }
        }
    }

    

  

	void Start () 
	{
		provider = FindObjectOfType<LeapServiceProvider>();

		targets.Add(target1);
		targets.Add(target2);
		targets.Add(target3);
		targets.Add(target4);
		targets.Add(target5);
	}
    private IEnumerator WaitforColor(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        targets[0].GetComponent<Renderer>().material.color = Color.black;
     
    }




	public void setTarget()
	{
		int rando = (int) Mathf.Round(Random.Range(0f, 4f));

		targets[rando].GetComponent<Renderer>().material.color = Color.black;
	
	}

    public void targetHit()
    {
        if ((ball.transform.localPosition.x < targets[0].transform.localPosition.x) && (targets[0].GetComponent<Renderer>().material.color == Color.black))
        {
            targets[0].GetComponent<Renderer>().material.color = Color.blue; 
        }
        if ((ball.transform.localPosition.x < targets[1].transform.localPosition.x) && (ball.transform.localPosition.y > targets[1].transform.localPosition.y) && (targets[1].GetComponent<Renderer>().material.color == Color.black))
        {
            targets[1].GetComponent<Renderer>().material.color = Color.blue; 
        }
        if ((ball.transform.localPosition.y > targets[2].transform.localPosition.y) && (targets[2].GetComponent<Renderer>().material.color == Color.black))
        {
            targets[2].GetComponent<Renderer>().material.color = Color.blue; 
        }
        if ((ball.transform.localPosition.y > targets[3].transform.localPosition.y) && (ball.transform.localPosition.x > targets[3].transform.localPosition.x) && (targets[3].GetComponent<Renderer>().material.color == Color.black))
        {
            targets[3].GetComponent<Renderer>().material.color = Color.blue; 
        }
        if ((ball.transform.localPosition.x > targets[4].transform.localPosition.x) && (targets[4].GetComponent<Renderer>().material.color == Color.black))
        {
            targets[4].GetComponent<Renderer>().material.color = Color.blue; 
        }
        if (targets[0].GetComponent<Renderer>().material.color == Color.blue)
        {
            StartCoroutine(WaitforColor(2));
        }
    }


	void Update ()
	{
        moveBallWithLeap();

        targetHit();

   

	}
}



