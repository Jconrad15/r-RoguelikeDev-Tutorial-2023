using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private int movementIncrements = 30;

    [SerializeField]
    SpriteRenderer sr;

    private int x;
    private int y;
    private Sprite image;
    private Color color;
    private bool isPlayer;
    private GameMap map;

    private bool isMoving = false;
    private Vector2 currentTarget;

    public void Init(
        (int, int) startingPosition, Sprite image, Color color,
        GameMap map, bool isPlayer = false)
    {
        this.map = map;

        x = startingPosition.Item1;
        y = startingPosition.Item2;
        transform.position = new Vector3(x, y, 0);

        this.image = image;
        sr.sprite = image;

        this.color = color;
        sr.color = color;

        this.isPlayer = isPlayer;

        currentTarget = new Vector2(x, y);
    }

    public void SetStartingPosition((int, int) pos)
    {
        x = pos.Item1;
        y = pos.Item2;
        SetToTargetPos();
    }

    public void Move(int dx, int dy)
    {
        if (isMoving)
        {
            SetToTargetPos();
            StopAllCoroutines();
        }

        int targetX = x + dx;
        int targetY = y + dy;
        if (map.IsWalkable(targetX, targetY) == false) { return; }

        currentTarget = new Vector2(targetX, targetY);
                x = (int)currentTarget.x;
        y = (int)currentTarget.y;
        StartCoroutine(
            LerpMove(currentTarget));
    }

    private IEnumerator LerpMove(Vector2 target)
    {
        isMoving = true;
        Vector2 startingPos = transform.position;
        for (int i = 0; i < movementIncrements; i++)
        {
            transform.position =
                Vector2.Lerp(
                    startingPos,
                    target,
                    i / (float)movementIncrements);
            yield return new WaitForEndOfFrame();
        }

        SetToTargetPos();
    }

    private void SetToTargetPos()
    {
        x = (int)currentTarget.x;
        y = (int)currentTarget.y;
        transform.position = currentTarget;
        isMoving = false;
    }
}
