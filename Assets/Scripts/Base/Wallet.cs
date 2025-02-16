using System;
using System.Collections;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private int _unitPrice = 3;
    [SerializeField] private int _basePrice = 5;

    private int _coinCount;
    public bool _isModeNewBase;

    public event Action<int> BalanceChanged;
    public event Action NewUnitResourceSpended;
    public event Action NewBaseResourceSpended;

    private void Awake()
    {
        _coinCount = 0;
        _isModeNewBase = false;
    }

    private void Start()
    {
        BalanceChanged?.Invoke(_coinCount);
    }

    private void Update()
    {
        if (_isModeNewBase == false)
            SpendCoinToNewObject(_unitPrice, NewUnitResourceSpended);
    }

    private void OnEnable()
    {
        _base.ModeChanged += OnBayNewBase;
    }

    private void OnDisable()
    {
        _base.ModeChanged -= OnBayNewBase;
    }

    public void AddCoin()
    {
        _coinCount++;

        BalanceChanged?.Invoke(_coinCount);
    }

    private void OnBayNewBase()
    {
        _isModeNewBase = true;

        StartCoroutine(AccumulateForBase());
    }

    private IEnumerator AccumulateForBase()
    {
        while (_coinCount < _basePrice)
        {
            yield return null;
        }

        SpendCoinToNewObject(_basePrice, NewBaseResourceSpended);

        _isModeNewBase = false;
    }

    private void SpendCoinToNewObject(int value, Action action)
    {
        if (CanSpendCoin(value) == false)
            return;

        _coinCount -= value;

        action?.Invoke();
        BalanceChanged?.Invoke(_coinCount);
    }

    private bool CanSpendCoin(int value)
    {
        return _coinCount >= value;
    }
}
