using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inside each tile object
public class Hightlight : MonoBehaviour {
    public int tileX;
    public int tileY;

    public Color hightlightColor = Color.green;
    public Color restrictedColor = Color.red;
    public Color pathHighLight = Color.blue;

    public bool restricted = false;
    public Color originalColor;
    public Color currentColor;

    Renderer tileMaterial;
    // Use this for initialization
    void Start() {
        originalColor = GetComponent<Renderer>().material.color;
        tileMaterial = GetComponent<Renderer>();
        currentColor = originalColor;
    }


    private void OnMouseOver() {
        if (restricted) {
            tileMaterial.material.color = restrictedColor;
        } else {
            tileMaterial.material.color = hightlightColor;
        }
    }

    private void OnMouseExit() {
        tileMaterial.material.color = currentColor;
    }
}