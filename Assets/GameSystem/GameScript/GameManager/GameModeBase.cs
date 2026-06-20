using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for different game modes / win conditions.
/// Derived classes should implement specific win logic.
/// </summary>
public abstract class GameModeBase : MonoBehaviour
{
    public bool isGameOver { get; private set; } = false;

    /// <summary>
    /// Called once when the game starts.
    /// </summary>
    protected virtual void Start()
    {
        isGameOver = false;
        OnGameStart();
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    protected virtual void Update()
    {
        if (!isGameOver)
        {
            CheckWinCondition();
        }
    }

    /// <summary>
    /// Called when the game starts. Override to initialize your mode.
    /// </summary>
    protected virtual void OnGameStart() { }

    /// <summary>
    /// Check for the win condition. Derived classes must implement.
    /// </summary>
    protected abstract void CheckWinCondition();

    /// <summary>
    /// Call this method to end the game and trigger win logic.
    /// </summary>
    protected void WinGame()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("?? Game Won!");
        OnGameWon();
    }

    /// <summary>
    /// Override to do custom logic when the game is won.
    /// </summary>
    protected virtual void OnGameWon() { }

    /// <summary>
    /// Call this method to end the game and trigger lose logic.
    /// </summary>
    protected void LoseGame()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("?? Game Lost!");
        OnGameLost();
    }

    /// <summary>
    /// Override to do custom logic when the game is lost.
    /// </summary>
    protected virtual void OnGameLost() { }
}
