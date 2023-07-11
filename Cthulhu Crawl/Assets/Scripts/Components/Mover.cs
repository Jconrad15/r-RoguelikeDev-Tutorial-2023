using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private Action<Entity> cbOnEndMove;

    private readonly int movementIncrements = 30;
    public bool IsMoving { get; private set; } = false;
    private Vector2 currentTarget;
    private Entity entity;
    private Path_AStar path;
    public Tile NextTile { get; private set; }

    public void Init(Entity entity)
    {
        this.entity = entity;
    }

    public bool TryMoveInDirection(Direction direction)
    {
        (int dx, int dy) = GameMap.ConvertDirectionToDeltaCoord(direction);

        if (IsMoving) { QuickFinishMovement(); }

        (int x, int y) = entity.GetPosition();
        int targetX = x + dx;
        int targetY = y + dy;
        
        // Check if tile is walkable
        if (entity.Map.IsWalkable(targetX, targetY) == false)
        {
            return false;
        }

        // Check if entity is in tile
        Entity targetTileEntity =
            entity.entityManager.GetEntityAtLocation(targetX, targetY);
        if (targetTileEntity != null)
        {
            if (targetTileEntity.BlocksMovement)
            {
                return false;
            }
        }

        MovementAction(targetX, targetY);
        return true;
    }

    public bool TryMoveToNextTile()
    {
        if (IsMoving) { QuickFinishMovement(); }

        // Check if tile is walkable
        if (NextTile.isWalkable == false)
        {
            return false;
        }

        // Check if entity is in tile
        Entity targetTileEntity =
            entity.entityManager.GetEntityAtLocation(NextTile.position);
        if (targetTileEntity != null)
        {
            if (targetTileEntity.BlocksMovement)
            {
                return false;
            }
        }

        MovementAction(
            NextTile.position.Item1,
            NextTile.position.Item2);
        return true;
    }

    private void MovementAction(int targetX, int targetY)
    {
        currentTarget = new Vector2(targetX, targetY);
        entity.SetPosition((int)currentTarget.x, (int)currentTarget.y);

        if (entity.IsPlayer == false)
        {
            NextTile = path.Dequeue();
        }
        StartCoroutine(LerpMove(currentTarget));
    }

    public void QuickFinishMovement()
    {
        StopAllCoroutines();
        SetToTargetPos();
    }

    private IEnumerator LerpMove(Vector2 target)
    {
        IsMoving = true;
        Vector2 startingPos = transform.position;
        for (int i = 0; i < movementIncrements; i++)
        {
            transform.position =
                Vector2.Lerp(
                    startingPos,
                    target,
                    i / (float)movementIncrements);
            yield return new WaitForEndOfFrame();
        }

        SetToTargetPos();
    }

    private void SetToTargetPos()
    {
        entity.SetPosition((int)currentTarget.x, (int)currentTarget.y);

        transform.position = currentTarget;
        IsMoving = false;
        cbOnEndMove?.Invoke(entity);
    }

    public Tile GetNextInPath()
    {
        return path.Dequeue();
    }

    public void GeneratePathToTargetTile(Tile targetTile)
    {
        (int x, int y) = entity.GetPosition();
        Tile currentTile = entity.Map.TryGetTileAtCoord(x, y);

        path = new Path_AStar(
            currentTile, targetTile,
            entity.Map, entity.entityManager);

        // Delete the first in the path, this is the current tile
        _ = path.Dequeue();

        NextTile = path.Dequeue();
    }

    public void RegisterOnEndMove(Action<Entity> callbackfunc)
    {
        cbOnEndMove += callbackfunc;
    }

    public void UnregisterOnEndMove(Action<Entity> callbackfunc)
    {
        cbOnEndMove -= callbackfunc;
    }
}
