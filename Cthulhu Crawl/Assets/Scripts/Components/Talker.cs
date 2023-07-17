using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : MonoBehaviour
{
    private Entity entity;
    public Dialogue Dialogue { get; private set; }

    public void Init(Entity entity, Dialogue dialogue)
    {
        this.entity = entity;
        Dialogue = dialogue;
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
        FindAnyObjectByType<DialogueDisplayManager>()
            .StartDialogue(talkInitiator, this);
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
