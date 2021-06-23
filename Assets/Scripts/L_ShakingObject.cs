using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_ShakingObject : MonoBehaviour {

	//public parameters
	public GameObject dropItem;
	public float shakeCountNeeded;

	//private parameters
	private bool isMouseClicking = false;
	private Vector3 initPosition;
	private Rigidbody2D rb2D;
	private BoxCollider2D boxCollider;

	private Vector3 positionLastFrame;
	private float sqrShakeCountNeeded;
	private float tempSqrShakeCount = 0f;

	// Use this for initialization
	void Start () 
	{
		rb2D = GetComponent<Rigidbody2D> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		initPosition = rb2D.position;
		sqrShakeCountNeeded = shakeCountNeeded * shakeCountNeeded;
	}

	void OnMouseDown()
	{
		//Debug.Log ("Mouse Hold!");
        if(!L_GameManager.instance.isStirring)
        {
            isMouseClicking = true;
        }
	}

	// Update is called once per frame
	void Update () 
	{
        if (L_GameManager.instance.GetIsStartBattle())
        {
            if (isMouseClicking)
            {
                if (!Input.GetMouseButton(0) || L_GameManager.instance.isStirring)
                {
                    isMouseClicking = false;
                    rb2D.MovePosition(initPosition);
                    tempSqrShakeCount = 0f;
                    Debug.Log("Mouse release! or start stirring");
                }
                else
                {
                    Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    tempSqrShakeCount = tempSqrShakeCount + Mathf.Abs((new Vector2(newPosition.x, newPosition.y) - new Vector2(positionLastFrame.x, positionLastFrame.y)).sqrMagnitude);
                    //Debug.Log (tempSqrShakeCount + ", LAST" + positionLastFrame + ", this " + newPosition);
                    if (tempSqrShakeCount >= sqrShakeCountNeeded)
                    {
                        tempSqrShakeCount = 0;
                        //Debug.Log (tempSqrShakeCount);
                        DropItem();
                    }
                    rb2D.MovePosition(newPosition);
                }

                positionLastFrame = rb2D.position;
            }
        }
		
	}

	void DropItem()
	{
		Vector3 spawnPosition = transform.position;
		Instantiate (dropItem, rb2D.position, dropItem.transform.rotation);
	}
}
