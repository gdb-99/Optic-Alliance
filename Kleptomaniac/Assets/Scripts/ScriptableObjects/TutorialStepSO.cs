using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/TutorialStep")]
public class TutorialStepSO : ScriptableObject
{
    public string title;
    public string info;

    public bool premiseScene;
}
