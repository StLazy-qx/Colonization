using System.Collections;
using UnityEngine;

public class ObstacleMoveHandler : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _moveSpeed;
    private float _angleRotation = 100f;
    private float _maxAvoidanceDistance = 1f;
    private float _detectObstacleDistance = 1.5f;

    public void Init(Rigidbody rigidbody, float moveSpeed)
    {
        _rigidbody = rigidbody;
        _moveSpeed = moveSpeed;
    }

    public bool IsObstacleOnWay(Transform checkPoint, out RaycastHit hit)
    {
        return Physics.Raycast(checkPoint.position, transform.forward,
            out hit, _detectObstacleDistance) &&
                hit.collider.gameObject.TryGetComponent<Base>(out _);
    }

    public IEnumerator GoAroundObstacle(RaycastHit hit)
    {
        float traveledDistance = 0f;

        PerformAvoidanceRotation(hit);

        while (traveledDistance < _maxAvoidanceDistance)
        {
            _rigidbody.MovePosition(transform.position +
            transform.forward * _moveSpeed * Time.deltaTime);

            traveledDistance += _moveSpeed * Time.deltaTime;

            yield return null;
        }
    }

    private void PerformAvoidanceRotation(RaycastHit hit)
    {
        Vector3 obstacleNormal = hit.normal;
        float angle = Vector3.SignedAngle(transform.forward,
            obstacleNormal, Vector3.up);

        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y +
            (angle >= 0 ? _angleRotation : -_angleRotation), 0);
    }
}
