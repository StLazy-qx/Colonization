using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommonPoolResources : MonoBehaviour
{
    private List<Coin> _discoveredResources = new();
    private List<Coin> _usedResources = new();

    public event Action ResourceToCollectioning;

    //public bool CanGiveResource => _discoveredResources.Count > 0;

    public void AddResource(Coin coin)
    {
        CleanUsedResources();

        if (coin != null)
        {
            if (_discoveredResources.Contains(coin) == false 
                && _usedResources.Contains(coin) == false)
            {
                ResourceToCollectioning?.Invoke();
                _discoveredResources.Add(coin);
            }
        }
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
