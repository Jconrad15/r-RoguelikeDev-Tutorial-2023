using System.Collections.Generic;
using UnityEngine;

public static class BresenhamLine
{
    public static List<(int, int)> GetLocationsAlongLine(
        (int, int) start, (int, int) end)
    {
        return LineNoDiagonal(
            start.Item1, start.Item2, end.Item1, end.Item2);
    }

    private static List<(int, int)> LineNoDiagonal(
        int x0, int y0, int x1, int y1)
    {
        List<(int, int)> positions = new List<(int, int)>();

        int xDist = Mathf.Abs(x1 - x0);
        int yDist = -Mathf.Abs(y1 - y0);
        int xStep = (x0 < x1 ? +1 : -1);
        int yStep = (y0 < y1 ? +1 : -1);
        int error = xDist + yDist;

        positions.Add((x0, y0));

        while (x0 != x1 || y0 != y1)
        {
            if (2 * error - yDist > xDist - 2 * error)
            {
                // horizontal step
                error += yDist;
                x0 += xStep;
            }
            else
            {
                // vertical step
                error += xDist;
                y0 += yStep;
            }

            positions.Add((x0, y0));
        }

        return positions;
    }
}
