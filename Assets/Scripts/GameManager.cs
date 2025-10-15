using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using Modes;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private static UIManager UI => UIManager.Instance;
    
    [SerializeField] private GameRules gameRules;
    [SerializeField] private Sprite backCardSprite;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject slotPrefab;

    public float AutoMoveDelay = 3f;
    
    public bool Paused { get; private set; }
    public bool TimerPaused { get; set; }
    public float Timer { get; private set; }
    public bool DisableInteractions { get; set; }
    public int Moves { get; set; }
    
    public GameRules GameRules => gameRules;
    public GameObject SlotPrefab => slotPrefab;
    public GameObject CardPrefab => cardPrefab;
    public Sprite BackCardSprite => backCardSprite;

    public event OnActionsChangedArgs OnActionsChanged;
    public delegate void OnActionsChangedArgs(bool isUndoNotEmpty, bool isRedoNotEmpty);
    
    private string _lastGameModeArgs = string.Empty;
    private readonly List<Action> _undoActions = new();
    private readonly List<Action> _redoActions = new();
    private int _screenWidth;
    private int _screenHeight;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        EnhancedTouchSupport.Enable();
        
        // handling of native refresh rate on Android devices
        #if UNITY_ANDROID
        var refreshRate = Screen.currentResolution.refreshRateRatio.value;
        Application.targetFrameRate = (int)refreshRate;
        #endif
        
        var anims = new GameObject("Game Animations");
        anims.transform.SetParent(transform);
        anims.AddComponent<GameAnimations>();
        
        CalculateScreenRatio();
    }

    private void Start()
    {
        LoadGameMode();
    }

    private void Update()
    {
        if (!Paused && !TimerPaused)
            Timer += Time.deltaTime;
        CalculateScreenRatio();
    }

    private void CalculateScreenRatio()
    {
        if (_screenWidth == Screen.width && _screenHeight == Screen.height) return;
        
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
        
        var ratio = Screen.width / (float)Screen.height;
        Camera.main.orthographicSize = Mathf.Lerp(6f, 5f, Mathf.InverseLerp(1.4f, 1.7f, ratio));
    }

    public void LoadMainMenu()
    {
        gameRules.RemoveAllBoard();
        Destroy(gameRules.gameObject);
        SceneManager.LoadScene("Main Menu");
        UIController.Instance.GoToMainMenu();
    }

    public void ReloadGameMode()
    {
        // Debug.Log("Reloading game mode");
        LoadGameMode(_lastGameModeArgs);
    }

    public void LoadGameModeAndScene(IGameMode gameMode, string args = "")
    {
        _lastGameModeArgs = args;
        var rules = gameMode.InitializeGameRules();
        gameRules = rules;
        SceneManager.LoadScene("SampleScene");
        LoadGameMode(args);
    }

    public void LoadGameMode(string args = "")
    {
        // Debug.Log("Loading game mode");
        _lastGameModeArgs = args;
        Timer = 0f;
        Moves = 0;
        Paused = false;
        TimerPaused = false;
        DisableInteractions = false;
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
        DisableInteractions = paused;
        Time.timeScale = paused ? 0 : 1;
    }

    public void AddMove(Action undoMove)
    {
        _undoActions.Add(undoMove);
        _redoActions.Clear();
        ActionsChanged();
    }

    public void Undo()
    {
        if (_undoActions.Count <= 0) return;
        var action = _undoActions.Last();
        _undoActions.RemoveAt(_undoActions.Count - 1);
        action.Invoke();
        _redoActions.Add(action);
        GameRules.RefreshGetItDone();
        Moves++;
        ActionsChanged();
    }

    public void Redo()
    {
        if (_redoActions.Count <= 0) return;
        var action = _redoActions.Last();
        _redoActions.Remove(action);
        action.Invoke();
        _undoActions.Add(action);
        Moves++;
        ActionsChanged();
    }

    private void ActionsChanged()
    {
        OnActionsChanged?.Invoke(_undoActions.Count > 0, _redoActions.Count > 0);
    }
}