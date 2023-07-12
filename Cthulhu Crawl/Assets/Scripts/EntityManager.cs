using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField]
    private Entity entityPrefab;
    [SerializeField]
    private SpriteDatabase spriteDatabase;
    private List<Entity> entities;
    public Entity Player { get; private set; }
    private GameMap gameMap;
    private EntityDatabase entityDatabase;
    private TurnManager turnManager;

    public void InitializeEntities(GameMap gameMap, int seed)
    {
        turnManager = FindAnyObjectByType<TurnManager>();
        turnManager.RegisterOnStartAITurn(OnStartAITurn);

        this.gameMap = gameMap;
        entityDatabase = FindAnyObjectByType<EntityDatabase>();
        entities = new List<Entity>();
        CreatePlayer(gameMap);
        CreateCharacterEntities(gameMap, seed);
        CreateItemEntities(gameMap, seed);
    }

    private void CreatePlayer(GameMap gameMap)
    {
        Player = Instantiate(entityPrefab, transform);
        Player.name = "Player";
        EntitySO playerEntitySO = entityDatabase.player;
        Player.Init(
            playerEntitySO,
            gameMap.startingPosition,
            gameMap,
            this,
            true);

        Mover m = Player.AddComponent<Mover>();
        m.Init(Player);

        Fighter f = Player.AddComponent<Fighter>();
        f.Init(playerEntitySO.fighterSO, Player);

        Inventory inventory = Player.AddComponent<Inventory>();
        inventory.Init(Player);

        PlayerController pc = Player.AddComponent<PlayerController>();
        pc.Init();

        entities.Add(Player);
    }

    private void CreateCharacterEntities(GameMap gameMap, int seed)
    {
        for (int i = 1; i < gameMap.rooms.Count; i++)
        {
            (int, int) pos = gameMap.rooms[i].GetRandomPosition(seed);

            Entity entity = Instantiate(entityPrefab, transform);
            entity.name = "Character Entity #" + i.ToString();
            EntitySO selectedEntitySO;
            if (Random.value > 0.8f)
            {
                selectedEntitySO = entityDatabase.demon;
            }
            else
            {
                selectedEntitySO = entityDatabase.cultist;
            }

            entity.Init(
                selectedEntitySO,
                pos,
                gameMap,
                this);

            if (selectedEntitySO.fighterSO != null)
            {
                Fighter f = entity.AddComponent<Fighter>();
                f.Init(selectedEntitySO.fighterSO, entity);
            }

            AI ai = entity.AddComponent<AI>();
            ai.Init(entity);
            Mover m = entity.AddComponent<Mover>();
            m.Init(entity);

            entities.Add(entity);
        }
    }

    private void CreateItemEntities(GameMap gameMap, int seed)
    {
        for (int i = 1; i < gameMap.rooms.Count; i++)
        {
            (int, int) pos = gameMap.rooms[i].GetRandomPosition(seed);

            Entity itemEntity = Instantiate(entityPrefab, transform);
            itemEntity.name = "Item Entity #" + i.ToString();

            EntitySO selectedEntitySO = entityDatabase.HealingPotion;
            itemEntity.Init(
                selectedEntitySO,
                pos,
                gameMap,
                this);
            entities.Add(itemEntity);
        }
    }

    public void UpdateEntityVisibility()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].UpdateVisibilityColor();
        }
    }

    public List<Entity> GetEntityAtLocation(int targetX, int targetY)
    {
        List<Entity> foundEntities = new List<Entity>();
        for (int i = 0; i < entities.Count; i++)
        {
            (int x, int y) = entities[i].GetPosition();
            if (x == targetX &&
                y == targetY)
            {
                foundEntities.Add(entities[i]);
            }
        }

        return foundEntities;
    }

    public List<Entity> GetEntityAtLocation((int, int) targetPos)
    {
        return GetEntityAtLocation(targetPos.Item1, targetPos.Item2);
    }

    private void OnStartAITurn()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            Entity e = entities[i];
            if (e.TryGetComponent(out AI AI) == false) { continue; }
            AI.Act();
        }

        turnManager.AIEndTurn();
    }

}
