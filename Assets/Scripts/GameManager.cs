using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using Modes;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameRules gameRules;
    [SerializeField] private Sprite backCardSprite;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject slotPrefab;
    public GameRules GameRules => gameRules;
    public Sprite BackCardSprite => backCardSprite;

    private UIManager UI => UIManager.Instance;
    public bool DisabledInteractions { get; set; }
    public GameObject CardPrefab => cardPrefab;
    public GameObject SlotPrefab => slotPrefab;

    public float Timer { get; private set; }
    // public bool Paused { get; set; }
    
    private readonly List<Action> _undoActions = new();
    private readonly List<Action> _redoActions = new();
    
    public delegate void OnActionsChangedArgs(bool isUndoNotEmpty, bool isRedoNotEmpty);
    public event OnActionsChangedArgs OnActionsChanged;

    public bool Paused { get; private set; }
    
    // private GameAnimations _animations;

    private string _lastGameModeArgs = string.Empty;
    
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        
        var anims = new GameObject("Game Animations");
        anims.transform.SetParent(transform);
        // _animations = anims.AddComponent<GameAnimations>();
        anims.AddComponent<GameAnimations>();
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

    public void LoadMainMenu()
    {
        gameRules.RemoveAllBoard();
        // gameRules = null;
        Destroy(gameRules.gameObject);
        SceneManager.LoadScene("Main Menu");
        UIController.Instance.GoToMainMenu();
    }

    public void ReloadGameMode()
    {
        Debug.Log("Reloading game mode");
        LoadGameMode(_lastGameModeArgs);
    }

    public void LoadGameModeAndScene(IGameMode gameMode, string args = "")
    {
        _lastGameModeArgs = args;
        var rules = gameMode.InitializeGameRules();
        gameRules = rules;
        // DontDestroyOnLoad(rules.gameObject);
        SceneManager.LoadScene("SampleScene");
        LoadGameMode(args);
    }

    public void LoadGameMode(string args = "")
    {
        Debug.Log("Loading game mode");
        _lastGameModeArgs = args;
        Timer = 0f;
        _undoActions.Clear();
        _redoActions.Clear();
        ActionsChanged();
        
        if (GameRules)
            GameRules.GameStart(args);
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