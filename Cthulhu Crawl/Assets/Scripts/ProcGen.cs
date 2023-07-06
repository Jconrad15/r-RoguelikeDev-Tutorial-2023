using System.Collections.Generic;
using UnityEngine;

public static class ProcGen
{

    public static GameMap GenerateDungeon(
        int width, int height,
        int roomMinSize,
        int seed)
    {
        // Create seed based state
        Random.State oldState = Random.state;
        Random.InitState(seed);

        GameMap newMap = new GameMap(width, height);

        GenerateRoomsAndHallways(
            width, height,
            roomMinSize, newMap);

        // Restore state
        Random.state = oldState;

        return newMap;
    }

    private static void GenerateRoomsAndHallways(
        int width, int height,
        int roomMinSize, GameMap newMap)
    {
        List<RectangularRoom> rooms = new List<RectangularRoom>();

        BinaryTree tree = CreatePartitionTree(width, height, roomMinSize);

        Queue<Node> toDoNodes = new Queue<Node>();
        toDoNodes.Enqueue(tree.rootNode);
        while (toDoNodes.Count > 0)
        {
            Node currentNode = toDoNodes.Dequeue();

            Node rightNode = currentNode.RightNode;
            Node leftNode = currentNode.LeftNode;

            // Get child nodes
            if (leftNode != null &&
                rightNode != null)
            {
                toDoNodes.Enqueue(leftNode);
                toDoNodes.Enqueue(rightNode);
                continue;
            }

            // Try Edit room for partition
            int x0 = currentNode.XRange.Item1;
            int y0 = currentNode.YRange.Item1;
            int x1 = currentNode.XRange.Item2;
            int y1 = currentNode.YRange.Item2;
            int roomWidth = x1 - x0;
            int roomHeight = y1 - y0;

            bool roomAreaDetermined = false;
            int counter = 0;
            int maxCounter = 10;
            while (roomAreaDetermined == false)
            {
                // Exit if smallest room possible
                if (roomWidth <= roomMinSize && 
                    roomHeight <= roomMinSize)
                {
                    roomAreaDetermined = true;
                }

                int tempx0 = x0 + Random.Range(
                    0, (roomWidth - roomMinSize) / 2);
                int tempx1 = x1 - Random.Range(
                    0, (roomWidth - roomMinSize) / 2);
                int tempy0 = y0 + Random.Range(
                    0, (roomHeight - roomMinSize) / 2);
                int tempy1 = y1 - Random.Range(
                    0, (roomHeight - roomMinSize) / 2);
                int tempRoomWidth = tempx1 - tempx0;
                int tempRoomHeight = tempy1 - tempy0;

                if (tempRoomWidth >= roomMinSize &&
                    tempRoomHeight >= roomMinSize)
                {
                    x0 = tempx0;
                    y0 = tempy0;
                    x1 = tempx1;
                    y1 = tempy1;
                    roomWidth = tempRoomWidth;
                    roomHeight = tempRoomHeight;
                    roomAreaDetermined = true;
                }

                if (counter >= maxCounter)
                {
                    roomAreaDetermined = true;
                }
                counter++;
            }

            RectangularRoom room = new RectangularRoom(
                x0, y0, roomWidth, roomHeight);

            CarveRoom(newMap, room.Inner);
            if (rooms.Count == 0)
            {
                newMap.startingPosition = room.Center;
            }
            rooms.Add(room);
        }

        // Tunnel between rooms
        for (int i = 1; i < rooms.Count; i++)
        {
            TunnelBetween(
                newMap,
                rooms[i].Center,
                rooms[i - 1].Center);
        }
        // Tunnel first to last
        TunnelBetween(
            newMap,
            rooms[rooms.Count - 1].Center,
            rooms[0].Center);

        Debug.Log(rooms.Count);
        newMap.rooms = rooms;
    }

    private static BinaryTree CreatePartitionTree(
        int width, int height, int roomMinSize)
    {
        BinaryTree tree = new BinaryTree(
            new Node(
                null,
                (0, width - 1),
                (0, height - 1)));

        Queue<Node> toDoNodes = new Queue<Node>();
        toDoNodes.Enqueue(tree.rootNode);

        while (toDoNodes.Count > 0)
        {
            Node currentNode = toDoNodes.Dequeue();

            // Don't subdivide if too small of an area
            if ((currentNode.XRange.Item2 - currentNode.XRange.Item1)
                <= roomMinSize * 2
                ||
                (currentNode.YRange.Item2 - currentNode.YRange.Item1)
                <= roomMinSize * 2)
            {
                continue;
            }

            // Chance not to subdivide
            if (Random.value < 0.6f && toDoNodes.Count > 1)
            {
                continue;
            }

            if (Random.value > 0.5f)
            {
                // X
                int splitX = Random.Range(
                    currentNode.XRange.Item1 + roomMinSize + 1,
                    currentNode.XRange.Item2 - roomMinSize + 1);

                currentNode.LeftNode = new Node(
                    currentNode,
                    (currentNode.XRange.Item1, splitX),
                    currentNode.YRange);
                currentNode.RightNode = new Node(
                    currentNode,
                    (splitX, currentNode.XRange.Item2),
                    currentNode.YRange);
            }
            else
            {
                // Y

                int splitY = Random.Range(
                    currentNode.YRange.Item1 + roomMinSize + 1,
                    currentNode.YRange.Item2 - roomMinSize + 1);

                currentNode.LeftNode = new Node(
                    currentNode,
                    currentNode.XRange,
                    (currentNode.YRange.Item1, splitY));
                currentNode.RightNode = new Node(
                    currentNode,
                    currentNode.XRange,
                    (splitY, currentNode.YRange.Item2));
            }

            if (currentNode.LeftNode != null)
            {
                toDoNodes.Enqueue(currentNode.LeftNode);
            }
            if (currentNode.RightNode != null)
            {
                toDoNodes.Enqueue(currentNode.RightNode);
            }
        }

        return tree;
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
