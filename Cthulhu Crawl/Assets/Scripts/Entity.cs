using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
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
    public string EntityName { get; private set; }

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
        EntityName = entitySO.name;

        x = startingPosition.Item1;
        y = startingPosition.Item2;
        transform.position = new Vector3(x, y, 0);
    }

    public void ActInDirection(Direction direction)
    {
        if (TryGetComponent(out Mover mover) == false)
        {
            return;
        }

        if (TryGetComponent(out Fighter fighter) == false)
        {
            return;
        }

        mover.TryMoveInDirection(direction);

        fighter.MeleeAction(direction);
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

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

}
