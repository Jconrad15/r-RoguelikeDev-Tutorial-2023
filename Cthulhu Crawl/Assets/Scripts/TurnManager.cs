using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn { Player, AI };
public class TurnManager : MonoBehaviour
{
    private Action cbOnStartAITurn;
    private Action cbOnStartPlayerTurn;

    public Turn CurrentTurn { get; private set; }

    public void Init()
    {
        CurrentTurn = Turn.Player;
    }

    public void PlayerEndTurn()
    {
        CurrentTurn = Turn.AI;
        cbOnStartAITurn?.Invoke();
    }

    public void AIEndTurn()
    {
        CurrentTurn = Turn.Player;
        cbOnStartPlayerTurn?.Invoke();
    }

    public void RegisterOnStartAITurn(Action callbackfunc)
    {
        cbOnStartAITurn += callbackfunc;
    }

    public void UnregisterOnStartAITurn(Action callbackfunc)
    {
        cbOnStartAITurn -= callbackfunc;
    }

    public void RegisterOnStartPlayerTurn(Action callbackfunc)
    {
        cbOnStartPlayerTurn += callbackfunc;
    }

    public void UnregisterOnStartPlayerTurn(Action callbackfunc)
    {
        cbOnStartPlayerTurn -= callbackfunc;
    }

}
