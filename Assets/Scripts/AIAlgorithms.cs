using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIAlgorithms {

    public static List<Node> occupiedTiles = new List<Node>();


    // Not very efficient but good enough
    public static List<Node> AvailableTiles(GameObject selectedUnit, int distance)
    {
        Node[,] Graph;
        MapGenerator map;
        //Graph = new Node[GameManager.boardWidth, GameManager.boardHeight];
        map = GameManager.map;
        Graph = map.graph;
        List<Node> availableTiles = new List<Node>();
        Dictionary<Node, float> dist = new Dictionary<Node, float>();

        // Setup the "Q" -- the list of nodes we haven't checked yet.
        List<Node> unvisited = new List<Node>();

        Node source = Graph[selectedUnit.GetComponent<Unit>().tileX, selectedUnit.GetComponent<Unit>().tileY];

        dist[source] = 0;

        /*  Initialize everything to have INFINITY distance, since
            we don't know any better right now. Also, it's possible
            that some nodes CAN'T be reached from the source,
            which would make INFINITY a reasonable value */
        foreach (Node v in Graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
            }

            unvisited.Add(v);
        }

        while (unvisited.Count > 0)
        {
            // "u" is going to be the unvisited node with the smallest distance.
            Node u = null;

            foreach (Node possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            unvisited.Remove(u);

            foreach (Node v in u.neighbours)
            {
                float alt = dist[u] + map.CostToEnterTile(v.x, v.y);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    if (alt <= distance)
                    {
                        availableTiles.Add(v);
                        //Highlight the targeting tiles, test only >>>>>>>>>>>>>>>>>>
                        //GameObject highLightedTile = map.CoordToTileObject(v.x, v.y);
                        //highLightedTile.GetComponent<Renderer>().material.color = Color.cyan;
                        //highLightedTile.GetComponent<Hightlight>().originalColor = Color.cyan;
                    }
                }
            }
        }

        // If the tile is occupied by an unit, then this tile is not available
        for(int i = 0; i < occupiedTiles.Count; ++i)
        {
            if (availableTiles.Contains(occupiedTiles[i]))
            {
                availableTiles.Remove(occupiedTiles[i]);
            }
        }

        return availableTiles;
    }




}
