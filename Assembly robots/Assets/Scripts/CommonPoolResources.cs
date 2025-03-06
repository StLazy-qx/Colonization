using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommonPoolResources : MonoBehaviour
{
    private List<Coin> _discoveredResources = new();
    private List<Coin> _usedResources = new();

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
    }

    public bool TryGetResource(out Coin coin)
    {
        coin = null;

        foreach (Coin currentCoin in _discoveredResources)
        {
            if (_usedResources.Contains(currentCoin) == false)
            {
                coin = currentCoin;

                _discoveredResources.Remove(coin);
                _usedResources.Add(coin);

                return true;
            }
        }

        return false;
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
