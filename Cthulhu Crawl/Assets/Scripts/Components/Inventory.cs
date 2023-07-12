using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<Entity> items;

    public void Init(Entity entity)
    {
        this.entity = entity;
        items = new List<Entity>();
    }

    public void Drop(Entity item)
    {
        if (items.Contains(item) == false)
        {
            Debug.LogError("inventory does not have item");
            return;
        }

        items.Remove(item);

        item.PlaceOnMapAtLocation(entity.GetPosition());
        DisplayMessageSystem.Instance.DisplayMessage(
            "You dropped the " + item.EntityName,
            ColorPalette.b1);

        for (int i = 0; i < items.Count; i++)
        {
            items[i].PlaceOnUIAtLocation(
                inventoryLocations[items.Count]);
        }
    }

    public bool TryPickupItem()
    {
        if (items.Count >= capacity)
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
            if (tileEntities[i].TryGetComponent(out Mover mover) == false)
            {
                tileEntities[i].PlaceOnUIAtLocation(
                    inventoryLocations[items.Count]);
                items.Add(tileEntities[i]);
                return true;
            }
        }

        return false;
    }

}
