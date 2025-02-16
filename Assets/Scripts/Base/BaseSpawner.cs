using System.Collections;
using UnityEngine;

public class BaseSpawner : Spawner<Base>
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private int _cooldown;

    private Knight _knight;
    private Vector3 _newBasePosition;
    private WaitForSeconds _delay;
    private bool _isFirstBaseCreated = false;

    private void Start()
    {
        if (_isFirstBaseCreated == false)
        {
            CreateFirstBase();

            _isFirstBaseCreated = true;
        }
    }

    private void OnEnable()
    {
        foreach (Base @base in PoolObjects.GetListObjects())
        {
            @base.NewBaseCreating += OnPutNewBase;
        }
    }

    private void OnDisable()
    {
        foreach (Base @base in PoolObjects.GetListObjects())
        {
            @base.NewBaseCreating -= OnPutNewBase;
        }
    }

    protected override void OnAwake()
    {
        _delay = new WaitForSeconds(_cooldown);
    }

    protected override void DistributeObjects()
    {
        Base newBase = PoolObjects.GetObject(_newBasePosition);
        newBase.DisableKnightSpawning();

        if (_knight != null)
        {
            _knight.Initialize(newBase.Wallet);
            newBase.BuildTemplate(_newBasePosition, _knight);
        }

        ResetUnpitParameters();
    }

    private void OnPutNewBase(Vector3 position, Knight knight)
    {
        if (knight != null)
        {
            _knight = knight;
            _newBasePosition = position;
            StartCoroutine(DelayCreation());
        }
    }

    private void CreateFirstBase()
    {
        Vector3 centerPosition = SpawnPlace.position;
        PoolObjects.GetObject(centerPosition);
    }

    private IEnumerator DelayCreation()
    {
        yield return _delay;

        DistributeObjects();
    }

    private void ResetUnpitParameters()
    {
        _knight = null;
        _newBasePosition = Vector3.zero;
    }
}
