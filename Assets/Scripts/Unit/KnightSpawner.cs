using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KnightSpawner : Spawner<Knight>
{
    [SerializeField] private Wallet _wallet;

    private float _distanceBetweenPoint = 0.15f;
    private bool _isShouldSpawnKnights = true;
    private List<Vector3> _spawnPoints = new List<Vector3>();

    public int ActiveObjectsCount => PoolObjects.CountActiveObjects;


    private void Start()
    {
        DistributeObjects();
    }

    public bool TryGetFreeUnit(out Knight knight)
    {
        knight = PoolObjects.GetListActiveObjects().
            FirstOrDefault(knight => knight.IsBusy == false);

        return knight != null;
    }

    public void CreateNewUnit()
    {
        Vector3 newPoint = DetermineSpawnCoordinate();

        _spawnPoints.Add(newPoint);

        Knight knight = PoolObjects.GetObject(newPoint);

        knight.Initialize(_wallet);
    }

    public void AddKnight(Knight knight)
    {
        if (knight != null && PoolObjects.GetListObjects()
            .Contains(knight) == false)
        {
            PoolObjects.AddUnit(knight);
        }
    }

    public void RemoveUnit(Knight knight)
    {
        if (knight != null)
            PoolObjects.RemoveUnit(knight);
    }

    public void DisableBeginSpawning()
    {
        _isShouldSpawnKnights = false;
    }


    protected override void DistributeObjects()
    {
        if (_isShouldSpawnKnights == false)
            return;

        GenerateSpawnPoints();

        foreach (Vector3 position in _spawnPoints)
        {
            Knight knight = PoolObjects.GetObject(position);

            knight.Initialize(_wallet);
        }
    }

    private void GenerateSpawnPoints()
    {
        _spawnPoints.Clear();

        for (int i = 0; i < PoolObjects.Count; i++)
        {
            Vector3 newPoint = DetermineSpawnCoordinate();

            if (IsPointValid(newPoint))
                _spawnPoints.Add(newPoint);
            else
                i--;
        }
    }

    private bool IsPointValid(Vector3 point)
    {
        foreach (Vector3 existingPoint in _spawnPoints)
        {
            if (existingPoint.IsEnoughClose(point, _distanceBetweenPoint))
                return false;
        }

        return true;
    }
}
