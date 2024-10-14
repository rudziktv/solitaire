using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameRules gameRules;
    [SerializeField] private Sprite backCardSprite;
    public GameRules GameRules => gameRules;
    public Sprite BackCardSprite => backCardSprite;

    private UIManager UI => UIManager.Instance;
    public bool DisabledInteractions { get; set; }

    public float Timer { get; private set; }
    // public bool Paused { get; set; }
    
    private readonly List<Action> _undoActions = new();
    private readonly List<Action> _redoActions = new();
    
    public delegate void OnActionsChangedArgs(bool isUndoNotEmpty, bool isRedoNotEmpty);
    public event OnActionsChangedArgs OnActionsChanged;

    public bool Paused { get; private set; }
    
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadGameMode();
    }

    private void Update()
    {
        if (!Paused)
            Timer += Time.deltaTime;
    }

    public void LoadGameMode()
    {
        Timer = 0f;
        _undoActions.Clear();
        _redoActions.Clear();
        GameRules.GameStart();
        ActionsChanged();
    }

    public void Escape()
    {
        UI.EscapePopupOpen();
        SetPauseGame(!Paused);
    }

    public void SetPauseGame(bool paused)
    {
        Paused = paused;
        DisabledInteractions = paused;
        Time.timeScale = paused ? 0 : 1;
    }

    public void AddMove(Action undoMove)
    {
        _undoActions.Add(undoMove);
        _redoActions.Clear();
        
        // Debug.Log($"Undo moves: {_undoActions.Count}");
        ActionsChanged();
    }

    public void Undo()
    {
        if (_undoActions.Count <= 0) return;
        var action = _undoActions.Last();
        _undoActions.RemoveAt(_undoActions.Count - 1);
        action.Invoke();
        _redoActions.Add(action);
        // Debug.Log($"Undo moves: {_undoActions.Count}");
        ActionsChanged();
    }

    public void Redo()
    {
        if (_redoActions.Count <= 0) return;
        var action = _redoActions.Last();
        _redoActions.Remove(action);
        action.Invoke();
        _undoActions.Add(action);
        ActionsChanged();
    }

    private void ActionsChanged()
    {
        OnActionsChanged?.Invoke(_undoActions.Count > 0, _redoActions.Count > 0);
    }
}