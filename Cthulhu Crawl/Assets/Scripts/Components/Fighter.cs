using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int MaxHealth { get; private set; }
    public int Defense { get; private set; }
    public int Power { get; private set; }

    private int currentHealth;
    public int CurrentHealth
    {
        get => currentHealth;
        private set
        {
            if (currentHealth > MaxHealth)
            {
                value = MaxHealth;
            }
            else if (currentHealth < 0)
            {
                value = 0;
            }
            currentHealth = value;
        }
    }

    private Entity entity;

    public void Init(
        int maxHealth, int currentHealth,
        int defense, int power, Entity entity)
    {
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
        Defense = defense;
        Power = power;
        this.entity = entity;
    }

    public bool TryMeleeAction(Direction direction)
    {
        (int dx, int dy) = GameMap.ConvertDirectionToDeltaCoord(direction);
        (int x, int y) = entity.GetPosition();
        int targetX = x + dx;
        int targetY = y + dy;

        // Check if entity is in tile
        Entity targetTileEntity =
            entity.entityManager.GetEntityAtLocation(targetX, targetY);
        if (targetTileEntity == null) { return false; }

        return true;
    }

}
