using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RectangularRoom
{
    private int x1;
    private int x2;
    private int y1;
    private int y2;

    public (int, int) Center
    {
        get
        {
            int centerX = (x1 + x2) / 2;
            int centerY = (y1 + y2) / 2;
            return (centerX, centerY);
        }
    }

    public (int, int) GetRandomPosition()
    {
        List<(int, int)> locations = Inner;
        return locations[Random.Range(0, locations.Count)];
    }

    public List<(int, int)> Inner
    {
        get
        {
            List<(int,int)> locations = new List<(int,int)> ();

            int[] xSlice = Enumerable.Range(
                x1 + 1, (x2 - 1) - x1).ToArray();
            int[] ySlice = Enumerable.Range(
                y1 + 1, (y2 - 1) - y1).ToArray();

            for (int i = 0; i < xSlice.Length; i++)
            {
                int x = xSlice[i];

                for (int j = 0; j < ySlice.Length; j++)
                {
                    int y = ySlice[j];

                    locations.Add((x, y));
                }
            }

            return locations;
        }
    }

    public RectangularRoom(int x, int y, int width, int height)
    {
        x1 = x;
        y1 = y;
        x2 = x + width;
        y2 = y + height;
    }

    public bool Intersects(RectangularRoom otherRoom)
    {
        return x1 <= otherRoom.x2
            && x2 >= otherRoom.x1
            && y1 <= otherRoom.y2
            && y2 >= otherRoom.y1;
    }


}
