using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : MonoBehaviour
{
    private Entity entity;
    private string dialogue;

    public void Init(Entity entity, string dialogue)
    {
        this.entity = entity;
        this.dialogue = dialogue;
    }

    public bool TryTalkAction(Direction direction)
    {
        GetPositionInDirection(
            direction, out int targetX, out int targetY);

        // Check if entity is in tile
        List<Entity> targetTileEntities =
            entity.entityManager.GetEntityAtLocation(targetX, targetY);
        if (targetTileEntities == null) { return false; }

        // Talk
        for (int i = 0; i < targetTileEntities.Count; i++)
        {
            if (targetTileEntities[i].TryGetComponent(
                out Talker otherTalker))
            {
                otherTalker.TalkTo(this);
                return true;
            }
        }

        return false;
    }

    public void TalkTo(Talker talkInitiator)
    {
        Debug.Log(talkInitiator.entity.EntityName +
            " talked to " +
            entity.EntityName);
        Debug.Log(dialogue);
    }

    private void GetPositionInDirection(
        Direction direction, out int targetX, out int targetY)
    {
        (int dx, int dy) = GameMap.ConvertDirectionToDeltaCoord(direction);
        (int x, int y) = entity.GetPosition();
        targetX = x + dx;
        targetY = y + dy;
    }
}
