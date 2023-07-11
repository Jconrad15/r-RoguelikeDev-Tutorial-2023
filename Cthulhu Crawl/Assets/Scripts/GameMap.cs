using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Tile[] tiles;
    public (int, int) startingPosition;
    public List<RectangularRoom> rooms;
    public Path_TileGraph TileGraph { get; private set; }

    public GameMap(int width, int height)
    {
        Width = width;
        Height = height;
        
        tiles = new Tile[width * height];
        for (int i = 0; i < tiles.Length; i++)
        {
            (int, int) pos = GetPosition(i);
            tiles[i] = new Tile(TileType.Wall, pos);
        }
    }

    public bool InBounds(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    public bool IsWalkable(int x, int y)
    {
        if (InBounds(x, y) == false) { return false; }

        int index = GetIndex(x, y);

        return tiles[index].isWalkable == true;
    }

    public int GetIndex(int x, int y)
    {
        return y * Width + x;
    }

    public (int, int) GetPosition(int index)
    {
        int x = index % Width;
        int y = index / Width;
        return (x, y);
    }

    public (int, int) GetRandomFloorPosition(int seed)
    {
        // Create seed based state
        Random.State oldState = Random.state;
        Random.InitState(seed);

        List<(int, int)> floorTiles = new List<(int, int)>();

        for (int i = 0; i < rooms.Count; i++)
        {
            floorTiles.AddRange(rooms[i].Inner);
        }

        (int, int) location =
            floorTiles[Random.Range(0, floorTiles.Count)];

        // Restore state
        Random.state = oldState;
        return location;
    }

    public Tile TryGetTileAtCoord(int x, int y)
    {
        if (InBounds(x, y) == false) { return null; }
        return tiles[GetIndex(x, y)];
    }

    public Tile TryGetTileAtCoord((int, int) position)
    {
        return TryGetTileAtCoord(position.Item1, position.Item2);
    }

    public void UpdateTileGraph()
    {
        TileGraph = new Path_TileGraph(this);
    }

    public Tile[] GetTileNeighbors((int, int) position)
    {
        int x = position.Item1;
        int y = position.Item2;
        Tile[] neighbors = new Tile[4]
        {
            TryGetTileAtCoord(x, y + 1), // N
            TryGetTileAtCoord(x + 1, y), // E
            TryGetTileAtCoord(x, y - 1), // S
            TryGetTileAtCoord(x - 1, y)  // W
        };

        return neighbors;
    }

    public float GetDistanceBetweenTiles(Tile a, Tile b)
    {
        float distance =
            Mathf.Sqrt(
                Mathf.Pow(b.position.Item1 - a.position.Item1, 2)
                + Mathf.Pow(b.position.Item2 - a.position.Item2, 2));

        return distance;
    }

    public static (int, int) ConvertDirectionToDeltaCoord(
        Direction dir)
    {
        int dx = 0;
        int dy = 0;
        switch (dir)
        {
            case Direction.N:
                dx = 0;
                dy = 1;
                break;
            case Direction.E:
                dx = 1;
                dy = 0;
                break;
            case Direction.S:
                dx = 0;
                dy = -1;
                break;
            case Direction.W:
                dx = -1;
                dy = 0;
                break;
        }
        return (dx, dy);
    }

}
