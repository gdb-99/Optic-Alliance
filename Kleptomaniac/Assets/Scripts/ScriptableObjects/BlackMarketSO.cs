using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/BlackMarket")]
public class BlackMarketSO : ScriptableObject
{
    public List<ItemSO> items;
}
