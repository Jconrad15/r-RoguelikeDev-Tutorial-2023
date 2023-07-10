using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private Path_AStar path;
    private Entity entity;

    public void Init(Entity entity)
    {
        this.entity = entity;
    }

    public void PerfromAction()
    {

    }

    public void GeneratePathToPlayer(Tile targetTile)
    {
        (int x, int y) = entity.GetPosition();
        Tile currentTile = entity.Map.TryGetTileAtCoord(x, y);

        path = new Path_AStar(
            currentTile, targetTile,
            entity.Map, entity.entityManager);
    }

}
