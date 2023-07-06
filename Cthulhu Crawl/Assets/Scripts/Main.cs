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
        CreateEntity();
    }

    private void InitializeMap()
    {
        int mapWidth = 30;
        int mapHeight = 20;
        int roomMaxSize = 10;
        int roomMinSize = 3;
        int maxRooms = 30;
        gameMap = ProcGen.GenerateDungeon(
            mapWidth, mapHeight, maxRooms, roomMinSize, roomMaxSize,
            seed);
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

    private void CreateEntity()
    {
        Entity entity = Instantiate(prefab, entityGOContainer.transform);
        entity.Init(
            (2, 2),
            spriteDatabase.GetEnemySprite(),
            ColorPalette.r2,
            gameMap);
        entities.Add(entity);
    }
}
