using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ProcGen
{

    public static GameMap GenerateDungeon(
        int width, int height,
        int maxRooms, int roomMinSize, int roomMaxSize)
    {
        GameMap newMap = new GameMap(width, height);

        List<RectangularRoom> rooms = new List<RectangularRoom>();
        for (int i = 0; i < maxRooms; i++)
        {
            int roomWidth = Random.Range(roomMinSize, roomMaxSize);
            int roomHeight = Random.Range(roomMinSize, roomMaxSize);
            int x = Random.Range(0, width - roomWidth - 1);
            int y = Random.Range(0, height - roomHeight - 1);

            RectangularRoom newRoom = new RectangularRoom(
                x, y, roomWidth, roomHeight);

            bool overlap = rooms.Any(r => r.Intersects(newRoom));
            if (overlap) { continue; }

            CarveRoom(newMap, newRoom.Inner);

            if (rooms.Count == 0)
            {
                newMap.startingPosition = newRoom.Center;
            }
            else
            {
                TunnelBetween(
                    newMap,
                    rooms[rooms.Count - 1].Center,
                    newRoom.Center);
            }

            rooms.Add(newRoom);
        }

        Debug.Log(rooms.Count);
        return newMap;
    }

    private static void CarveRoom(
        GameMap map, List<(int, int)> locations)
    {
        for (int i = 0; i < locations.Count; i++)
        {
            int x = locations[i].Item1;
            int y = locations[i].Item2;

            int index = map.GetIndex(x, y);
            map.tiles[index] = new Tile(TileType.Floor);
        }
    }

    private static void TunnelBetween(
        GameMap map, (int, int) start, (int, int) end)
    {
        List<(int, int)> locations =
            BresenhamLine.GetLocationsAlongLine(start, end);

        CarveRoom(map, locations);
    }
}
