using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePenaltyModifier : GraphModifier
{
    public bool ApplyOnScan = true;
    public Tilemap tilemap;

    public override void OnPostScan()
    {
        if (ApplyOnScan)
        {
            ScanGraph();
        }
    }

    private void ScanGraph()
    {
        if (AstarPath.active == null)
        {
            Debug.LogError("There is no AstarPath object in the scene", this);
            return;
        }

        AstarPath.active.AddWorkItem(new AstarWorkItem(ctx =>
        {
            GridGraph gridGraph = AstarPath.active.data.gridGraph;
            for (int z = 0; z < gridGraph.depth; z++)
            {
                for (int x = 0; x < gridGraph.width; x++)
                {
                    GraphNode node = gridGraph.GetNode(x, z);
                    var v3 = Vector3Int.FloorToInt((Vector3)node.position);
                    var tile = tilemap.GetTile(v3);
                    if (tile != null)
                    {
                        node.Tag = 1;
                    }
                }
            }

        }));
    }
}