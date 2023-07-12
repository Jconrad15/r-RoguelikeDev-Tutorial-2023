using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private Entity entity;

    public void Init(Entity entity)
    {
        this.entity = entity;
    }

    public void Act()
    {
        Debug.Log(entity.EntityName + " acts now.");

        // This can be changed to alter the AI behavior
        Tile currentTile = GetLocalTile();
        if (currentTile == null)
        {
            Debug.LogError("Entity on a null tile, skipping turn.");
            return;
        }

        if (currentTile.visibility != TileVisibility.Visible)
        {
            Debug.Log("Entity not visible, choosing to stay put.");
            return;
        }

        if (TryGetComponent(out Mover mover))
        {
            Tile playerTile = GetPlayerTile();
            mover.GeneratePathToTargetTile(playerTile);
            if (mover.TryMoveToNextTile())
            {
                Debug.Log("AI moved");
                return;
            }
        }

        if (TryGetComponent(out Fighter fighter))
        {
            (bool attackableNeighbor, Direction d)
                = TryGetAttackableNeighbor();
            if (attackableNeighbor)
            {
                if (fighter.TryMeleeAction(d))
                {
                    return;
                }
            }
        }
        Debug.Log("AI did nothing");
    }

    private (bool, Direction) TryGetAttackableNeighbor()
    {
        foreach (Direction d in
            (Direction[])Enum.GetValues(typeof(Direction)))
        {
            (int dx, int dy) = GameMap.ConvertDirectionToDeltaCoord(d);
            (int x, int y) = entity.GetPosition();
            int targetX = x + dx;
            int targetY = y + dy;

            // Check if entity is in tile
            List<Entity> targetEntities =
                entity.entityManager.GetEntityAtLocation(targetX, targetY);
            if (targetEntities != null)
            {
                for (int i = 0; i < targetEntities.Count; i++)
                {
                    if (targetEntities[i].IsPlayer)
                    {
                        return (true, d);
                    }
                }
            }
        }

        return (false, Direction.N);
    }

    private Tile GetPlayerTile()
    {
        return entity.Map.TryGetTileAtCoord(
            entity.entityManager.Player.GetPosition());
    }

    private Tile GetLocalTile()
    {
        return entity.Map.TryGetTileAtCoord(entity.GetPosition());
    }

}
