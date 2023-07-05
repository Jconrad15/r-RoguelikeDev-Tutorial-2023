using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap
{
    public int width;
    public int height;
    public Tile[] tiles;
    public (int, int) startingPosition;

    public GameMap(int width, int height)
    {
        this.width = width;
        this.height = height;
        
        tiles = new Tile[width * height];
        for (int i = 0; i < tiles.Length; i++)
        {
            //(int x, int y) = GetPosition(i);
            tiles[i] = new Tile(TileType.Wall);
        }
    }

    public bool InBounds(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public bool IsWalkable(int x, int y)
    {
        if (InBounds(x, y) == false) { return false; }

        int index = GetIndex(x, y);

        return tiles[index].walkable == true;
    }

    public int GetIndex(int x, int y)
    {
        return y * width + x;
    }

    public (int, int) GetPosition(int index)
    {
        int x = index % width;
        int y = index / width;
        return (x, y);
    }
}
