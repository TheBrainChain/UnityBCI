
/*
using UnityEngine;
using System.Collections;

public class SSVEP_Target_mover : MonoBehaviour
{
	public int count = 0;
	public GameObject Target;

	public GameObject SSVEP_Red;
	public GameObject SSVEP_Blue;
	public GameObject SSVEP_Green;

	public Renderer Red_rend;
	public Renderer Blue_rend;
	public Renderer Green_rend;
	public bool redOn;
	public bool blueOn;
	public bool greenOn;
	private Vector3 velocity = Vector3.zero;
	public void Start()
	{

		Red_rend = SSVEP_Red.GetComponent<Renderer> ();
		Red_rend.enabled = true;
		Blue_rend = SSVEP_Blue.GetComponent<Renderer> ();
		Blue_rend.enabled = true;
		Green_rend = SSVEP_Green.GetComponent<Renderer> ();
		Green_rend.enabled = true;
		redOn = false;
		blueOn = false;
		greenOn = false;
	}

	IEnumerator Wait(float sec, string Cube)
	{
		if (Cube == "Red")
		{
			redOn = true;
			Red_rend.enabled = false;
			yield return new WaitForSeconds (sec);
			Red_rend.enabled = true;
			redOn = false;
		}
		if (Cube == "Blue")
		{
			blueOn = true;
			Blue_rend.enabled = false;
			yield return new WaitForSeconds (sec);
			Blue_rend.enabled = true;
			blueOn = false;
		}
		if (Cube == "Green")
		{
			greenOn = true;
			Green_rend.enabled = false;
			yield return new WaitForSeconds (sec);
			Green_rend.enabled = true;
			greenOn = false;
		}
	}

	public void FixedUpdate()
	{
		Red_rend.enabled = true;
		Green_rend.enabled = true;
		Blue_rend.enabled = true;

		if (count % 5 == 0)
		{
			if (redOn = false)
			{
				Red_rend.enabled = true;
				redOn = true;
			} else
			{
				Red_rend.enabled = false;
				redOn = false;
			}
		}
		if (count % 6 == 0)
		{
			if (greenOn = false)
			{
				Green_rend.enabled = true;
				greenOn = true;
			} else
			{
				Green_rend.enabled = false;
				greenOn = false;
			}
		}
		if (count % 9 == 0)
		{
			if (blueOn = false)
			{
				Blue_rend.enabled = true;
				blueOn = true;
			} else
			{
				Blue_rend.enabled = false;
				blueOn = false;
			}
		}
		count = count + 1;



//		transform.Translate (new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), Input.GetAxis ("Zed")));

		Vector3 TargetPos = Random.insideUnitSphere * Random.Range (-100, 100);

		Target.transform.position = Vector3.SmoothDamp(Target.transform.position, TargetPos, ref velocity,2);

	}
		
	void OnCollisionEnter( Collision col)
	{
		if (col.gameObject.name == "Cube")
		{
			Target.transform.position = Random.insideUnitSphere;
		}
	}
}
*/