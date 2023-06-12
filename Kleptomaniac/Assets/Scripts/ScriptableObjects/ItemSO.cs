using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Item")]
public class ItemSO : ScriptableObject
{
    public bool isPermanent;
    public string id;
    public new string name;
    public string description;
    public float price;
    public GameObject itemModel;
    public Sprite itemSprite;

    public ItemAction action;

    public enum ItemAction
    {
        LIGHT,
        DISABLE_CAMERA,
        MAP,
    }
}
