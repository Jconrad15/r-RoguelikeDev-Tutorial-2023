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


}
