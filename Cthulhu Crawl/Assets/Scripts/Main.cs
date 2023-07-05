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

    private void Start()
    {
        spriteDatabase = FindAnyObjectByType<SpriteDatabase>();
        entities = new List<Entity>();

        CreatePlayer();
        CreateEntity();
    }

    private void CreatePlayer()
    {
        Entity player = Instantiate(prefab, entityGOContainer.transform);
        player.Init(
            0, 0, spriteDatabase.GetPlayerSprite(), Color.white, true);
        _ = player.AddComponent<PlayerController>();
        entities.Add(player);
    }

    private void CreateEntity()
    {
        Entity entity = Instantiate(prefab, entityGOContainer.transform);
        entity.Init(
            2, 2, spriteDatabase.GetEnemySprite(), Color.red);
        entities.Add(entity);
    }
}
