using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Entity entity;

    private int capacity = 9;
    private (int, int)[] inventoryLocations = new (int, int)[9]
    {
        (31, 16),
        (32, 16),
        (33, 16),
        (31, 15),
        (32, 15),
        (33, 15),
        (31, 14),
        (32, 14),
        (33, 14)
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
            items[i].PlaceOnMapAtLocation(
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
                tileEntities[i].PlaceOnMapAtLocation(
                    inventoryLocations[items.Count]);
                items.Add(tileEntities[i]);
                return true;
            }
        }

        return false;
    }

}
