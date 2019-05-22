using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGenerator : MonoBehaviour {
    public int mapWidth;
    public int mapLength;

    public GameObject SelectedUnitCurrentPlayer;
    public GameObject TargetedUnitCurrentPlayer;
    public GameObject TargetedUnitEnemyPlayer;


    // tiles will be the the index of tileType depends on what has been defined
    public TileInfo[] tileTypes;
    public int[,] tiles;
    public Node[,] graph;

    public GameObject[,] allTiles;

    // Used for temperory map generation
    private Vector3 mapLocation;
	// Use this for initialization
	void Start () {
        GameManager.map = this;
        GameManager.boardWidth = mapWidth;
        GameManager.boardHeight = mapLength;

        //selectedUnit = GameObject.FindGameObjectWithTag("Player");

        allTiles = new GameObject[mapWidth, mapLength];
        GenerateMapData();
        GenerateMapVisual();
        GeneratePathfindingGraph();
    }
    


    // Assigning tile type to each tile
    void GenerateMapData()
    {
        // Allocate our map tiles
        tiles = new int[mapWidth, mapLength];

        int x, y;

        // Initialize our map tiles to be grass
        for (x = 0; x < mapWidth; x++)
        {
            for (y = 0; y < mapLength; y++)
            {
                tiles[x, y] = 0;
            }
        }

        Map1.SetUp();
        tiles = Map1.tiles;

        //// Make a big swamp area
        //for (x = 3; x <= 5; x++)
        //{
        //    for (y = 0; y < 4; y++)
        //    {
        //        tiles[x, y] = 1;
        //    }
        //}

        //// Make a u-shaped mountain range
        //tiles[5, 4] = 2;
        //tiles[6, 4] = 2;
        //tiles[7, 4] = 2;
        //tiles[8, 4] = 2;

        //tiles[4, 5] = 2;
        //tiles[4, 6] = 2;
        //tiles[8, 5] = 2;
        //tiles[8, 6] = 2;

    }

    // Based on the tile type, generate the map in Unity
    void GenerateMapVisual()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                if (y % 2 == 1)
                {
                    mapLocation = new Vector3(x + 0.5f, 0, y);
                }
                else
                {
                    mapLocation = new Vector3(x, 0, y);
                }
                TileInfo tt = tileTypes[tiles[x, y]];
                GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, mapLocation, Quaternion.Euler(90,0,0), GameObject.Find("MapHolder").transform);
                //print(x + " " + y);
                go.GetComponent<Hightlight>().tileX = x;
                go.GetComponent<Hightlight>().tileY = y;
                go.GetComponent<Hightlight>().restricted = !(tt.isWalkable);
                go.name = "Tile " + x + ", " + y + " " + tt.tileType;
                allTiles[x, y] = go;
                //allTiles[x, y].GetComponent<Hightlight>().tileX = x;
                //allTiles[x, y].GetComponent<Hightlight>().tileY = y;
                //allTiles[x, y].name = "Tile " + x + ", " + y + " " + tt.tileType;
            }
        }
    }

    //public GameObject CoordToTileObject(int x, int y)
    //{
    //    return allTiles[x, y];
    //}
    public GameObject CoordToTileObject(float x, float y)
    {
        return allTiles[Mathf.RoundToInt(x - 0.1f), Mathf.RoundToInt(y)];
    }

    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        if(y % 2 == 1)
        {
            return new Vector3(x + 0.5f, 0, y);
        }
        else
        {
            return new Vector3(x, 0, y);
        }
    }



    // -------------------------------------------------------------------------------------------------//
    // --------------------------------------------Path Finding-----------------------------------------//
    // -------------------------------------------------------------------------------------------------//

    void GeneratePathfindingGraph()
    {
        // Initialize the array
        graph = new Node[mapWidth, mapLength];

        // Initialize a Node for each spot in the array
        for (int y = 0; y < mapLength; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //print("X: " + x + " Y: " + y + " Started!");
                graph[x, y] = new Node(graph, x, y);
                //print("X: " + x + " Y: " + y + " Compleated!");
            }
        }
    }


    public bool UnitCanEnterTile(int x, int y)
    {

        // Test the unit's walk/hover/fly type against various
        // terrain flags here to see if they are allowed to enter the tile.
        if (allTiles[x, y].GetComponent<Hightlight>().restricted)
            return false;

        return tileTypes[tiles[x, y]].isWalkable;
    }

    public float CostToEnterTile(int targetX, int targetY)
    {

        TileInfo tt = tileTypes[tiles[targetX, targetY]];

        if (UnitCanEnterTile(targetX, targetY) == false)
            return Mathf.Infinity;

        float cost = tt.movementCost;

        return cost;

    }

    public List<Node> NodeWithinDistance(GameObject go, int maxDistance)
    {
        List<Node> validNodes = new List<Node>();
        Dictionary<Node, float> dist = new Dictionary<Node, float>();

        // Setup the "Q" -- the list of nodes we haven't checked yet.
        List<Node> unvisited = new List<Node>();

        Node source = graph[
                            go.GetComponent<Unit>().tileX,
                            go.GetComponent<Unit>().tileY
                            ];


        dist[source] = 0;

        // Initialize everything to have INFINITY distance, since
        // we don't know any better right now. Also, it's possible
        // that some nodes CAN'T be reached from the source,
        // which would make INFINITY a reasonable value
        foreach (Node v in graph)
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
                float alt = dist[u] + CostToEnterTile(v.x, v.y);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    if (alt <= maxDistance)
                    {
                        validNodes.Add(v);
                    }
                }
            }
        }
        return validNodes;
    }

    List<GameObject> highLightedTiles;

    public void PathHighLight()
    {
        highLightedTiles = new List<GameObject>();
        //Dictionary<Node, float> dist = new Dictionary<Node, float>();

        //// Setup the "Q" -- the list of nodes we haven't checked yet.
        //List<Node> unvisited = new List<Node>();

        //Node source = graph[
        //                    SelectedUnitCurrentPlayer.GetComponent<Unit>().tileX,
        //                    SelectedUnitCurrentPlayer.GetComponent<Unit>().tileY
        //                    ];


        //dist[source] = 0;

        //// Initialize everything to have INFINITY distance, since
        //// we don't know any better right now. Also, it's possible
        //// that some nodes CAN'T be reached from the source,
        //// which would make INFINITY a reasonable value
        //foreach (Node v in graph)
        //{
        //    if (v != source)
        //    {
        //        dist[v] = Mathf.Infinity;
        //    }

        //    unvisited.Add(v);
        //}

        //while (unvisited.Count > 0)
        //{
        //    // "u" is going to be the unvisited node with the smallest distance.
        //    Node u = null;

        //    foreach (Node possibleU in unvisited)
        //    {
        //        if (u == null || dist[possibleU] < dist[u])
        //        {
        //            u = possibleU;
        //        }
        //    }

        //    unvisited.Remove(u);

        //    foreach (Node v in u.neighbours)
        //    {
        //        float alt = dist[u] + CostToEnterTile(v.x, v.y);
        //        if (alt < dist[v])
        //        {
        //            dist[v] = alt;
        //            if (alt <= SelectedUnitCurrentPlayer.GetComponent<Unit>().remainingMovement)
        //            {
        //                GameObject highLightedTile = CoordToTileObject(v.x, v.y);
        //                highLightedTile.GetComponent<Renderer>().material.color = highLightedTile.GetComponent<Hightlight>().pathHighLight;
        //                highLightedTile.GetComponent<Hightlight>().currentColor = highLightedTile.GetComponent<Hightlight>().pathHighLight;
        //                highLightedTiles.Add(highLightedTile);
        //            }
        //        }
        //    }
        //}
        List<Node> highlightingNodes;
        highlightingNodes = NodeWithinDistance(SelectedUnitCurrentPlayer, (int)SelectedUnitCurrentPlayer.GetComponent<Unit>().remainingMovementPoints);
        foreach(Node highlightingNode in highlightingNodes)
        {
            GameObject highLightedTile = CoordToTileObject(highlightingNode.x, highlightingNode.y);
            highLightedTile.GetComponent<Renderer>().material.color = highLightedTile.GetComponent<Hightlight>().pathHighLight;
            highLightedTile.GetComponent<Hightlight>().currentColor = highLightedTile.GetComponent<Hightlight>().pathHighLight;
            highLightedTiles.Add(highLightedTile);
        }

    }

    public void ClearHighLight()
    {
        if (highLightedTiles != null)
        {
            foreach (GameObject tile in highLightedTiles)
            {
                tile.GetComponent<Hightlight>().currentColor = tile.GetComponent<Hightlight>().originalColor;
                tile.GetComponent<Renderer>().material.color = tile.GetComponent<Hightlight>().originalColor;
                //highLightedTiles.Remove(tile);
            }
        }
    }

    //public void GeneratePathTo(int x, int y)
    //{
    //    if(SelectedUnitCurrentPlayer == null)
    //    {
    //        return;
    //    }
    //    ClearHighLight();
    //    print("Finding path for " + SelectedUnitCurrentPlayer);
    //    // Clear out our unit's old path.
    //    SelectedUnitCurrentPlayer.GetComponent<Unit>().currentPath = null;

    //    if (UnitCanEnterTile(x, y) == false)
    //    {
    //        // We probably clicked on a mountain or something, so just quit out.
    //        return;
    //    }

    //    Dictionary<Node, float> dist = new Dictionary<Node, float>();
    //    Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

    //    // Setup the "Q" -- the list of nodes we haven't checked yet.
    //    List<Node> unvisited = new List<Node>();

    //    Node source = graph[
    //                        SelectedUnitCurrentPlayer.GetComponent<Unit>().tileX,
    //                        SelectedUnitCurrentPlayer.GetComponent<Unit>().tileY
    //                        ];

    //    Node target = graph[x, y];

    //    dist[source] = 0;
    //    prev[source] = null;

    //    // Initialize everything to have INFINITY distance, since
    //    // we don't know any better right now. Also, it's possible
    //    // that some nodes CAN'T be reached from the source,
    //    // which would make INFINITY a reasonable value
    //    foreach (Node v in graph)
    //    {
    //        if (v != source)
    //        {
    //            dist[v] = Mathf.Infinity;
    //            prev[v] = null;
    //        }

    //        unvisited.Add(v);
    //    }

    //    while (unvisited.Count > 0)
    //    {
    //        // "u" is going to be the unvisited node with the smallest distance.
    //        Node u = null;

    //        foreach (Node possibleU in unvisited)
    //        {
    //            if (u == null || dist[possibleU] < dist[u])
    //            {
    //                u = possibleU;
    //            }
    //        }

    //        if (u == target)
    //        {
    //            break;  // Exit the while loop!
    //        }

    //        unvisited.Remove(u);

    //        foreach (Node v in u.neighbours)
    //        {
    //            //float alt = dist[u] + u.DistanceTo(v);
    //            float alt = dist[u] + CostToEnterTile(v.x, v.y);
    //            if (alt < dist[v])
    //            {
    //                dist[v] = alt;
    //                prev[v] = u;
    //            }
    //        }
    //    }

    //    // If we get there, the either we found the shortest route
    //    // to our target, or there is no route at ALL to our target.

    //    if (prev[target] == null)
    //    {
    //        // No route between our target and the source
    //        return;
    //    }

    //    List<Node> currentPath = new List<Node>();

    //    Node curr = target;

    //    // Step through the "prev" chain and add it to our path
    //    while (curr != null)
    //    {
    //        currentPath.Add(curr);
    //        curr = prev[curr];
    //    }

    //    // Right now, currentPath describes a route from out target to our source
    //    // So we need to invert it!

    //    currentPath.Reverse();

    //    SelectedUnitCurrentPlayer.GetComponent<Unit>().currentPath = currentPath;
    //}

    public List<Node> GeneratePathTo(int x, int y, GameObject go)
    {
        if (go == null)
        {
            return null;
        }
        ClearHighLight();
        //print("Finding path for " + go);
        // Clear out our unit's old path.
        go.GetComponent<Unit>().currentPath = null;

        if (UnitCanEnterTile(x, y) == false)
        {
            // We probably clicked on a mountain or something, so just quit out.
            return null;
        }

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        // Setup the "Q" -- the list of nodes we haven't checked yet.
        List<Node> unvisited = new List<Node>();

        Node source = graph[
                            go.GetComponent<Unit>().tileX,
                            go.GetComponent<Unit>().tileY
                            ];

        Node target = graph[x, y];

        dist[source] = 0;
        prev[source] = null;

        // Initialize everything to have INFINITY distance, since
        // we don't know any better right now. Also, it's possible
        // that some nodes CAN'T be reached from the source,
        // which would make INFINITY a reasonable value
        foreach (Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
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

            if (u == target)
            {
                break;  // Exit the while loop!
            }

            unvisited.Remove(u);

            foreach (Node v in u.neighbours)
            {
                //float alt = dist[u] + u.DistanceTo(v);
                float alt = dist[u] + CostToEnterTile(v.x, v.y);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        // If we get there, the either we found the shortest route
        // to our target, or there is no route at ALL to our target.

        if (prev[target] == null)
        {
            // No route between our target and the source
            return null;
        }

        List<Node> currentPath = new List<Node>();

        Node curr = target;

        // Step through the "prev" chain and add it to our path
        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }

        // Right now, currentPath describes a route from out target to our source
        // So we need to invert it!

        currentPath.Reverse();

        go.GetComponent<Unit>().currentPath = currentPath;
        return currentPath;
    }
}
