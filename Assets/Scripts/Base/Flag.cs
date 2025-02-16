using System;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private float _baseSpawnDelay;

    private KnightMover _knightMover;
    private float _height = 0f;

    public event Action PlaceSellected;
    public event Action<Vector3> KnightMovementFinished;

    public bool IsActive => gameObject.activeSelf;

    private void Awake()
    {
        Deactivate();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        PlaceSellected?.Invoke();
    }

    public void GetDeliveryKnight(Knight knight)
    {
        if (knight == null)
            return;

        if (knight.TryGetComponent(out KnightMover knightMover))
        {
            _knightMover = knightMover;
            _knightMover.FlagReached += OnReactToKnight;
        }
    }

    private void OnReactToKnight()
    {
        Vector3 flagPosition = new Vector3(transform.position.x,
            _height, transform.position.z);

        KnightMovementFinished.Invoke(flagPosition);
        Reset();
    }

    private void Reset()
    {
        Deactivate();

        _knightMover.FlagReached -= OnReactToKnight;
        _knightMover = null;
    }
}
