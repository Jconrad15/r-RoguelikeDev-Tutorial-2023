using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    private Entity prefab;
    [SerializeField]
    private GameObject entityGOContainer;

    private List<Entity> entities;
    private SpriteDatabase spriteDatabase;

    private GameMap gameMap;
    [SerializeField]
    private MapVisuals mapVisuals;

    private int seed;

    private void Start()
    {
        spriteDatabase = FindAnyObjectByType<SpriteDatabase>();
        entities = new List<Entity>();
        seed = Random.Range(-10000, 10000);

        InitializeMap();

        CreatePlayer();
        CreateEntities();
    }

    private void InitializeMap()
    {
        int mapWidth = 30;
        int mapHeight = 20;
        int roomMinSize = 4;
        gameMap = ProcGen.GenerateDungeon(
            mapWidth, mapHeight, roomMinSize, seed);
        mapVisuals.UpdateVisuals(gameMap);
    }

    private void CreatePlayer()
    {
        Entity player = Instantiate(prefab, entityGOContainer.transform);
        player.Init(
            gameMap.startingPosition,
            spriteDatabase.GetPlayerSprite(),
            ColorPalette.b1,
            gameMap,
            true);
        _ = player.AddComponent<PlayerController>();
        entities.Add(player);
    }

    private void CreateEntities()
    {
        for (int i = 0; i < 3; i++)
        {
            Entity entity = Instantiate(
                prefab, entityGOContainer.transform);
            entity.Init(
                gameMap.GetRandomFloorTile(seed + i),
                spriteDatabase.GetEnemySprite(),
                ColorPalette.r2,
                gameMap);
            entities.Add(entity);
        }
    }
}
