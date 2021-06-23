using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_KillZone : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D other)
	{
        if(other.gameObject.tag == "DropItem")
        {
            Destroy(other.gameObject);

        }
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
