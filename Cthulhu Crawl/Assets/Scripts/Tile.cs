using UnityEngine;

public enum TileType { Wall, Floor };
public enum TileVisibility { Visible, NotVisible, PreviouslySeen };
public class Tile
{
    public bool transparent;
    public Color foregroundColor;
    public Color backgroundColor;
    public TileType tileType;
    public TileVisibility visibility;
    public (int, int) position;
    public bool isWalkable;

    public Tile(TileType tileType, (int, int) position)
    {
        this.tileType = tileType;
        visibility = TileVisibility.NotVisible;

        switch (tileType)
        {
            case TileType.Wall:
                transparent = false;
                foregroundColor = ColorPalette.r3;
                backgroundColor = Color.black;
                isWalkable = false;
                break;

            case TileType.Floor:
                transparent = true;
                foregroundColor = ColorPalette.r1;
                backgroundColor = Color.black;
                isWalkable = true;
                break;
        }

        this.position = position;
    }
}