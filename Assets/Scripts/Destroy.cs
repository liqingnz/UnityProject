using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {
    [SerializeField]
    public float destryDelay = 1f;
	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, destryDelay);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
