using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProcGen
{

    public static GameMap GenerateDungeon(int width, int height)
    {
        GameMap newMap = new GameMap(width, height);

        RectangularRoom room1 = new RectangularRoom(0, 1, 15, 5);
        RectangularRoom room2 = new RectangularRoom(20, 10, 6, 7);

        CarveRoom(newMap, room1.Inner);
        CarveRoom(newMap, room2.Inner);

        return newMap;
    }

    private static void CarveRoom(
        GameMap newMap, List<(int, int)> locations)
    {
        for (int i = 0; i < locations.Count; i++)
        {
            int x = locations[i].Item1;
            int y = locations[i].Item2;

            int index = newMap.GetIndex(x, y);
            newMap.tiles[index] = new Tile(TileType.Floor);
        }
    }
}
