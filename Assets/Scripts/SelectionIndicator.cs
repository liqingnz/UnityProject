using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIndicator : MonoBehaviour {

    MapGenerator selectedUnitHolder;
    GameObject selectedUnit;
	// Use this for initialization
	void Start () {
        selectedUnitHolder = GameObject.Find("MapHolder").GetComponent<MapGenerator>();
	}
	
	// Update is called once per frame
	void Update () {
        selectedUnit = selectedUnitHolder.SelectedUnitCurrentPlayer;
        if (selectedUnit != null)
        {
            transform.position = selectedUnit.transform.position;
        }
        else
        {
            transform.position = new Vector3(10, -3, 1);
        }
	}
}
