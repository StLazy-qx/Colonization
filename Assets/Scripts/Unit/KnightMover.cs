using System;
using System.Collections;
using UnityEngine;

public class KnightMover : MonoBehaviour
{
    [SerializeField] private Knight _knight;
    [SerializeField] private Transform _checkPoint;
    [SerializeField] private ObstacleMoveHandler _obstacleHandler;

    private Vector3 _collectionPosition;
    private Vector3 _baseBuildPosition;
    private Rigidbody _rigidbody;
    private float _moveSpeed = 25f;
    private float _rotationSpeed = 10f;
    private float _distanceToTarget = 0.15f;
    private float _distanceToFlag = 3f;

    public event Action<Vector3> FlagReached;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _obstacleHandler.Init(_rigidbody, _moveSpeed);
    }

    public void MoveToBuildBasePoint(Vector3 position)
    {
        _knight.ToBusy();

        Vector3 buildPoint = position +
            (transform.position - position).normalized * _distanceToFlag;

        _baseBuildPosition = new Vector3(position.x, 0, position.z);

        StartCoroutine(MoveSequence(buildPoint, OnFlagReached));
    }

    public void GoToTarget(Vector3 targetPosition)
    {
        if (_knight.IsBusy)
            return;

        _knight.ToBusy();
        _collectionPosition = transform.position;

        if (_knight.HasTargetCoin)
        {
            StartCoroutine(MoveSequence(targetPosition,
                () => _knight.PickUpCoin(), _collectionPosition,
                () => _knight.DropOffCoin(), ResetPosition));
        }
        else
        {
            _knight.ToFree();
        }
    }

    private IEnumerator MoveSequence(Vector3 target, Action onComplete)
    {
        yield return MoveToTarget(target);
        onComplete?.Invoke();
    }

    private IEnumerator MoveSequence(Vector3 target1, Action action1, Vector3 target2, Action action2, Action action3)
    {
        yield return MoveToTarget(target1);
        action1?.Invoke();

        yield return MoveToTarget(target2);
        action2?.Invoke();

        action3?.Invoke();
    }

    private IEnumerator MoveToTarget(Vector3 target)
    {
        Vector3 flatTarget = new Vector3(target.x, transform.position.y, target.z);

        while (transform.position.IsEnoughClose(flatTarget, _distanceToTarget) == false)
        {
            if (_obstacleHandler.IsObstacleOnWay
                (_checkPoint, out RaycastHit hit))
            {
                yield return _obstacleHandler.GoAroundObstacle(hit);
            }

            MoveTowards(flatTarget);

            yield return null;
        }
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;

        RotateTowards(direction);
        MoveForward();
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation
            (direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void MoveForward()
    {
        _rigidbody.MovePosition(transform.position +
            transform.forward * _moveSpeed * Time.deltaTime);
    }

    private void OnFlagReached()
    {
        FlagReached?.Invoke(_baseBuildPosition);
        _knight.ToFree();
    }

    private void ResetPosition()
    {
        transform.position = _collectionPosition;
        transform.rotation = Quaternion.identity;
    }
}