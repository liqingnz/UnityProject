using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inside MapHolder
[System.Serializable]
public class TileInfo {
    //public int xCoord;
    //public int yCoord;

    // What is it called
    public string tileType;

    // Texture of specific terrain
    public GameObject tileVisualPrefab;

    // Might have terrain elevation
    // public float height = 0;

    // Movement Cost
    public int movementCost = 1;

    // Can a unit walk on this tile
    public bool isWalkable = true;

    
}
