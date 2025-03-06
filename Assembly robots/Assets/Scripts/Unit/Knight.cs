using System;
using UnityEngine;

[RequireComponent(typeof(KnightMover),typeof(BaseBuilder))]

public class Knight : CreatableObject
{
    private readonly string _layerName = "Knight";

    [SerializeField] private Transform _holdPoint;

    private Coin _targetCoin;
    private Wallet _wallet;
    private Base _base;
    private BaseBuilder _baseBuilder;
    private KnightMover _mover;

    public event Action<Vector3> BaseBuildFinished;

    public bool IsBusy { get; private set; }
    public bool HasTargetCoin => _targetCoin;

    private void Awake()
    {
        IsBusy = false;
        _targetCoin = null;
        gameObject.layer = LayerMask.NameToLayer(_layerName);
        _mover = GetComponent<KnightMover>();
        _baseBuilder = GetComponent<BaseBuilder>();

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

    public void Initialize(Wallet wallet, Base @base)
    {
        if (wallet == null && @base == null)
            return;

        _wallet = wallet;
        _base = @base;

        _baseBuilder.SetTemplate(_base);
        ToFree();
    }

    public void ToBusy()
    {
        IsBusy = true;
    }

    public void ToFree()
    {
        IsBusy = false;
    }

    public void SetTargetCoin(Coin coin)
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

        newBase.AcceptKnight(this);
        BaseBuildFinished?.Invoke(newBase.SpawnPosition.position);
        ToFree();
    }
}