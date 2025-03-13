using System;
using UnityEngine;

[RequireComponent(typeof(UnitMover),typeof(BaseBuilder))]

public class Knight : CreatableObject
{
    private readonly string _layerName = "Knight";

    [SerializeField] private Transform _holdPoint;

    private Coin _targetCoin;
    private Wallet _wallet;
    private Base _base;
    private BaseBuilder _baseBuilder;
    private UnitMover _mover;

    public event Action<Vector3> BaseBuildFinished;

    public bool IsBusy { get; private set; }

    private void Awake()
    {
        IsBusy = false;
        _targetCoin = null;
        gameObject.layer = LayerMask.NameToLayer(_layerName);
        _mover = GetComponent<UnitMover>();
        _baseBuilder = GetComponent<BaseBuilder>();

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(_layerName),
            LayerMask.NameToLayer(_layerName));
    }

    private void OnEnable()
    {
        _mover.FlagReached += BuildBase;
        _mover.GoingToSubjectFinished += PickUpCoin;
        _mover.CameBacked += DropOffCoin;
    }

    private void OnDisable()
    {
        _mover.FlagReached -= BuildBase;
        _mover.GoingToSubjectFinished -= PickUpCoin;
        _mover.CameBacked -= DropOffCoin;
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

    public void ToBusy() =>
        IsBusy = true;

    public void ToFree() =>
        IsBusy = false;

    public void SetTargetCoin(Coin coin)
    {
        if (IsBusy)
            return;

        _targetCoin = coin;

        ToBusy();
        VerifyCoin();
        _mover.GoToTarget(coin.transform.position);

        if(_targetCoin == null)
            ToFree();
    }

    private void PickUpCoin()
    {
        VerifyCoin();

        _targetCoin.transform.SetParent(_holdPoint);
        _targetCoin.SetHoldState(_holdPoint.position);
    }

    private void DropOffCoin()
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
        ToBusy();
        _mover.MoveToBuildBasePoint(buildPosition);

        Base newBase = (Base)_baseBuilder.Create(buildPosition);
        transform.position = newBase.SpawnPosition.position;

        newBase.AcceptKnight(this);
        ToFree();
    }
}