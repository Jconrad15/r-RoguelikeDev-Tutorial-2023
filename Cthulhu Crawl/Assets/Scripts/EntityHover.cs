using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHover : MonoBehaviour
{
    private Entity entity;

    private void OnMouseEnter()
    {
        if (entity == null)
        {
            entity = GetComponent<Entity>();
        }

        DisplayMessageSystem.Instance.DisplayMessage(
            entity.EntityName,
            ColorPalette.r1);
    }

}
