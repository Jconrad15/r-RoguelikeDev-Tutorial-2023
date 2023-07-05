using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Entity entity;

    private void Start()
    {
        entity = GetComponent<Entity>();
    }

    private void Update()
    {
        bool left = Input.GetKeyDown(KeyCode.A);
        bool right = Input.GetKeyDown(KeyCode.D);

        bool up = Input.GetKeyDown(KeyCode.W);
        bool down = Input.GetKeyDown(KeyCode.S);

        if (left)
        {
            entity.Move(-1, 0);
        }
        else if (right)
        {
            entity.Move(1, 0);
        }
        else if (up)
        {
            entity.Move(0, 1);
        }
        else if (down)
        {
            entity.Move(0, -1);
        }

    }
}
