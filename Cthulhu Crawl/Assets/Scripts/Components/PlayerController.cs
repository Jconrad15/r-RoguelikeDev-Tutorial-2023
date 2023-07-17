using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Mover playerMover;
    private Fighter playerFighter;
    private Talker playerTalker;

    private Inventory playerInventory;
    private TurnManager turnManager;

    private DialogueDisplayManager dialogueDisplayManager;

    public void Init()
    {
        playerMover = GetComponent<Mover>();
        playerFighter = GetComponent<Fighter>();
        playerTalker = GetComponent<Talker>();
        playerInventory = GetComponent<Inventory>();
        turnManager = FindAnyObjectByType<TurnManager>();

        dialogueDisplayManager = FindAnyObjectByType<DialogueDisplayManager>();
    }

    private void Update()
    {
        if (turnManager.CurrentTurn != Turn.Player) { return; }
        GetPlayerInput();
    }

    private void GetPlayerInput()
    {
        bool playerActed = false;

        bool left = Input.GetKeyDown(KeyCode.A);
        bool right = Input.GetKeyDown(KeyCode.D);

        bool up = Input.GetKeyDown(KeyCode.W);
        bool down = Input.GetKeyDown(KeyCode.S);

        bool space = false;
        if (Input.GetKeyDown(KeyCode.G) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            space = true;
        }

        if (ModeController.Instance.CurrentMode == Mode.Exploration)
        {
            playerActed = ProcessPlayerMovement(
                left, right, up, down, space);
        }

        // End turn
        if (playerActed)
        {
            turnManager.PlayerEndTurn();
        }
    }

    private bool ProcessPlayerMovement(
        bool left, bool right, bool up, bool down, bool space)
    {
        bool playerActed = false;

        if (left)
        {
            if (playerMover.TryMoveInDirection(Direction.W))
            {
                playerActed = true;
                dialogueDisplayManager.HideDialogue();
            }
            else
            {
                if (playerTalker.TryTalkAction(Direction.W))
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
                dialogueDisplayManager.HideDialogue();
            }
            else
            {
                if (playerTalker.TryTalkAction(Direction.E))
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
                dialogueDisplayManager.HideDialogue();
            }
            else
            {
                if (playerTalker.TryTalkAction(Direction.N))
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
                dialogueDisplayManager.HideDialogue();
            }
            else
            {
                if (playerTalker.TryTalkAction(Direction.S))
                {
                    playerActed = true;
                }
            }
        }
        else if (space)
        {
            if (playerInventory.TryPickupItem())
            {
                playerActed = true;
                dialogueDisplayManager.HideDialogue();
            }
            else
            {
                DisplayMessageSystem.Instance.DisplayMessage(
                    "There is no item to pick up.",
                    ColorPalette.r2);
            }
        }

        return playerActed;
    }
}
