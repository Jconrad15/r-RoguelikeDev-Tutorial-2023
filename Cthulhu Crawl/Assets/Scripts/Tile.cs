using UnityEngine;

public enum TileType { Wall, Floor };
public enum TileVisibility { Visible, NotVisible, PreviouslySeen };
public class Tile
{
    public bool walkable;
    public bool transparent;
    public Color foregroundColor;
    public Color backgroundColor;
    public TileType tileType;
    public TileVisibility visibility;
    public (int, int) position;

    public Tile(TileType tileType, (int, int) position)
    {
        this.tileType = tileType;
        visibility = TileVisibility.NotVisible;

        switch (tileType)
        {
            case TileType.Wall:
                walkable = false;
                transparent = false;
                foregroundColor = ColorPalette.r3;
                backgroundColor = Color.black;
                break;

            case TileType.Floor:
                walkable = true;
                transparent = true;
                foregroundColor = ColorPalette.r1;
                backgroundColor = Color.black;
                break;
        }

        this.position = position;
    }
}