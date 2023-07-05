using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private int x;
    private int y;
    private Sprite image;
    private Color color;
    private bool isPlayer;
    private GameMap map;

    public void Init(
        int x, int y, Sprite image, Color color,
        GameMap map, bool isPlayer = false)
    {
        this.map = map;

        this.x = x;
        this.y = y;
        transform.position = new Vector3(x, y, 0);

        this.image = image;
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = image;

        this.color = color;
        sr.color = color;

        this.isPlayer = isPlayer;
    }

    public void Move(int dx, int dy)
    {
        int targetX = x + dx;
        int targetY = y + dy;
        if (map.IsWalkable(targetX, targetY) == false) { return; }

        x = targetX;
        y = targetY;
        transform.position = new Vector3(x, y, 0);
    }

}
