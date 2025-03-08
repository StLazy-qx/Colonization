using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Wallet))]

public class Base : CreatableObject
{
    private readonly int _clickOnBaseAnimation = Animator.StringToHash("ClickOnBase");

    [SerializeField] private CommonPoolResources _resourcePool;
    [SerializeField] private FlagPlacer _flagPlacer;
    [SerializeField] private KnightCreator _knightCreator;
    [SerializeField] private DeterminePointSpawner _determinePoint;
    [SerializeField] private Animator _animator;

    private Wallet _wallet;
    private Knight _lostKnight;
    private Vector3 _buildTargetPosition;
    private int _beginKnightsCount = 3;
    private int _minKnightsCount = 1;
    private bool _isCollectResourceToUnitsSpawned = true;

    private List<Knight> _knights = new List<Knight>();

    public event Action ModeChanged;

    public bool HasKnights => _knights.Count > _minKnightsCount;
    public Transform SpawnPosition => _determinePoint.Position;

    private void Awake()
    {
        _wallet = GetComponent<Wallet>();
    }

    private void Start()
    {
        InitialSquad();
    }

    private void OnEnable()
    {
        _wallet.NewBaseResourceSpended += SendKnightBuildBase;
        _wallet.NewUnitResourceSpended += CreateKnight;
        _flagPlacer.Disabled += EnableResourceCollectionToSpawnUnits;
    }

    private void OnDisable()
    {
        _wallet.NewBaseResourceSpended -= SendKnightBuildBase;
        _wallet.NewUnitResourceSpended -= CreateKnight;
        _flagPlacer.Disabled -= EnableResourceCollectionToSpawnUnits;
    }

    private void Update()
    {
        CollectResources();
    }

    public void PlayClikAnimation()
    {
        _animator.SetTrigger(_clickOnBaseAnimation);
    }

    public void InItializeBuild()
    {
        _beginKnightsCount = 0;

        _knights.Clear();
        EnableResourceCollectionToSpawnUnits();
    }

    public void SetBuildPosition(Vector3 position)
    {
        _buildTargetPosition = position;
    }

    public void AcceptKnight(Knight knight)
    {
        if (knight == null)
            return;

        _knights.Add(knight);
        knight.Initialize(_wallet, this);

        if (knight.TryGetComponent(out KnightMover mover))
        {
            mover.GoToTarget(_determinePoint.GetPosition());
        }
    }

    public void EnableResourceCollectionToSpawnUnits()
    {
        _isCollectResourceToUnitsSpawned = true;
    }

    public void DisableSpawnUnitsMode()
    {
        _isCollectResourceToUnitsSpawned = false;

        ModeChanged?.Invoke();
    }

    private void InitialSquad()
    {
        for (int i = 0; i < _beginKnightsCount; i++)
            CreateKnight();
    }

    private void CollectResources()
    {
        if (TryGetFreeKnight(out Knight knight))
        {
            if (_resourcePool.TryGetResource(out Coin coin))
            {
                knight.SetTargetCoin(coin);
            }
        }
    }

    private void SendKnightBuildBase()
    {
        if (HasKnights == false) 
            return;

        if (_isCollectResourceToUnitsSpawned == false)
        {
            if (TryGetFreeKnight(out Knight knight))
            {
                _lostKnight = knight;

                if (_lostKnight.TryGetComponent(out KnightMover knightMover))
                {
                    knightMover.MoveToBuildBasePoint(_buildTargetPosition);
                    _knights.Remove(_lostKnight);
                }
            }
        }
    }

    private bool TryGetFreeKnight(out Knight knight)
    {
        knight = _knights.FirstOrDefault
            (knight => knight.IsBusy == false);

        return knight != null;
    }

    private void CreateKnight()
    {
        Knight knight = (Knight)_knightCreator.
            Create(_determinePoint.GetPosition());

        knight.Initialize(_wallet, this);
        _knights.Add(knight);
    }
}