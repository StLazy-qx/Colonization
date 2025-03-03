using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : PoolableObject
{
    private const float Half = 0.5f;

    [SerializeField] private Pool<T> _poolObjects;
    [SerializeField] private Transform _spawnPlace;

    protected Pool<T> PoolObjects => _poolObjects;

    public Transform SpawnPlace => _spawnPlace;

    private void Awake()
    {
        OnAwake();
        _poolObjects.Initialize();
    }

    protected Vector3 DetermineSpawnPoint()
    {
        Vector3 center = _spawnPlace.position;

        float pointX = center.x + Random.Range
            (-_spawnPlace.localScale.x * Half, _spawnPlace.localScale.x * Half);
        float pointZ = center.z + Random.Range
            (-_spawnPlace.localScale.z * Half, _spawnPlace.localScale.z * Half);

        return new Vector3(pointX, _spawnPlace.position.y, pointZ);
    }

    protected virtual void OnAwake() {}

    protected abstract void DistributeObject();
}