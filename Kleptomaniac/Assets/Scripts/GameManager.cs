using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;


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

    public enum GameState
    {
        GameOver,
        Running,
        Pause,
        Clear
    }

    public enum GamePhase
    {
        Theft,
        Escape
    }

    public GamePhase currentPhase;

    [SerializeField] PlayerSO _playerData;

    void Start()
    {
        // Inizializza lo stato di gioco
        currentPhase = GamePhase.Theft;
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
}
