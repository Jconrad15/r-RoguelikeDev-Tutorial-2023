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

    public void InitializeEntities(GameMap gameMap, int seed)
    {
        entities = new List<Entity>();

        CreatePlayer(gameMap);
        CreateEntities(gameMap, seed);
    }

    private void CreatePlayer(GameMap gameMap)
    {
        Player = Instantiate(entityPrefab, transform);
        Player.Init(
            gameMap.startingPosition,
            spriteDatabase.GetPlayerSprite(),
            ColorPalette.b1,
            gameMap,
            true);
        _ = Player.AddComponent<PlayerController>();
        entities.Add(Player);
    }

    private void CreateEntities(GameMap gameMap, int seed)
    {
        for (int i = 1; i < gameMap.rooms.Count; i++)
        {
            (int, int) pos = gameMap.rooms[i].GetRandomPosition(seed);

            Entity entity = Instantiate(
                entityPrefab, transform);
            entity.Init(
                pos,
                spriteDatabase.GetEnemySprite(),
                ColorPalette.r2,
                gameMap);
            entities.Add(entity);
        }
    }

}
