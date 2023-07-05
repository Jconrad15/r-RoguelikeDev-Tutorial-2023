using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDatabase : MonoBehaviour
{
    [SerializeField]
    private Sprite playerSprite;
    public Sprite GetPlayerSprite() { return playerSprite; }

    [SerializeField]
    private Sprite enemySprite;
    public Sprite GetEnemySprite() { return enemySprite; }

    [SerializeField]
    private Sprite wallSprite;
    [SerializeField]
    private Sprite floorSprite;

    public Sprite GetTile(TileType type)
    {
        switch (type)
        {
            case TileType.Wall:
                return wallSprite;

            case TileType.Floor:
                return floorSprite;

            default:
                return null;
        }
    }

}
