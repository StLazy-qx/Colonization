using System;
using UnityEngine;

[RequireComponent(typeof(KnightMover))]

public class Knight : CreatableObject
{
    private readonly string _layerName = "Knight";

    [SerializeField] private Transform _holdPoint;

    private Coin _targetCoin;
    private Wallet _wallet;
    private BaseBuilder _baseBuilder;
    private BaseCollection _baseCollection;
    private KnightMover _mover;

    public event Action BaseBuilding;

    public bool IsBusy { get; private set; }
    public bool HasTargetCoin => _targetCoin;

    private void Awake()
    {
        IsBusy = false;
        _targetCoin = null;
        gameObject.layer = LayerMask.NameToLayer(_layerName);
        _mover = GetComponent<KnightMover>();

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(_layerName),
            LayerMask.NameToLayer(_layerName));
    }

    private void OnEnable()
    {
        _mover.FlagReached += BuildBase;
    }

    private void OnDisable()
    {
        _mover.FlagReached -= BuildBase;
    }

    public void Initialize(Wallet wallet,
        BaseBuilder builder, BaseCollection baseCollection)
    {
        if (wallet == null)
            return;

        if (builder == null)
            return;        
        
        if (baseCollection == null)
            return;

        _wallet = wallet;
        _baseBuilder = builder;
        _baseCollection = baseCollection;
    }

    public void ToBusy()
    {
        IsBusy = true;
    }

    public void ToFree()
    {
        IsBusy = false;
    }

    public void TargetCoin(Coin coin, Vector3 collectionPoint)
    {
        VerifyCoin();

        if (this.TryGetComponent(out KnightMover knightMover))
        {
            _targetCoin = coin;
            knightMover.GoToTarget(coin.transform.position);
        }
    }

    public void PickUpCoin()
    {
        VerifyCoin();

        _targetCoin.transform.SetParent(_holdPoint);
        _targetCoin.SetHoldState(_holdPoint.position);
    }

    public void DropOffCoin()
    {
        VerifyCoin();
        _targetCoin.StopHolded();
        _wallet.AddCoin();

        IsBusy = false;
        _targetCoin = null;
    }

    private void VerifyCoin()
    {
        if (_targetCoin == null)
            return;
    }

    private void BuildBase(Vector3 buildPosition)
    {
        Base newBase = (Base)_baseBuilder.Create(buildPosition);

        if (newBase != null)
        {
            _baseCollection.Add(newBase);
            BaseBuilding?.Invoke();
            newBase.AcceptKnight(this);
        }
    }
}