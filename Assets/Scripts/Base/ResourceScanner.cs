using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private BaseSpawner _baseSpawner;
    [SerializeField] private float _radius;
    [SerializeField] private float _interval;

    private Coroutine _scanningCoroutine;
    private WaitForSeconds _delay;
    private Queue<Coin> _discoveredObjects = new Queue<Coin>();
    private HashSet<Coin> _uniqueObjects = new HashSet<Coin>();

    public event Action<int> ResourcesCounting;

    private void Awake()
    {
        _delay = new WaitForSeconds(_interval);
    }

    private void Start() => BeginScanning();

    private void DequeueResource()
    {
        if (_discoveredObjects.Count == 0)
            return;

        foreach (Base @base in _baseSpawner.GetListActiveObjects())
        {
            if (@base.TryGetFreeKnight(out Knight knight))
            {
                Vector3 newBasePosition = @base.transform.position;
                knight.TargetCoin(_discoveredObjects.Dequeue(), newBasePosition);
            }
        }
    }

    private void BeginScanning()
    {
        if (_scanningCoroutine != null)
        {
            StopCoroutine(_scanningCoroutine);
        }

        _scanningCoroutine = StartCoroutine(CheckRoutine());
    }

    private IEnumerator CheckRoutine()
    {
        while (gameObject.activeSelf)
        {
            CheckupMap();

            yield return _delay;
        }
    }

    private void CheckupMap()
    {
        CleanListUniqueObjects();

        Collider[] hits = Physics.OverlapSphere(transform.position, 
            _radius, _targetLayer);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Coin coin))
            {
                if (_uniqueObjects.Add(coin))
                {
                    _discoveredObjects.Enqueue(coin);
                }
            }
        }
        
        ResourcesCounting?.Invoke(_discoveredObjects.Count);
        DequeueResource();
    }

    private void CleanListUniqueObjects()
    {
        int _limitUniqueList = 40;
        int _maxQueueAmount = 20;

        _uniqueObjects.RemoveWhere(coin => coin == null);

        if (_uniqueObjects.Count >= _limitUniqueList)
        {
            while (_uniqueObjects.Count > _maxQueueAmount)
            {
                Coin removedCoin = _discoveredObjects.Dequeue();
                _uniqueObjects.Remove(removedCoin);
            }
        }
    }
}
