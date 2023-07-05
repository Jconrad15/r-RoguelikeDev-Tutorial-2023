using System.Collections.Generic;
using UnityEngine;

public static class BresenhamLine
{
    public static List<(int, int)> GetLocationsAlongLine(
        (int, int) start, (int, int) end)
    {
        List<(int, int)> positions = new List<(int, int)>();

        int x0 = start.Item1;
        int y0 = start.Item2;
        int x1 = end.Item1;
        int y1 = end.Item2;

        return LineNoDiag(x0, y0, x1, y1);
        /*
        if (Mathf.Abs(y1 - y0) < Mathf.Abs(x1 - x0))
        {
            if (x0 > x1)
            {
                return PlotLineLow(x1, y1, x0, y0);
            }
            else
            {
                return PlotLineLow(x0, y0, x1, y1);
            }
        }
        else
        {
            if (y0 > y1)
            {
                return PlotLineHigh(x1, y1, x0, y0);
            }
            else
            {
                return PlotLineHigh(x0, y0, x1, y1);
            }
        }
        */
    }
/*
    private static List<(int, int)> PlotLineLow(
        int x0, int y0, int x1, int y1)
    {
        List<(int, int)> positions = new List<(int, int)>();

        int dx = x1 - x0;
        int dy = y1 - y0;
        int yi = 1;

        if (dy < 0)
        {
            yi = -1;
            dy = -dy;
        }
        int D = (2 * dy) - dx;
        int y = y0;
        for (int x = x0; x < x1; x++)
        {
            positions.Add((x, y));

            if (D > 0)
            {
                y = y + yi;
                D = D + (2 * (dy - dx));
            }
            else
            {
                D = D + 2 * dy;
            }
        }
        return positions;
    }

    private static List<(int, int)> PlotLineHigh(
        int x0, int y0, int x1, int y1)
    {
        List<(int, int)> positions = new List<(int, int)>();

        int dx = x1 - x0;
        int dy = y1 - y0;
        int xi = 1;

        if (dx < 0)
        {
            xi = -1;
            dx = -dx;
        }
        int D = (2 * dx) - dy;
        int x = x0;

        for (int y = y0; y < y1; y++)
        {
            positions.Add((x, y));

            if (D > 0)
            {
                x = x + xi;
                D = D + (2 * (dx - dy));
            }
            else
            {
                D = D + 2 * dx;
            }
        }

        return positions;
    }
*/
    public static List<(int, int)> LineNoDiag(
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
