using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Wall, Floor };
public class Tile
{
    public bool walkable;
    public bool transparent;
    public Color foregroundColor;
    public Color backgroundColor;
    public TileType tileType;

    public Tile(
        bool walkable, bool transparent,
        Color foregroundColor, Color backgroundColor,
        TileType tileType)
    {
        this.walkable = walkable;
        this.transparent = transparent;
        this.foregroundColor = foregroundColor;
        this.backgroundColor = backgroundColor;
        this.tileType = tileType;
    }

    public Tile(TileType tileType)
    {
        this.tileType = tileType;

        switch (tileType)
        {
            case TileType.Wall:
                walkable = false;
                transparent = false;
                foregroundColor = Color.white;
                backgroundColor = Color.black;
                break;

            case TileType.Floor:
                walkable = true;
                transparent = true;
                foregroundColor = Color.white;
                backgroundColor = Color.black;
                break;
        }
    }
}