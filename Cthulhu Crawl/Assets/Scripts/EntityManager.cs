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

    public void InitializeEntities(GameMap gameMap, int seed)
    {
        this.gameMap = gameMap;
        entityDatabase = FindAnyObjectByType<EntityDatabase>();
        entities = new List<Entity>();
        CreatePlayer(gameMap);
        CreateEntities(gameMap, seed);
    }

    private void CreatePlayer(GameMap gameMap)
    {
        Player = Instantiate(entityPrefab, transform);
        Player.Init(
            entityDatabase.player,
            gameMap.startingPosition,
            gameMap,
            this,
            true);
        _ = Player.AddComponent<PlayerController>();
        entities.Add(Player);
    }

    private void CreateEntities(GameMap gameMap, int seed)
    {
        for (int i = 1; i < gameMap.rooms.Count; i++)
        {
            (int, int) pos = gameMap.rooms[i].GetRandomPosition(seed);

            Entity entity = Instantiate(entityPrefab, transform);

            if (Random.value > 0.8f)
            {
                entity.Init(
                    entityDatabase.demon,
                    pos,
                    gameMap,
                    this);
            }
            else
            {
                entity.Init(
                    entityDatabase.cultist,
                    pos,
                    gameMap,
                    this);
            }

            // Add AI component to the entity
            AI ai = entity.AddComponent<AI>();
            ai.Init(entity);

            entities.Add(entity);
        }
    }

    public void UpdateEntityVisibility()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].UpdateVisibilityColor();
        }
    }

    public Entity GetEntityAtLocation(int targetX, int targetY)
    {
        for (int i = 0; i < entities.Count; i++)
        {
            (int x, int y) = entities[i].GetPosition();
            if (x == targetX &&
                y == targetY)
            {
                return entities[i];
            }
        }

        return null;
    }

    public Entity GetEntityAtLocation((int, int) targetPos)
    {
        for (int i = 0; i < entities.Count; i++)
        {
            (int x, int y) = entities[i].GetPosition();
            if (x == targetPos.Item1 &&
                y == targetPos.Item2)
            {
                return entities[i];
            }
        }

        return null;
    }

    public void HandleEntityTurns()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            Entity e = entities[i];
            if (e.TryGetComponent(out AI ai) == false) { continue; }
            Debug.Log("Entity #" + i.ToString() + "'s turn.");

            Tile playerTile = gameMap.TryGetTileAtCoord(
                Player.GetPosition());

            ai.GeneratePathToPlayer(playerTile);
        }
    }

}
