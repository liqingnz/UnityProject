using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder {
    Node[,] Graph;
    MapGenerator map;
    List<GameObject> highLightedTiles;

    public PathFinder(int mapLength, int mapWidth) {
        Graph = new Node[mapLength, mapWidth];
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapGenerator>();

        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapWidth; y++) {
                Graph[x, y] = new Node(Graph, x, y);
            }
        }
    }


    public void PathHighLight(GameObject selectedUnit) {
        highLightedTiles = new List<GameObject>();
        Dictionary<Node, float> dist = new Dictionary<Node, float>();

        // Setup the "Q" -- the list of nodes we haven't checked yet.
        List<Node> unvisited = new List<Node>();

        Node source = Graph[selectedUnit.GetComponent<Unit>().tileX, selectedUnit.GetComponent<Unit>().tileY];

        dist[source] = 0;

        /*  Initialize everything to have INFINITY distance, since
            we don't know any better right now. Also, it's possible
            that some nodes CAN'T be reached from the source,
            which would make INFINITY a reasonable value */
        foreach (Node v in Graph) {
            if (v != source) {
                dist[v] = Mathf.Infinity;
            }

            unvisited.Add(v);
        }

        while (unvisited.Count > 0) {
            // "u" is going to be the unvisited node with the smallest distance.
            Node u = null;

            foreach (Node possibleU in unvisited) {
                if (u == null || dist[possibleU] < dist[u]) {
                    u = possibleU;
                }
            }

            unvisited.Remove(u);

            foreach (Node v in u.neighbours) {
                float alt = dist[u] + map.CostToEnterTile(v.x, v.y);
                if (alt < dist[v]) {
                    dist[v] = alt;
                    if (alt <= selectedUnit.GetComponent<Unit>().remainingMovementPoints) {
                        GameObject highLightedTile = map.CoordToTileObject(v.x, v.y);
                        highLightedTile.GetComponent<Renderer>().material.color = highLightedTile.GetComponent<Hightlight>().pathHighLight;
                        highLightedTile.GetComponent<Hightlight>().currentColor = highLightedTile.GetComponent<Hightlight>().pathHighLight;
                        highLightedTiles.Add(highLightedTile);
                    }
                }
            }
        }
    }

}

