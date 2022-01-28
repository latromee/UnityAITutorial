using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathMarker
{
    public MapLocation location;
    public float G, H, F;
    public GameObject marker;
    public PathMarker parent;

    public PathMarker(MapLocation l, float g, float h, float f, GameObject m, PathMarker p)
    {
        location = l;
        G = g;
        H = h;
        F = f;
        marker = m;
        parent = p;
    }
    public override bool Equals(object obj)
    {
        if (obj == null || !this.GetType().Equals(obj.GetType())) return false;
        else return location.Equals(((PathMarker)obj).location);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class FindPathAStar : MonoBehaviour
{
    public Maze maze;
    public Material openMaterial;
    public Material closeMaterial;

    List<PathMarker> opened = new List<PathMarker>();
    List<PathMarker> closed = new List<PathMarker>();

    public GameObject start, end, pathP;

    PathMarker goalNode, startNode, lastPos;

    bool done = false;

    void RemoveAllMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");

        foreach (GameObject marker in markers) Destroy(marker);
    }

    void BeginSearch()
    {
        done = false;
        RemoveAllMarkers();

        List<MapLocation> locations = new List<MapLocation>();
        for (int z = 1; z < maze.depth - 1; z++)
        {
            for (int x = 1; x < maze.width - 1; x++)
            {
                if (maze.map[x, z] != 1) locations.Add(new MapLocation(x, z));
            }
        }

        locations.Shuffle();

        Vector3 startLocation = new Vector3(locations[0].x * maze.scale, maze.scale, locations[0].z * maze.scale);
        startNode = new PathMarker(new MapLocation(locations[0].x, locations[0].z), 0, 0, 0,
            Instantiate(start, startLocation, Quaternion.identity), null);

        Vector3 goalLocation = new Vector3(locations[1].x * maze.scale, maze.scale, locations[1].z * maze.scale);
        goalNode = new PathMarker(new MapLocation(locations[1].x, locations[1].z), 0, 0, 0,
            Instantiate(end, goalLocation, Quaternion.identity), null);

        opened.Clear();
        closed.Clear();
        opened.Add(startNode);
        lastPos = startNode;

    }

    void Search(PathMarker thisNode)
    {
        if (thisNode.Equals(goalNode)) { done = true; return; }

        foreach (MapLocation dir in maze.directions)
        {
            MapLocation neighbour = dir + thisNode.location;
            if (maze.map[neighbour.x, neighbour.z] == 1) continue;
            if (neighbour.x < 1 || neighbour.x >= maze.width || neighbour.z < 1 || neighbour.z >= maze.depth) continue;
            if (IsClosed(neighbour)) continue;

            float G = Vector2.Distance(thisNode.location.ToVector(), neighbour.ToVector()) + thisNode.G;
            float H = Vector2.Distance(neighbour.ToVector(), goalNode.location.ToVector());
            float F = G + H;

            GameObject pathBlock = Instantiate(pathP,
                new Vector3(neighbour.x * maze.scale, 0, neighbour.z * maze.scale), Quaternion.identity);

            TextMesh[] textMeshes = pathBlock.GetComponentsInChildren<TextMesh>();
            textMeshes[0].text = "G: " + G.ToString("0.00");
            textMeshes[1].text = "H: " + H.ToString("0.00");
            textMeshes[2].text = "F: " + F.ToString("0.00");

            if (!UpdateMarker(neighbour, G, H, F, thisNode))
                opened.Add(new PathMarker(neighbour, G, H, F, pathBlock, thisNode));
        }
        opened = opened.OrderBy(p => p.F).ToList<PathMarker>();
        PathMarker pm = (PathMarker)opened.ElementAt(0);
        closed.Add(pm);

        opened.RemoveAt(0);
        pm.marker.GetComponent<Renderer>().material = closeMaterial;

        lastPos = pm;
    }

    bool UpdateMarker(MapLocation pos, float g, float h, float f, PathMarker prt)
    {
        foreach (PathMarker p in opened)
        {
            if (p.location.Equals(pos))
            {
                p.G = g;
                p.H = h;
                p.F = f;
                p.parent = prt;
                return true;
            }
        }
        return false;
    }
    bool IsClosed(MapLocation marker)
    {
        foreach (PathMarker p in closed)
        {
            if (p.location.Equals(marker)) return true;
        }
        return false;
    }

    void GetPath()
    {
        RemoveAllMarkers();
        PathMarker begin = lastPos;

        while (!startNode.Equals(begin) && begin != null) 
        {
            Instantiate(pathP, new Vector3(begin.location.x * maze.scale, 0, begin.location.z * maze.scale), 
                Quaternion.identity);
            begin = begin.parent;
        }
        Instantiate(pathP, new Vector3(startNode.location.x * maze.scale, 0, startNode.location.z * maze.scale),
              Quaternion.identity);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) BeginSearch();
        if (Input.GetKeyDown(KeyCode.O)) Search(lastPos);
        if (Input.GetKeyDown(KeyCode.M)) GetPath();
    }
}
