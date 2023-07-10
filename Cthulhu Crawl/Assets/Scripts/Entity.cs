using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private Action<Entity> cbOnEndMove;

    private int movementIncrements = 30;

    [SerializeField]
    private SpriteRenderer entitySR;
    [SerializeField]
    private SpriteRenderer backgroundSR;

    public EntityManager entityManager;

    private int x;
    private int y;
    private Color color;
    public bool IsPlayer { get; private set; }
    public bool BlocksMovement { get; private set; }
    public GameMap Map { get; private set; }
    private string entityName;

    private bool isMoving = false;
    private Vector2 currentTarget;

    public void Init(
        EntitySO entitySO, (int, int) startingPosition, 
        GameMap map, EntityManager entityManager,
        bool isPlayer = false)
    {
        this.entityManager = entityManager;
        Map = map;
        IsPlayer = isPlayer;
        entitySR.sprite = entitySO.sprite;
        color = entitySO.color;
        entitySR.color = color;
        BlocksMovement = entitySO.blocksMovement;
        entityName = entitySO.name;

        x = startingPosition.Item1;
        y = startingPosition.Item2;
        transform.position = new Vector3(x, y, 0);

        currentTarget = new Vector2(x, y);
    }

    public void SetStartingPosition((int, int) pos)
    {
        x = pos.Item1;
        y = pos.Item2;
        SetToTargetPos();
    }

    public void ActInDirection(Direction direction)
    {
        int dx = 0;
        int dy = 0;

        switch (direction)
        {
            case Direction.N:
                dx = 0;
                dy = 1;
                break;
            case Direction.E:
                dx = 1;
                dy = 0;
                break;
            case Direction.S:
                dx = 0;
                dy = -1;
                break;
            case Direction.W:
                dx = -1;
                dy = 0;
                break;
        }

        if (isMoving)
        {
            SetToTargetPos();
            StopAllCoroutines();
        }

        int targetX = x + dx;
        int targetY = y + dy;
        if (Map.IsWalkable(targetX, targetY) == false) { return; }

        Entity targetTileEntity = 
            entityManager.GetEntityAtLocation(targetX, targetY);
        if (targetTileEntity == null)
        {
            MovementAction(targetX, targetY);
        }
        else
        {
            MeleeAction(targetTileEntity);
        }

    }

    private void MeleeAction(Entity targetTileEntity)
    {
        Debug.Log("You kick the " + targetTileEntity.entityName);
    }

    private void MovementAction(int targetX, int targetY)
    {
        currentTarget = new Vector2(targetX, targetY);
        x = (int)currentTarget.x;
        y = (int)currentTarget.y;

        StartCoroutine(LerpMove(currentTarget));
    }

    private IEnumerator LerpMove(Vector2 target)
    {
        isMoving = true;
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
        x = (int)currentTarget.x;
        y = (int)currentTarget.y;
        transform.position = currentTarget;
        isMoving = false;
        cbOnEndMove?.Invoke(this);
    }

    public void UpdateVisibilityColor()
    {
        TileVisibility v = Map.tiles[Map.GetIndex(x, y)].visibility;

        Color selectedColor = color;
        Color bgColor = backgroundSR.color;
        if (v == TileVisibility.NotVisible ||
            v == TileVisibility.PreviouslySeen)
        {
            selectedColor.a = 0;
            entitySR.color = selectedColor;
            bgColor.a = 0;
            backgroundSR.color = bgColor;
        }
        else
        {
            entitySR.color = selectedColor;
            bgColor.a = 255;
            backgroundSR.color = bgColor;
        }
    }

    public (int, int) GetPosition()
    {
        return (x, y);
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
