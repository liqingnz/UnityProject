using UnityEngine;
using System.Collections.Generic;


// Used for graph/path finding
public class Node {
    public List<Node> neighbours;
    public int x;
    public int y;

    public Node() {
        neighbours = new List<Node>();
    }

    public Node(Node[,] graph, int x, int y) {
        int mapWidth = GameManager.boardWidth;
        this.x = x;
        this.y = y;
        neighbours = new List<Node>();

        if (x>0) {
            graph[x-1, y].neighbours.Add(this);
            neighbours.Add(graph[x-1, y]);
        }
        if (y > 0) {
            if(y%2 == 1) { // odd row
                graph[x, y-1].neighbours.Add(this);
                neighbours.Add(graph[x, y-1]);
                if (x < mapWidth-1) { // check if on right edge
                    graph[x + 1, y - 1].neighbours.Add(this);
                    neighbours.Add(graph[x + 1, y - 1]);
                }
            } else { // even row
                graph[x, y-1].neighbours.Add(this);
                neighbours.Add(graph[x, y-1]);
                if (x > 0) { // check if on left edge
                    graph[x-1, y - 1].neighbours.Add(this);
                    neighbours.Add(graph[x-1, y - 1]);
                }
            }
        }
    }
}
