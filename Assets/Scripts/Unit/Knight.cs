using UnityEngine;

public class Knight : PoolableObject
{
    private readonly string _layerName = "Knight";

    [SerializeField] private Transform _holdPoint;

    private Coin _targetCoin;
    private Wallet _wallet;

    public bool IsBusy { get; private set; }
    public bool HasTargetCoin => _targetCoin;

    private void Awake()
    {
        IsBusy = false;
        _targetCoin = null;
        gameObject.layer = LayerMask.NameToLayer(_layerName);

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(_layerName),
            LayerMask.NameToLayer(_layerName));
    }

    public void Initialize(Wallet wallet)
    {
        _wallet = wallet;
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
}
