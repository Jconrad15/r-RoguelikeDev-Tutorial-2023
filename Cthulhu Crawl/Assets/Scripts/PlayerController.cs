using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Entity player;
    private Mover playerMover;
    private Fighter playerFighter;

    public void Init()
    {
        player = GetComponent<Entity>();
        playerMover = GetComponent<Mover>();
        playerFighter = GetComponent<Fighter>();
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
            if (playerMover.TryMoveInDirection(Direction.W))
            {
                playerActed = true;
            }
            else
            {
                if (playerFighter.TryMeleeAction(Direction.W))
                {
                    playerActed = true;
                }
            }
        }
        else if (right)
        {
            if (playerMover.TryMoveInDirection(Direction.E))
            {
                playerActed = true;
            }
            else
            {
                if (playerFighter.TryMeleeAction(Direction.E))
                {
                    playerActed = true;
                }
            }
        }
        else if (up)
        {
            if (playerMover.TryMoveInDirection(Direction.N))
            {
                playerActed = true;
            }
            else
            {
                if (playerFighter.TryMeleeAction(Direction.N))
                {
                    playerActed = true;
                }
            }
        }
        else if (down)
        {
            if (playerMover.TryMoveInDirection(Direction.S))
            {
                playerActed = true;
            }
            else
            {
                if (playerFighter.TryMeleeAction(Direction.S))
                {
                    playerActed = true;
                }
            }
        }

        // End turn
        if (playerActed)
        {
            player.entityManager.HandleEntityTurns();
        }
    }
}
