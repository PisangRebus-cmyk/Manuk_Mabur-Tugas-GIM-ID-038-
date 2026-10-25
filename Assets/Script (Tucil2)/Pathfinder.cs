using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public static Pathfinder Instance { get; private set; }

    [Header("Grid area (bottom-left corner and size)")]
    [SerializeField] private Vector2 origin = new Vector2(-15f, -6f);
    [SerializeField] private int width = 60;
    [SerializeField] private int height = 24;
    [SerializeField] private float cellSize = 0.5f;

    [Header("What blocks the path")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private bool showGizmos = true;

    private class Node
    {
        public int x, y;
        public bool walkable;
        public int g, h;
        public Node parent;
        public int F => g + h;
    }

    private Node[,] grid;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BuildGrid();   // Start, so all colliders already exist
    }

    private void BuildGrid()
    {
        grid = new Node[width, height];
        Vector2 box = Vector2.one * cellSize * 0.9f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool blocked = Physics2D.OverlapBox(CellCenter(x, y), box, 0f, obstacleLayer);
                grid[x, y] = new Node { x = x, y = y, walkable = !blocked };
            }
        }
    }

    private Vector2 CellCenter(int x, int y)
    {
        return origin + new Vector2((x + 0.5f) * cellSize, (y + 0.5f) * cellSize);
    }

    private Node NodeFromWorld(Vector2 pos)
    {
        int x = Mathf.FloorToInt((pos.x - origin.x) / cellSize);
        int y = Mathf.FloorToInt((pos.y - origin.y) / cellSize);
        if (x < 0 || y < 0 || x >= width || y >= height) return null;
        return grid[x, y];
    }

    
    private Node NearestWalkable(Node node)
    {
        if (node == null || node.walkable) return node;
        for (int r = 1; r <= 2; r++)
            for (int dx = -r; dx <= r; dx++)
                for (int dy = -r; dy <= r; dy++)
                {
                    int nx = node.x + dx, ny = node.y + dy;
                    if (nx < 0 || ny < 0 || nx >= width || ny >= height) continue;
                    if (grid[nx, ny].walkable) return grid[nx, ny];
                }
        return null;
    }

    public List<Vector2> FindPath(Vector2 startWorld, Vector2 endWorld)
    {
        List<Vector2> result = new List<Vector2>();
        if (grid == null) return result;

        Node start = NearestWalkable(NodeFromWorld(startWorld));
        Node end = NearestWalkable(NodeFromWorld(endWorld));
        if (start == null || end == null) return result;

        foreach (Node n in grid) { n.g = 0; n.h = 0; n.parent = null; }

        List<Node> open = new List<Node> { start };
        HashSet<Node> closed = new HashSet<Node>();

        while (open.Count > 0)
        {
            Node current = open[0];
            for (int i = 1; i < open.Count; i++)
                if (open[i].F < current.F || (open[i].F == current.F && open[i].h < current.h))
                    current = open[i];

            open.Remove(current);
            closed.Add(current);

            if (current == end)
                return Retrace(start, end);

            foreach (Node neighbour in GetNeighbours(current))
            {
                if (!neighbour.walkable || closed.Contains(neighbour)) continue;

                int newG = current.g + Distance(current, neighbour);
                bool inOpen = open.Contains(neighbour);

                if (newG < neighbour.g || !inOpen)
                {
                    neighbour.g = newG;
                    neighbour.h = Distance(neighbour, end);
                    neighbour.parent = current;
                    if (!inOpen) open.Add(neighbour);
                }
            }
        }
        return result;   
    }

    private List<Vector2> Retrace(Node start, Node end)
    {
        List<Vector2> path = new List<Vector2>();
        Node current = end;
        while (current != start)
        {
            path.Add(CellCenter(current.x, current.y));
            current = current.parent;
        }
        path.Reverse();
        return path;
    }

    private List<Node> GetNeighbours(Node node)
    {
        List<Node> list = new List<Node>();
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                int nx = node.x + dx, ny = node.y + dy;
                if (nx < 0 || ny < 0 || nx >= width || ny >= height) continue;

                if (dx != 0 && dy != 0 &&
                    (!grid[node.x + dx, node.y].walkable || !grid[node.x, node.y + dy].walkable))
                    continue;

                list.Add(grid[nx, ny]);
            }
        }
        return list;
    }

    private int Distance(Node a, Node b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return 14 * Mathf.Min(dx, dy) + 10 * Mathf.Abs(dx - dy);
    }

    
}