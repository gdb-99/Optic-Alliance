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
    [SerializeField] MissionCompletedUIController missionCompletedUI;

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

    public LevelDataSO LevelData
    {
        get
        {
            return _levelData;
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
        //Reward solo la prima volta che il livello viene completato
        if (_levelData.done == false)
        {
            _playerData.AddMoney(_levelData.profit);
            _playerData.IncreaseReputation();
        }
        
        _levelData.done = true;

        int completedQuest = 0;

        if (quest1)
        {
            completedQuest++;
        }

        if (quest2)
        {
            completedQuest++;
        }

        if (quest3)
        {
            completedQuest++;
        }

        missionCompletedUI.ShowVictory(completedQuest);

        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void GoToSelectLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("SelectLevelScene");
    }
}
