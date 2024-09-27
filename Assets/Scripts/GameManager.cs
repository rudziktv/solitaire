using System;
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
}