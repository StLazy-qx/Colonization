using System;
using UnityEngine;

[RequireComponent(typeof(Wallet), (typeof(KnightCollection)))]

public class Base : CreatableObject
{
    private readonly int _clickOnBaseAnimation = Animator.StringToHash("ClickOnBase");

    [SerializeField] private KnightCreator _knightCreator;
    [SerializeField] private DeterminePointSpawner _determinePoint;
    [SerializeField] private BaseBuilder _baseBuilder;
    [SerializeField] private BaseCollection _baseCollection;
    [SerializeField] private Flag _flagTemplate;
    [SerializeField] private Animator _animator;

    private Wallet _wallet;
    private Flag _flag;
    private KnightCollection _knightCollector;
    private Knight _lostKnight;
    private int _beginKnightsCount = 3;

    public event Action ModeChanged;

    private void Awake()
    {
        _flag = Instantiate(_flagTemplate,transform);
        _wallet = GetComponent<Wallet>();
        _knightCollector = GetComponent<KnightCollection>();

        _baseBuilder.SetTemplate(this);

        CloseBuildMode();
    }

    private void Start()
    {
        _baseCollection.Add(this);
        InitialSquad();
    }

    private void OnEnable()
    {
        _wallet.NewBaseResourceSpended += SendKnightBuildBase;
        _wallet.NewUnitResourceSpended += CreateKnight;
    }

    private void OnDisable()
    {
        _wallet.NewBaseResourceSpended -= SendKnightBuildBase;
        _wallet.NewUnitResourceSpended -= CreateKnight;

        if (_lostKnight != null)
            _lostKnight.BaseBuilding -= CloseBuildMode;
    }

    public void SetBuild()
    {
        _beginKnightsCount = 0;
        _knightCollector.Clear();
        CloseBuildMode();

        Debug.Log("Произошел ресет базы");
    }

    public bool TryGetFreeKnight(out Knight knight)
    {
        return _knightCollector.TryGetFreeKnight(out knight);
    }

    public void PlayClikAnimation()
    {
        _animator.SetTrigger(_clickOnBaseAnimation);
    }

    public Flag GetFlag()
    {
        return _knightCollector.HasKnights() ? _flag : null;
    }

    public void AcceptKnight(Knight knight)
    {
        if (knight == null)
            return;

        _knightCollector.Add(knight);
        knight.Initialize(_wallet, _baseBuilder, _baseCollection);

        if (knight.TryGetComponent(out KnightMover mover))
        {
            mover.GoToTarget(_determinePoint.GetPosition());
        }
    }

    public void IncludeBuildMode()
    {
        ModeChanged?.Invoke();
        _flag.gameObject.SetActive(true);
    }

    public void CloseBuildMode()
    {
        _flag.gameObject.SetActive(false);
    }

    private void InitialSquad()
    {
        _knightCollector.Clear();

        for (int i = 0; i < _beginKnightsCount; i++)
            CreateKnight();
    }

    private void SendKnightBuildBase()
    {
        if (_knightCollector.HasKnights() == false)
            return;

        if (_knightCollector.TryGetFreeKnight(out Knight knight))
        {
            _lostKnight = knight;
            _lostKnight.BaseBuilding += CloseBuildMode;

            if (_lostKnight.TryGetComponent(out KnightMover knightMover))
            {
                knightMover.MoveToBuildBasePoint(_flag.transform.position);
                _knightCollector.RemoveObject(_lostKnight);
            }
        }
    }

    private void CreateKnight()
    {
        Knight knight = (Knight)_knightCreator.
            Create(_determinePoint.GetPosition());

        knight.Initialize(_wallet, _baseBuilder, _baseCollection);
        _knightCollector.Add(knight);
    }
}