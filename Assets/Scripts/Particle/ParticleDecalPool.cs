using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDecalPool : MonoBehaviour {

	private int particleDecalDataIndex;
	private ParticleDecalData[] particleData;
	public int maxDecals = 100;
	public float decalSizeMin = .5f;
	public float decalSizeMax = 1.5f;
	private ParticleSystem decalParticleSystem;
	private ParticleSystem.Particle[] particles;

	// Use this for initialization
	void Start () 
	{
		decalParticleSystem = GetComponent<ParticleSystem> ();
		particleData = new ParticleDecalData[maxDecals];
		for (int i = 0; i < maxDecals; i++)
		{
			particleData [i] = new ParticleDecalData ();
		}
		particles = new ParticleSystem.Particle[maxDecals];
	}

	public void ParticleHit (ParticleCollisionEvent particleCollisionEvent, Gradient colorGradient)
	{
		setParticleData (particleCollisionEvent, colorGradient);
		DisplayParticles ();
	}
	
	// Update is called once per frame
	void setParticleData (ParticleCollisionEvent particleCollisionEvent, Gradient colorGradient) 
	{
		if (particleDecalDataIndex >= maxDecals)
		{
			particleDecalDataIndex = 0;
		}

		particleData [particleDecalDataIndex].position = particleCollisionEvent.intersection;
		Vector3 particleRotationEuler = Quaternion.LookRotation (particleCollisionEvent.normal).eulerAngles;
		particleRotationEuler.z = Random.Range (0,360);
		particleData [particleDecalDataIndex].rotation = particleRotationEuler;
		particleData [particleDecalDataIndex].size = Random.Range(decalSizeMin,decalSizeMax);
		particleData [particleDecalDataIndex].color = Color.yellow;
		particleDecalDataIndex++;
	}

	void DisplayParticles()
	{
		for (int i = 0; i < particleData.Length; i++)
		{
			particles [i].position = particleData [i].position;
			particles [i].rotation3D = particleData [i].rotation;
			particles [i].startSize = particleData [i].size;
			particles [i].startColor = particleData [i].color;
		}
		decalParticleSystem.SetParticles (particles, particles.Length);
	}
}
