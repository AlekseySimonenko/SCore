using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOnStart : MonoBehaviour {

    public bool activeOnStart = true;

	// Use this for initialization
	void Start () {
        gameObject.SetActive(activeOnStart);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
