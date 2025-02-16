using System;
using UnityEngine;

[RequireComponent(typeof(Wallet))]

public class Base : PoolableObject
{
    private readonly int _clickOnBaseAnimation = Animator.StringToHash("ClickOnBase");

    [SerializeField] private KnightSpawner _knightSpawner;
    [SerializeField] private Flag _flagTemplate;
    [SerializeField] private Animator _animator;

    private Wallet _wallet;
    private Flag _flag;
    private Knight _lostKnight;
    private int _minKnightCoount = 1;

    public event Action ModeChanged;
    public event Action<Vector3, Knight> NewBaseCreating;

    public Wallet Wallet => _wallet;

    private void Awake()
    {
        _flag = Instantiate(_flagTemplate,transform);
        _wallet = GetComponent<Wallet>();
    }

    private void OnEnable()
    {
        _wallet.NewBaseResourceSpended += SendKnightToNewBase;
        _wallet.NewUnitResourceSpended += _knightSpawner.CreateNewUnit;
        _flag.PlaceSellected += OnSetMode;
        _flag.KnightMovementFinished += OnSpawnNewBase;
    }

    private void OnDisable()
    {
        _wallet.NewBaseResourceSpended -= SendKnightToNewBase;
        _wallet.NewUnitResourceSpended -= _knightSpawner.CreateNewUnit;
        _flag.PlaceSellected -= OnSetMode;
        _flag.KnightMovementFinished -= OnSpawnNewBase;
    }

    public void BuildTemplate(Vector3 buildPosition, Knight knight)
    {
        transform.position = buildPosition;

        AcceptKnight(knight);
    }

    public bool TryGetFreeKnight(out Knight knight)
    {
        knight = null;

        if (_knightSpawner.TryGetFreeUnit(out Knight freeknight))
        {
            knight = freeknight;

            return true;
        }

        return false;
    }

    public void PlayClikAnimation()
    {
        _animator.SetTrigger(_clickOnBaseAnimation);
    }

    public Flag GetFlag()
    {
        if (_knightSpawner.ActiveObjectsCount > _minKnightCoount)
        {
            return _flag;
        }

        return null;
    }

    public void DisableKnightSpawning()
    {
        _knightSpawner.DisableBeginSpawning();
    }

    private void SendKnightToNewBase()
    {
        if (_knightSpawner.ActiveObjectsCount <= _minKnightCoount)
            return;

        _knightSpawner.TryGetFreeUnit(out Knight freeKnight);

        Vector3 newBasePosition = _flag.transform.position;
        _lostKnight = freeKnight;

        _flag.GetDeliveryKnight(_lostKnight);

        if (_lostKnight.TryGetComponent(out KnightMover knightMover))
            knightMover.MoveToNewBase(newBasePosition);
    }

    private void AcceptKnight(Knight knight)
    {
        if (knight != null)
            _knightSpawner.AddKnight(knight);
    }

    private void OnSpawnNewBase(Vector3 flagPosition)
    {
        NewBaseCreating?.Invoke(flagPosition, _lostKnight);
        _knightSpawner.RemoveUnit(_lostKnight);

        _lostKnight = null;
    }

    private void OnSetMode()
    {
        if (_flag.IsActive)
            ModeChanged?.Invoke();
    }
}
