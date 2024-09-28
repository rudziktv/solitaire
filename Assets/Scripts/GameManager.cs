using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameRules gameRules;
    [SerializeField] private Sprite backCardSprite;
    public GameRules GameRules => gameRules;
    public Sprite BackCardSprite => backCardSprite;

    public bool DisabledInteractions { get; set; }
    // public bool Paused { get; set; }
    
    private readonly List<Action> _undoActions = new();
    private readonly List<Action> _redoActions = new();
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadGameMode();
    }

    public void LoadGameMode()
    {
        GameRules.GameStart();
    }

    public void AddMove(Action undoMove)
    {
        _undoActions.Add(undoMove);
        _redoActions.Clear();
    }

    public void Undo()
    {
        if (_undoActions.Count <= 0) return;
        var action = _undoActions.Last();
        _undoActions.Remove(action);
        action.Invoke();
        _redoActions.Add(action);
    }

    public void Redo()
    {
        if (_redoActions.Count <= 0) return;
        var action = _redoActions.Last();
        _redoActions.Remove(action);
        action.Invoke();
        _undoActions.Add(action);
    }
}