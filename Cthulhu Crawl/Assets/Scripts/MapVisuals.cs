using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVisuals : MonoBehaviour
{
    private GameObject[] tileGOs;
    private SpriteDatabase spriteDatabase;
    private Main main;


    private void Start()
    {
        main = FindAnyObjectByType<Main>();
        spriteDatabase = FindAnyObjectByType<SpriteDatabase>();
    }

    public void InitializeVisuals()
    {
        EntityManager em = FindAnyObjectByType<EntityManager>();

        em.Player.RegisterOnMove(UpdateVisuals);
        GameMap map = main.GetMap();
        FOVRecursive.DetermineVisibleTiles(
            map, em.Player.GetPosition(), 6);

        tileGOs = new GameObject[map.tiles.Length];
        for (int i = 0; i < map.tiles.Length; i++)
        {
            (int x, int y) = map.GetPosition(i);
            DrawNewTile(map.tiles[i], i, x, y);
        }
    }

    public void UpdateVisuals(Entity player)
    {
        GameMap map = main.GetMap();
        FOVRecursive.DetermineVisibleTiles(
            map, player.GetPosition(), 6);

        for (int i = 0; i < map.tiles.Length; i++)
        {
            (int x, int y) = map.GetPosition(i);
            UpdateTile(map.tiles[i], i, x, y);
        }

    }

    private void UpdateTile(Tile tile, int index, int x, int y)
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
