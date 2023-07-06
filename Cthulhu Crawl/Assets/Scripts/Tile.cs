using UnityEngine;

public enum TileType { Wall, Floor };
public class Tile
{
    public bool walkable;
    public bool transparent;
    public Color foregroundColor;
    public Color backgroundColor;
    public TileType tileType;

    public Tile(TileType tileType)
    {
        this.tileType = tileType;

        switch (tileType)
        {
            case TileType.Wall:
                walkable = false;
                transparent = false;
                foregroundColor = ColorPalette.b4;
                backgroundColor = Color.black;
                break;

            case TileType.Floor:
                walkable = true;
                transparent = true;
                foregroundColor = ColorPalette.r1;
                backgroundColor = Color.black;
                break;
        }
    }
}