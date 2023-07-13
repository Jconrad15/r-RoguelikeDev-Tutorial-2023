using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Entity entity;
    private static readonly float centerPoint = 32.25f;
    private static readonly float leftPointX = 30.985f;
    private static readonly float topRowY = 16f;
    private static readonly float rowOffsetY = 1.22f;
    private int capacity = 9;
    private (float, float)[] inventoryLocations = new (float, float)[9]
    {
        (leftPointX, topRowY),
        (centerPoint, topRowY),
        (centerPoint - leftPointX + centerPoint, topRowY),
        (leftPointX, topRowY - rowOffsetY),
        (centerPoint, topRowY - rowOffsetY),
        (centerPoint - leftPointX + centerPoint, topRowY - rowOffsetY),
        (leftPointX, topRowY - rowOffsetY- rowOffsetY),
        (centerPoint, topRowY - rowOffsetY- rowOffsetY),
        (centerPoint - leftPointX + centerPoint, topRowY - rowOffsetY- rowOffsetY)
    };
    private List<Entity> entityItems;

    public void Init(Entity entity)
    {
        this.entity = entity;
        entityItems = new List<Entity>();
    }

    public void Drop(Entity entityItem)
    {
        if (entityItems.Contains(entityItem) == false)
        {
            Debug.LogError("inventory does not have item");
            return;
        }

        entityItems.Remove(entityItem);

        entityItem.PlaceOnMapAtLocation(entity.GetPosition());
        DisplayMessageSystem.Instance.DisplayMessage(
            "You dropped the " + entityItem.EntityName,
            ColorPalette.b1);

        UpdateInventoryUI();
    }

    public void Remove(Entity entityItem)
    {
        entityItems.Remove(entityItem);
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < entityItems.Count; i++)
        {
            entityItems[i].PlaceOnUIAtLocation(
                inventoryLocations[i]);
        }
    }

    public bool TryPickupItem()
    {
        if (entityItems.Count >= capacity)
        {
            DisplayMessageSystem.Instance.DisplayMessage(
                "Inventory full.",
                ColorPalette.r2);
            return false;
        }

        // Check for item at location
        List<Entity> tileEntities = entity.entityManager
            .GetEntityAtLocation(entity.GetPosition());

        if (tileEntities == null)
        {
            return false;
        }

        for (int i = 0; i < tileEntities.Count; i++)
        {
            if (tileEntities[i].TryGetComponent(out Item item))
            {
                tileEntities[i].PlaceOnUIAtLocation(
                    inventoryLocations[entityItems.Count]);
                entityItems.Add(tileEntities[i]);
                item.MoveToInventory();
                return true;
            }
        }

        return false;
    }

}
