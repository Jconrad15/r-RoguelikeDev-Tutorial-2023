using System;
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

            int change = currentHealth - value;
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
        Entity targetTileEntity =
            entity.entityManager.GetEntityAtLocation(targetX, targetY);
        if (targetTileEntity == null) { return false; }

        // Attack
        if (targetTileEntity.TryGetComponent(out Fighter otherFighter))
        {
            otherFighter.Damage(this);
            return true;
        }
        else
        {
            Debug.LogWarning("Other entity is not a fighter");
            return false;
        }
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
                + " damage.");
            CurrentHealth -= damage;
        }
        else
        {
            DisplayMessageSystem.Instance.DisplayMessage(
                "No damage done.");
        }
    }

    public void Die()
    {
        if (TryGetComponent(out AI ai))
        {
            DisplayMessageSystem.Instance.DisplayMessage(
                entity.EntityName + " is dead");
            Destroy(ai);
        }
        else if (TryGetComponent(out PlayerController pc))
        {
            DisplayMessageSystem.Instance.DisplayMessage(
                "You died");
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
