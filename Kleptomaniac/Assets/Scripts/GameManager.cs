using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton Creation
    public static GameManager Instance { get; private set; }

    private GameManager()
    {
        // Private constructor to prevent instantiation from outside the class.
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
    }
    #endregion

    public bool quest1;
    public bool quest2;
    public bool quest3;

    public enum GameState
    {
        GameOver,
        Running,
        Pause,
        Clear
    }

    public event Action<GamePhase> OnGamePhaseChanged;

    public enum GamePhase
    {
        Theft,
        Escape
    }

    public GamePhase currentPhase;

    [SerializeField] PlayerSO _playerData;
    [SerializeField] LevelDataSO _levelData;

    void Start()
    {
        
        // Inizializza lo stato di gioco
        currentPhase = GamePhase.Theft;
        quest1 = false;
        quest2 = true;
        quest3 = true;
    }

    public PlayerSO PlayerData
    {
        get
        {
            return _playerData;
        }
    }

    public void SetPhaseTheft()
    {
        currentPhase = GamePhase.Theft;
    }

    public void SetPhaseEscape()
    {
        currentPhase = GamePhase.Escape;
    }

    public void SetGamePhase(GamePhase phase)
    {
        currentPhase = phase;

        // Se hai sottoscritto l'evento OnGamePhaseChanged, notifica gli ascoltatori del cambio di fase
        OnGamePhaseChanged?.Invoke(currentPhase);
    }

    public void EndLevel()
    {
        _playerData.AddMoney(_levelData.profit);
        _playerData.IncreaseReputation();
        _levelData.done = true;

        SceneManager.LoadScene("SelectLevelScene");
    }
}
