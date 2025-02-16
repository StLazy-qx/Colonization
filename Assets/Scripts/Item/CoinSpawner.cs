using System.Collections;
using UnityEngine;

public class CoinSpawner : Spawner<Coin>
{
    [SerializeField] private float _cooldown;
    [SerializeField] private Base _base;
    [SerializeField] private float _distanceAroundBase = 8f;

    private float _heightSpawned = 0.8f;
    private WaitForSeconds _delay;

    private void Start()
    {
        StartCoroutine(PerformProduction());
    }

    protected override void OnAwake()
    {
        _delay = new WaitForSeconds(_cooldown);
    }

    protected override void DistributeObjects()
    {
        Vector3 spawnPosition = DetermineSpawnCoordinate();

        if (IsValidPoint(spawnPosition))
        {
            Vector3 newPostion = new Vector3(spawnPosition.x,
                _heightSpawned, spawnPosition.z);

            PoolObjects.GetObject(newPostion);
        }
    }

    private IEnumerator PerformProduction()
    {
        while (PoolObjects.IsAllObjectsActive() == false)
        {
            DistributeObjects();

            yield return _delay;
        }
    }

    private bool IsValidPoint(Vector3 point)
    {
        return _base.transform.position.
            IsEnoughClose(point, _distanceAroundBase) == false;
    }
}
