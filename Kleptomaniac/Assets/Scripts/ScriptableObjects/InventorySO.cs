using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class InvetoryData
{
    public ItemSO data;
    public int counter;
}

[CreateAssetMenu(menuName = "GameData/Inventory")]
public class InventorySO : ScriptableObject
{

    public List<InvetoryData> items = new();
    public ItemSO[] items2;

    public int totalItems = 0;

    private void OnEnable()
    {
        items.Clear();
        totalItems= 0;
    }

    public void AddItem(ItemSO item)
    {
        
        var findData = items.FirstOrDefault(x => x.data.id == item.id);

        if (findData.IsUnityNull())
        {
            items.Add(new() { counter = 1, data = item });
            
            
        }
        else findData.counter++;

        totalItems++;

    }

    public void RemoveItem(ItemSO item)
    {

        var findDataIndex = items.FindIndex(x => x.data.id == item.id);

        if (findDataIndex > -1)
        {
            var itemData = items[findDataIndex];

            if (items[findDataIndex].counter > 1) {
                items[findDataIndex].counter--;
            }
            else
            {
                items.RemoveAt(findDataIndex);
            }

            totalItems--;

        }

    }

    public bool ContainsItem(ItemSO item) {
        foreach(InvetoryData invetoryData in items) {
            if(invetoryData.data.id == item.id) {
                return true;
            }
        }
        return false;
    }

}
