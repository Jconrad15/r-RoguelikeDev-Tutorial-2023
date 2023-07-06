using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FOVRecursive
{
    private static readonly int[] Octants =
        { 1, 2, 3, 4, 5, 6, 7, 8 };

    //  Octant data
    //
    //    \ 1 | 2 /
    //   8 \  |  / 3
    //   -----+-----
    //   7 /  |  \ 4
    //    / 6 | 5 \
    //
    //  1 = NNW, 2 =NNE, 3=ENE, 4=ESE, 5=SSE, 6=SSW, 7=WSW, 8 = WNW

    /// <summary>
    /// Start here: go through all the octants which surround the player
    /// to determine which open cells are visible
    /// </summary>
    public static void DetermineVisibleTiles(
        GameMap map, (int,int) playerPos, int visibleRange)
    {
        List<Tile> visibleTiles = new List<Tile>();
        for (int i = 0; i < Octants.Length; i++)
        {
            ScanOctant(
                1, Octants[i], 1.0, 0.0,
                map, playerPos, visibleRange, visibleTiles);
        }
        SetTileVisibility(map, visibleTiles); 
    }

    private static void SetTileVisibility(
        GameMap map, List<Tile> visibleTiles)
    {
        for (int i = 0; i < map.tiles.Length; i++)
        {
            Tile tile = map.tiles[i];
            bool isNowVisible = visibleTiles.Contains(tile);

            if (isNowVisible)
            {
                tile.visibility = TileVisibility.Visible;
            }
            else if (tile.visibility == TileVisibility.Visible)
            {
                tile.visibility = TileVisibility.PreviouslySeen;
            }
        }
    }

    private static void AddVisibleTile(Tile t, List<Tile> visibleTiles)
    {
        if (visibleTiles.Contains(t))
            return;

        visibleTiles.Add(t);
    }

    /// <summary>
    /// Examine the provided octant and calculate the visible cells within it.
    /// </summary>
    /// <param name="pDepth">Depth of the scan</param>
    /// <param name="pOctant">Octant being examined</param>
    /// <param name="pStartSlope">Start slope of the octant</param>
    /// <param name="pEndSlope">End slope of the octance</param>
    private static void ScanOctant(
        int pDepth, int pOctant, double pStartSlope, double pEndSlope,
        GameMap map, (int, int) playerPos, int visualRange,
        List<Tile> visibleTiles)
    {
        int visrange2 = visualRange * visualRange;
        int x = 0;
        int y = 0;

        switch (pOctant)
        {

            case 1: //nnw
                y = playerPos.Item2 - pDepth;
                if (y < 0)
                {
                    return;
                }

                x = playerPos.Item1 - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (x < 0)
                {
                    x = 0;
                }

                while (GetSlope(x, y, playerPos.Item1, playerPos.Item2, false) >= pEndSlope)
                {
                    if (GetVisDistance(x, y, playerPos.Item1, playerPos.Item2) <= visrange2)
                    {
                        Tile tile = map.tiles[map.GetIndex(x, y)];
                        AddVisibleTile(tile, visibleTiles);

                        if (tile.transparent == false) //current cell blocked
                        {
                            if (x - 1 >= 0 && map.tiles[map.GetIndex(x - 1, y)].transparent == true) //prior cell within range AND open...
                                                                  //...increment the depth, adjust the endslope and recurse
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, playerPos.Item1, playerPos.Item2, false), map, playerPos, visualRange, visibleTiles);
                        }
                        else
                        {
                            if (x - 1 >= 0 && map.tiles[map.GetIndex(x - 1, y)].transparent == false) //prior cell within range AND open...
                                                                  //..adjust the startslope
                                pStartSlope = GetSlope(x - 0.5, y - 0.5, playerPos.Item1, playerPos.Item2, false);

                        }
                    }
                    x++;
                }
                x--;
                break;

            case 2: //nne

                y = playerPos.Item2- pDepth;
                if (y < 0) return;

                x = playerPos.Item1 + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (x >= map.width)
                {
                    x = map.width - 1;
                }

                while (GetSlope(x, y, playerPos.Item1, playerPos.Item2, false) <= pEndSlope)
                {
                    if (GetVisDistance(x, y, playerPos.Item1, playerPos.Item2) <= visrange2)
                    {
                        Tile tile = map.tiles[map.GetIndex(x, y)];
                        AddVisibleTile(tile, visibleTiles);

                        if (tile.transparent == false)
                        {
                            if (x + 1 < map.width && map.tiles[map.GetIndex(x + 1, y)].transparent == true)
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, playerPos.Item1, playerPos.Item2, false), map, playerPos, visualRange, visibleTiles);
                        }
                        else
                        {
                            if (x + 1 < map.width && map.tiles[map.GetIndex(x + 1, y)].transparent == false)
                                pStartSlope = -GetSlope(x + 0.5, y - 0.5, playerPos.Item1, playerPos.Item2, false);
                        }
                    }
                    x--;
                }
                x++;
                break;

            case 3:

                x = playerPos.Item1 + pDepth;
                if (x >= map.width) return;

                y = playerPos.Item2 - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (y < 0) y = 0;

                while (GetSlope(x, y, playerPos.Item1, playerPos.Item2, true) <= pEndSlope)
                {

                    if (GetVisDistance(x, y, playerPos.Item1, playerPos.Item2) <= visrange2)
                    {

                        Tile tile = map.tiles[map.GetIndex(x, y)];
                        AddVisibleTile(tile, visibleTiles);

                        if (tile.transparent == false)
                        {
                            if (y - 1 >= 0 && map.tiles[map.GetIndex(x, y - 1)].transparent == true)
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, playerPos.Item1, playerPos.Item2, true), map, playerPos, visualRange, visibleTiles);
                        }
                        else
                        {
                            if (y - 1 >= 0 && map.tiles[map.GetIndex(x, y - 1)].transparent == false)
                                pStartSlope = -GetSlope(x + 0.5, y - 0.5, playerPos.Item1, playerPos.Item2, true);
                        }
                    }
                    y++;
                }
                y--;
                break;

            case 4:

                x = playerPos.Item1 + pDepth;
                if (x >= map.width) return;

                y = playerPos.Item2 + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (y >= map.height) y = map.height - 1;

                while (GetSlope(x, y, playerPos.Item1, playerPos.Item2, true) >= pEndSlope)
                {

                    if (GetVisDistance(x, y, playerPos.Item1, playerPos.Item2) <= visrange2)
                    {
                        Tile tile = map.tiles[map.GetIndex(x, y)];
                        AddVisibleTile(tile, visibleTiles);

                        if (tile.transparent == false)
                        {
                            if (y + 1 < map.height && map.tiles[map.GetIndex(x, y + 1)].transparent == true)
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, playerPos.Item1, playerPos.Item2, true), map, playerPos, visualRange, visibleTiles);
                        }
                        else
                        {
                            if (y + 1 < map.height && map.tiles[map.GetIndex(x, y + 1)].transparent == false)
                                pStartSlope = GetSlope(x + 0.5, y + 0.5, playerPos.Item1, playerPos.Item2, true);

                        }
                    }
                    y--;
                }
                y++;
                break;

            case 5:

                y = playerPos.Item2 + pDepth;
                if (y >= map.height) return;

                x = playerPos.Item1 + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (x >= map.width) x = map.width - 1;

                while (GetSlope(x, y, playerPos.Item1, playerPos.Item2, false) >= pEndSlope)
                {
                    if (GetVisDistance(x, y, playerPos.Item1, playerPos.Item2) <= visrange2)
                    {
                        Tile tile = map.tiles[map.GetIndex(x, y)];
                        AddVisibleTile(tile, visibleTiles);

                        if (tile.transparent == false)
                        {
                            if (x + 1 < map.height && map.tiles[map.GetIndex(x + 1, y)].transparent == true)
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, playerPos.Item1, playerPos.Item2, false), map, playerPos, visualRange, visibleTiles);
                        }
                        else
                        {
                            if (x + 1 < map.height
                                    && map.tiles[map.GetIndex(x + 1, y)].transparent == false)
                                pStartSlope = GetSlope(x + 0.5, y + 0.5, playerPos.Item1, playerPos.Item2, false);

                        }
                    }
                    x--;
                }
                x++;
                break;

            case 6:

                y = playerPos.Item2 + pDepth;
                if (y >= map.height) return;

                x = playerPos.Item1 - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (x < 0) x = 0;

                while (GetSlope(x, y, playerPos.Item1, playerPos.Item2, false) <= pEndSlope)
                {
                    if (GetVisDistance(x, y, playerPos.Item1, playerPos.Item2) <= visrange2)
                    {
                        Tile tile = map.tiles[map.GetIndex(x, y)];
                        AddVisibleTile(tile, visibleTiles);

                        if (tile.transparent == false)
                        {
                            if (x - 1 >= 0 && map.tiles[map.GetIndex(x - 1, y)].transparent == true)
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, playerPos.Item1, playerPos.Item2, false), map, playerPos, visualRange, visibleTiles);
                        }
                        else
                        {
                            if (x - 1 >= 0
                                    && map.tiles[map.GetIndex(x - 1, y)].transparent == false)
                                pStartSlope = -GetSlope(x - 0.5, y + 0.5, playerPos.Item1, playerPos.Item2, false);

                        }
                    }
                    x++;
                }
                x--;
                break;

            case 7:

                x = playerPos.Item1 - pDepth;
                if (x < 0) return;

                y = playerPos.Item2 + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (y >= map.height) y = map.height - 1;

                while (GetSlope(x, y, playerPos.Item1, playerPos.Item2, true) <= pEndSlope)
                {

                    if (GetVisDistance(x, y, playerPos.Item1, playerPos.Item2) <= visrange2)
                    {
                        Tile tile = map.tiles[map.GetIndex(x, y)];
                        AddVisibleTile(tile, visibleTiles);

                        if (tile.transparent == false)
                        {
                            if (y + 1 < map.height && map.tiles[map.GetIndex(x, y + 1)].transparent == true)
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, playerPos.Item1, playerPos.Item2, true), map, playerPos, visualRange, visibleTiles);
                        }
                        else
                        {
                            if (y + 1 < map.height && map.tiles[map.GetIndex(x, y + 1)].transparent == false)
                                pStartSlope = -GetSlope(x - 0.5, y + 0.5, playerPos.Item1, playerPos.Item2, true);
                        }
                    }
                    y--;
                }
                y++;
                break;

            case 8: //wnw

                x = playerPos.Item1 - pDepth;
                if (x < 0) return;

                y = playerPos.Item2 - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                if (y < 0) y = 0;

                while (GetSlope(x, y, playerPos.Item1, playerPos.Item2, true) >= pEndSlope)
                {

                    if (GetVisDistance(x, y, playerPos.Item1, playerPos.Item2) <= visrange2)
                    {
                        Tile tile = map.tiles[map.GetIndex(x, y)];
                        AddVisibleTile(tile, visibleTiles);

                        if (tile.transparent == false)
                        {
                            if (y - 1 >= 0 && map.tiles[map.GetIndex(x, y - 1)].transparent == true)
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, playerPos.Item1, playerPos.Item2, true), map, playerPos, visualRange, visibleTiles);
                        }
                        else
                        {
                            if (y - 1 >= 0 && map.tiles[map.GetIndex(x, y - 1)].transparent == false)
                                pStartSlope = GetSlope(x - 0.5, y - 0.5, playerPos.Item1, playerPos.Item2, true);
                        }
                    }
                    y++;
                }
                y--;
                break;
        }


        if (x < 0)
            x = 0;
        else if (x >= map.width)
            x = map.width - 1;

        if (y < 0)
            y = 0;
        else if (y >= map.height)
            y = map.height - 1;

        if (pDepth < visualRange & map.tiles[map.GetIndex(x,y)].transparent == true)
            ScanOctant(pDepth + 1, pOctant, pStartSlope, pEndSlope, map, playerPos, visualRange, visibleTiles);

    }

    /// <summary>
    /// Get the gradient of the slope formed by the two points
    /// </summary>
    /// <param name="pX1"></param>
    /// <param name="pY1"></param>
    /// <param name="pX2"></param>
    /// <param name="pY2"></param>
    /// <param name="pInvert">Invert slope</param>
    /// <returns></returns>
    private static double GetSlope(double pX1, double pY1, double pX2, double pY2, bool pInvert)
    {
        return pInvert ? (pY1 - pY2) / (pX1 - pX2) : (pX1 - pX2) / (pY1 - pY2);
    }


    /// <summary>
    /// Calculate the distance between the two points
    /// </summary>
    /// <param name="pX1"></param>
    /// <param name="pY1"></param>
    /// <param name="pX2"></param>
    /// <param name="pY2"></param>
    /// <returns>Distance</returns>
    private static int GetVisDistance(int pX1, int pY1, int pX2, int pY2)
    {
        return (pX1 - pX2) * (pX1 - pX2) + (pY1 - pY2) * (pY1 - pY2);
    }

}