using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_StirringSpoon : MonoBehaviour {

	public float maxAnimSpeed = 10.0f;
	public float minAnimSpeed = 0.0f;

	private Animator animator;

	// Use this for initialization
	void Awake () {
		animator = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void setAnimationSpeedRatio(float speedRation)
	{
		if (speedRation > 1f)
			speedRation = 1f;
		if (speedRation < 0)
			speedRation = 0;
		float speed = minAnimSpeed + (maxAnimSpeed - minAnimSpeed) * speedRation;
		animator.SetFloat ("speed", speed);
	}
}
