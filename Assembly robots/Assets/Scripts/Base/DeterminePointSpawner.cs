using UnityEngine;

public class DeterminePointSpawner : MonoBehaviour
{
    private const float Half = 0.5f;

    [SerializeField] private Transform _spawnPlace;

    public Transform SpawnPlace => _spawnPlace;

    public Vector3 GetPosition()
    {
        Vector3 center = _spawnPlace.position;

        float pointX = center.x + Random.Range
            (-_spawnPlace.localScale.x * Half, _spawnPlace.localScale.x * Half);
        float pointZ = center.z + Random.Range
            (-_spawnPlace.localScale.z * Half, _spawnPlace.localScale.z * Half);

        return new Vector3(pointX, _spawnPlace.position.y, pointZ);
    }
}
