using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    private EntityManager entityManager;

    private GameMap gameMap;
    [SerializeField]
    private MapVisuals mapVisuals;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private TurnManager turnManager;

    private int seed;

    private void Start()
    {
        seed = Random.Range(-10000, 10000);

        InitializeMap();
        entityManager.InitializeEntities(gameMap, seed);
        mapVisuals.InitializeVisuals();
        uiManager.Init();
        turnManager.Init();
    }

    private void InitializeMap()
    {
        int mapWidth = 30;
        int mapHeight = 20;
        int roomMinSize = 4;
        gameMap = ProcGen.GenerateDungeon(
            mapWidth, mapHeight, roomMinSize, seed);
    }

    public GameMap GetMap()
    {
        return gameMap;
    }
}
