using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapPinController : MonoBehaviour
{
    [SerializeField] Texture2D hoverCursor;
    [SerializeField] LevelDataSO level;
    [SerializeField] TextMeshProUGUI levelName;

    // Start is called before the first frame update
    void Start()
    {
        levelName.text = level.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        SelectLevelManager.Instance.DisplayLevelInfo(level);
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }
}
