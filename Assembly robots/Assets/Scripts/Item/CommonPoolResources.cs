using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommonPoolResources : MonoBehaviour
{
    private List<Coin> _discoveredResources = new();
    private List<Coin> _usedResources = new();

    public void Add(Coin coin)
    {
        if (coin != null)
        {
            if (_discoveredResources.Contains(coin) == false 
                && _usedResources.Contains(coin) == false)
            {
                _discoveredResources.Add(coin);
            }
        }
    }

    public bool TryGetResource(out Coin coin)
    {
        CleanUsedResources();

        coin = _discoveredResources.FirstOrDefault();

        if (coin == null)
        {
            coin = null;

            return false;
        }

        _discoveredResources.Remove(coin);
        _usedResources.Add(coin);

        return true;
    }

    private void CleanUsedResources()
    {
        int limitCapacityList = 5;

        if (_usedResources.Count > limitCapacityList)
            _usedResources.Clear();
    }
}
