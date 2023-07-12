using System;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public Action<Fighter> cbOnCurrentHealthChanged;
    public Action<Fighter> cbOnMaxHealthChanged;

    private int maxHealth;
    public int MaxHealth
    {
        get => maxHealth;
        private set
        {
            maxHealth = value;
            cbOnMaxHealthChanged?.Invoke(this);
        }
    }

    public int Defense { get; private set; }
    public int Power { get; private set; }

    private int currentHealth = 1;
    public int CurrentHealth
    {
        get => currentHealth;
        private set
        {
            if (currentHealth > MaxHealth)
            {
                value = MaxHealth;
            }
            else if (value <= 0)
            {
                Die();
            }

            int change = Mathf.Abs(currentHealth - value);
            currentHealth = value;
            if (change != 0)
            {
                cbOnCurrentHealthChanged?.Invoke(this);
            }
        }
    }

    private Entity entity;

    public void Init(FighterSO fighterSO, Entity entity)
    {
        MaxHealth = fighterSO.maxHealth;
        CurrentHealth = MaxHealth;
        Defense = fighterSO.defense;
        Power = fighterSO.power;
        this.entity = entity;
    }

    public bool TryMeleeAction(Direction direction)
    {
        (int dx, int dy) = GameMap.ConvertDirectionToDeltaCoord(direction);
        (int x, int y) = entity.GetPosition();
        int targetX = x + dx;
        int targetY = y + dy;

        // Check if entity is in tile
        List<Entity> targetTileEntities =
            entity.entityManager.GetEntityAtLocation(targetX, targetY);
        if (targetTileEntities == null) { return false; }

        // Attack
        for (int i = 0; i < targetTileEntities.Count; i++)
        {
            if (targetTileEntities[i].TryGetComponent(
                out Fighter otherFighter))
            {
                otherFighter.Damage(this);
                return true;
            }
        }

        return false;
    }

    public void Damage(Fighter otherFighter)
    {
        int damage = otherFighter.Power - Defense;

        if (damage > 0)
        {
            DisplayMessageSystem.Instance.DisplayMessage(
                otherFighter.entity.EntityName
                + " attacks "
                + entity.EntityName
                + " for "
                + damage.ToString()
                + " damage.",
                ColorPalette.r2);
            CurrentHealth -= damage;
        }
        else
        {
            DisplayMessageSystem.Instance.DisplayMessage(
                "No damage done.",
                ColorPalette.b2);
        }
    }

    public int Heal(int amount)
    {
        if (CurrentHealth == MaxHealth)
        {
            DisplayMessageSystem.Instance.DisplayMessage(
                "Health is already full.",
                ColorPalette.r3);
            return 0;
        }

        int amountRecovered;
        if (CurrentHealth + amount > MaxHealth)
        {
            amountRecovered = MaxHealth - CurrentHealth;
        }
        else
        {
            amountRecovered = Mathf.Min(
                MaxHealth - CurrentHealth,
                amount);
        }

        CurrentHealth += amountRecovered;
        return amountRecovered;
    }

    public void Die()
    {
        if (TryGetComponent(out AI ai))
        {
            DisplayMessageSystem.Instance.DisplayMessage(
                entity.EntityName + " is dead",
                ColorPalette.r2);
            Destroy(ai);
        }
        else if (TryGetComponent(out PlayerController pc))
        {
            DisplayMessageSystem.Instance.DisplayMessage(
                "You died",
                ColorPalette.r2);
            Destroy(pc);
        }

        EntityDatabase database =
            FindAnyObjectByType<EntityDatabase>();
        entity.Init(
            database.dead,
            entity.GetPosition(),
            entity.Map,
            entity.entityManager);
        Destroy(this);
    }

    public void RegisterOnCurrentHealthChanged(
        Action<Fighter> callbackfunc)
    {
        cbOnCurrentHealthChanged += callbackfunc;
    }

    public void UnregisterOnCurrentHealthChanged(
        Action<Fighter> callbackfunc)
    {
        cbOnCurrentHealthChanged -= callbackfunc;
    }

    public void RegisterOnMaxHealthChanged(
        Action<Fighter> callbackfunc)
    {
        cbOnMaxHealthChanged += callbackfunc;
    }

    public void UnregisterOnMaxHealthChanged(
        Action<Fighter> callbackfunc)
    {
        cbOnMaxHealthChanged -= callbackfunc;
    }
}
