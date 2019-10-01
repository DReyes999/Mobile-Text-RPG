using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextRPG
{
	public class ShakeBehavior : MonoBehaviour 
	{

		
		// Transform of the GameObject you want to shake
		public Transform bGtransform, eEngTransform;

		public int whichTransform;
		
		// Desired duration of the shake effect
		private float shakeDuration = 0f;
		
		// A measure of magnitude for the shake. Tweak based on your preference
		private float shakeMagnitude = 20.0f;
		
		// A measure of how quickly the shake effect should evaporate
		private float dampingSpeed = 1.0f;
		
		// The initial position of the GameObject
		Vector3 bGinitialPosition, eEngInitialPosition;
		
		void awake ()
		{
			
			if (transform == null)
				{
					bGtransform = transform.GetComponent(typeof(Transform)) as Transform;
					eEngTransform = transform.GetComponent(typeof(Transform)) as Transform;
				}
		}

		void OnEnable()
		{
			bGinitialPosition = bGtransform.localPosition;
			eEngInitialPosition = eEngTransform.localPosition;
		}
		
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () 
		{
			
			if (whichTransform == 0)
			{
				if (shakeDuration > 0)
				{
					bGtransform.localPosition = bGinitialPosition + Random.insideUnitSphere * shakeMagnitude;
					shakeDuration -= Time.deltaTime * dampingSpeed;
				}
				else
				{
					shakeDuration = 0f;
					bGtransform.localPosition = bGinitialPosition;
				}	
			} else if (whichTransform == 1)
			{
				if (shakeDuration > 0)
				{
					eEngTransform.localPosition = eEngInitialPosition + Random.insideUnitSphere * shakeMagnitude;
					shakeDuration -= Time.deltaTime * dampingSpeed;
				}
				else
				{
					shakeDuration = 0f;
					eEngTransform.localPosition = eEngInitialPosition;
				}
			}
			
		}

		public void TriggerShake(string transform) 
		{
			

			switch (transform)
			{
				case "BG":
					whichTransform = 0;
					break;
				case "EE":
					whichTransform = 1;
					break;
				default:
					break;
			}
			shakeDuration = 0.25f;

		}
	}
}
