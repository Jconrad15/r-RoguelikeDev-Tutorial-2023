using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Entity player;
    
    private void Start()
    {
        player = GetComponent<Entity>();
    }

    private void Update()
    {
        GetPlayerInput();
    }

    private void GetPlayerInput()
    {
        bool playerActed = false;

        bool left = Input.GetKeyDown(KeyCode.A);
        bool right = Input.GetKeyDown(KeyCode.D);

        bool up = Input.GetKeyDown(KeyCode.W);
        bool down = Input.GetKeyDown(KeyCode.S);

        if (left)
        {
            player.ActInDirection(Direction.W);
            playerActed = true;
        }
        else if (right)
        {
            player.ActInDirection(Direction.E);
            playerActed = true;
        }
        else if (up)
        {
            player.ActInDirection(Direction.N);
            playerActed = true;
        }
        else if (down)
        {
            player.ActInDirection(Direction.S);
            playerActed = true;
        }

        if (playerActed)
        {
            player.entityManager.HandleEntityTurns();
        }
    }
}
