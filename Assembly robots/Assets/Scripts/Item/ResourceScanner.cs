using System.Collections;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private CommonPoolResources _resourcePool;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _radius;
    [SerializeField] private float _interval;

    private Coroutine _scanningCoroutine;
    private WaitForSeconds _delay;

    private void Awake()
    {
        _delay = new WaitForSeconds(_interval);
    }

    private void Start()
    {
        BeginScanning();
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
        Collider[] hits = Physics.OverlapSphere(
            transform.position, _radius, _targetLayer);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Coin coin))
            {
                _resourcePool.Add(coin);
            }
        }
    }
}
