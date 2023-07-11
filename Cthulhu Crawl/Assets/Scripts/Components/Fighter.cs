using UnityEditor.SceneManagement;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int MaxHealth { get; private set; }
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
            else if (currentHealth <= 0)
            {
                value = 0;
                Die();
            }
            currentHealth = value;
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
        if (targetTileEntity.TryGetComponent(out Fighter f))
        {
            f.Damage(this);
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
            CurrentHealth -= damage;
            Debug.Log(
                otherFighter.entity.EntityName
                + " attacks "
                + entity.EntityName
                + " for "
                + damage.ToString()
                + " damage.");
        }
        else
        {
            Debug.Log("No damage done.");
        }
    }

    public void Die()
    {
        if (TryGetComponent(out AI ai))
        {
            Debug.Log(entity.EntityName + " is dead");
            Destroy(ai);
        }
        else if (TryGetComponent(out PlayerController pc))
        {
            Debug.Log("You died");
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

}
