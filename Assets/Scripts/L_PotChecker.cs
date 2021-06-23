using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_PotChecker : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D other)
	{
		L_DropItem dropItemComp = other.gameObject.GetComponent<L_DropItem> ();

		if (dropItemComp != null) 
		{
			L_GameManager.instance.AddItemInPool (dropItemComp.dropItemType, dropItemComp.pointPerItem);
			Destroy (dropItemComp.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
