using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVisuals : MonoBehaviour
{
    private GameObject[] tileGOs;
    private SpriteDatabase spriteDatabase;
    private Main main;
    private EntityManager entityManager;

    private void Start()
    {
        main = FindAnyObjectByType<Main>();
        spriteDatabase = FindAnyObjectByType<SpriteDatabase>();
        entityManager = FindAnyObjectByType<EntityManager>();
    }

    public void InitializeVisuals()
    {
        entityManager.Player.RegisterOnEndMove(UpdateVisuals);
        GameMap map = main.GetMap();
        FOVRecursive.DetermineVisibleTiles(
            map, entityManager.Player.GetPosition(), 6);

        tileGOs = new GameObject[map.tiles.Length];
        for (int i = 0; i < map.tiles.Length; i++)
        {
            (int x, int y) = map.GetPosition(i);
            DrawNewTile(map.tiles[i], i, x, y);
        }

        entityManager.UpdateEntityVisibility();
    }

    public void UpdateVisuals(Entity player)
    {
        GameMap map = main.GetMap();
        FOVRecursive.DetermineVisibleTiles(
            map, player.GetPosition(), 6);

        for (int i = 0; i < map.tiles.Length; i++)
        {
            UpdateTile(map.tiles[i], i);
        }

        entityManager.UpdateEntityVisibility();
    }

    private void UpdateTile(Tile tile, int index)
    {
        GameObject tileGO = tileGOs[index];
        SpriteRenderer sr = tileGO.GetComponent<SpriteRenderer>();
        SetSpriteColor(tile, sr);
    }

    private void DrawNewTile(Tile tile, int index, int x, int y)
    {
        GameObject newTileGO = new GameObject("Tile " + index);
        newTileGO.transform.SetParent(transform);
        tileGOs[index] = newTileGO;

        newTileGO.transform.position = new Vector2(x, y);

        SpriteRenderer sr = newTileGO.AddComponent<SpriteRenderer>();
        sr.sprite = spriteDatabase.GetTile(tile.tileType);
        SetSpriteColor(tile, sr);
    }

    private static void SetSpriteColor(Tile tile, SpriteRenderer sr)
    {
        Color selectedColor = tile.foregroundColor;
        if (tile.visibility == TileVisibility.NotVisible)
        {
            selectedColor.a = 0;
        }
        else if (tile.visibility == TileVisibility.PreviouslySeen)
        {
            selectedColor = Utility.HideColor(selectedColor);
        }
        sr.color = selectedColor;
    }
}
