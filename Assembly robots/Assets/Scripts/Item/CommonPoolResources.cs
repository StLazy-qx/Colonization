using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommonPoolResources : MonoBehaviour
{
    private List<Coin> _discoveredResources = new();
    private List<Coin> _usedResources = new();

    public event Action ResourceToCollectioning;

    public void AddResource(Coin coin)
    {
        CleanUsedResources();

        if (coin == null
            || _discoveredResources.Contains(coin)
            || _usedResources.Contains(coin))
        {
            return;
        }

        _discoveredResources.Add(coin);
        ResourceToCollectioning?.Invoke();
    }

    public bool TryGetResource(out Coin coin)
    {
        coin = _discoveredResources.FirstOrDefault();

        if (coin == null)
            return false;

        _discoveredResources.Remove(coin);
        _usedResources.Add(coin);

        return true;
    }

    private void CleanUsedResources()
    {
        int limitCapacityList = 40;
        int numberCleanCoins = 25;

        if (_usedResources.Count >= limitCapacityList)
        {
            _usedResources.RemoveRange(0, numberCleanCoins);
        }
    }
}
