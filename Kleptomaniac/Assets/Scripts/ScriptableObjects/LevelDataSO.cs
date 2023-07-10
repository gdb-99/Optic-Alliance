using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Level")]
public class LevelDataSO : ScriptableObject
{
    public new string name;
    public string description;
    public int minReputationLevel;
    public int profit;
    public LevelCode code;
    public bool done;

    private void Awake()
    {
        done = false;
    }

    public enum LevelCode
    {
        TUTORIAL,
        FIRST,
        NONE,
    }
}
