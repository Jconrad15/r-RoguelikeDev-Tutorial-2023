using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVisuals : MonoBehaviour
{
    private GameObject[] tileGOs;
    private SpriteDatabase spriteDatabase;

    private void Start()
    {
        spriteDatabase = FindAnyObjectByType<SpriteDatabase>();
    }

    public void UpdateVisuals(GameMap map)
    {
        tileGOs = new GameObject[map.tiles.Length];
        for (int i = 0; i < map.tiles.Length; i++)
        {
            (int x, int y) = map.GetPosition(i);
            DrawTile(map.tiles[i], i, x, y);
        }
    }

    private void DrawTile(Tile tile, int index, int x, int y)
    {
        GameObject newTileGO = new GameObject("Tile " + index);
        newTileGO.transform.SetParent(transform);
        tileGOs[index] = newTileGO;

        newTileGO.transform.position = new Vector2(x, y);

        SpriteRenderer sr = newTileGO.AddComponent<SpriteRenderer>();
        sr.sprite = spriteDatabase.GetTile(tile.tileType);
        sr.color = tile.foregroundColor;

    }
}
