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

    public void Init(
        int x, int y, Sprite image, Color color, bool isPlayer = false)
    {
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
        x += dx;
        y += dy;
        transform.position = new Vector3(x, y, 0);
    }

}
