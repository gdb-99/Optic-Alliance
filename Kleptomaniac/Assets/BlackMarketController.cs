using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMarketController : MonoBehaviour
{
    [SerializeField] BlackMarketSO _blackMarketData;
    [SerializeField] GameObject _prefabItem;
    [SerializeField] GameObject _permanentGrid;
    [SerializeField] GameObject _consumableGrid;

    void Start()
    {
        FillMarket();
    }

    public void FillMarket()
    {
        _blackMarketData.items.ForEach(item => {
            GameObject newItemInstance = Instantiate(_prefabItem);
            newItemInstance.transform.SetParent(item.isPermanent ? _permanentGrid.transform : _consumableGrid.transform, false);

            if(!newItemInstance.TryGetComponent<MenuItemController>(out var controller)) { return; }

            controller.SetItem(item);
        });
    }
}
