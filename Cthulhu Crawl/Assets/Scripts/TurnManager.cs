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
        StartCoroutine(EndPlayerFrame());
    }

    public void AIEndTurn()
    {
        StartCoroutine(EndAIFrame());
    }

    private IEnumerator EndPlayerFrame()
    {
        yield return new WaitForEndOfFrame();
        CurrentTurn = Turn.AI;
        cbOnStartAITurn?.Invoke();
    }

    private IEnumerator EndAIFrame()
    {
        yield return new WaitForEndOfFrame();
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
