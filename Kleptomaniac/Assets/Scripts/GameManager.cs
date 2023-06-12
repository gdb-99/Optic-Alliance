using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] PlayerSO _playerData;
    public PlayerSO PlayerData
    {
        get
        {
            return _playerData;
        }
    }


}
