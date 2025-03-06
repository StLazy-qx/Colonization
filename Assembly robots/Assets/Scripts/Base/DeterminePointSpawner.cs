using UnityEngine;

public class DeterminePointSpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPlace;

    public Transform Position => _spawnPlace.transform;

    public Vector3 GetPosition()
    {
        float half = 0.5f;
        Vector3 center = _spawnPlace.position;

        float pointX = center.x + Random.Range
            (-_spawnPlace.localScale.x * half, _spawnPlace.localScale.x * half);
        float pointZ = center.z + Random.Range
            (-_spawnPlace.localScale.z * half, _spawnPlace.localScale.z * half);

        return new Vector3(pointX, _spawnPlace.position.y, pointZ);
    }
}
