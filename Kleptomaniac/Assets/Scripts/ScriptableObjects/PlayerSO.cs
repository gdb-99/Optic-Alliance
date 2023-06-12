using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/PlayerData")]

public class PlayerSO : ScriptableObject
{
    public int reputation;
    public int money;

    public InventorySO backpackbackInv;
    public InventorySO globalInv;

    private void OnEnable()
    {
        reputation = 0;
        money = 100;
    }

    public void AddMoney(int valueToAdd)
    {
        if(valueToAdd > 0)
        {
            money += valueToAdd;
        }
    }

    /// <summary>
    /// Check if there is enough money, subtract the specified value and return the new money value
    /// If no enough money return -1
    /// </summary>
    /// <param name="valueToRemove">The value to subctract from money</param>
    /// <returns>The new money value or -1</returns>
    public int SubstractMoney(int valueToRemove)
    {
        if (valueToRemove <= money)
        {
            money -= valueToRemove;
            return money;
        }

        return -1;
    }

    public void IncreaseReputation()
    {
        reputation++;
    }
}
