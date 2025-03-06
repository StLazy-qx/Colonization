using UnityEngine;

public class CameraOperator : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _minX = -30f;
    [SerializeField] private float _minZ = -17f;
    [SerializeField] private float _maxX = 30f;
    [SerializeField] private float _maxZ = 30f;

    private void Awake()
    {
        _playerInput.HorizontalamCameraMoving += OnHorizontalInput;
        _playerInput.VerticalCameraMoving += OnVerticalInput;
    }

    private void OnDisable()
    {
        _playerInput.HorizontalamCameraMoving -= OnHorizontalInput;
        _playerInput.VerticalCameraMoving -= OnVerticalInput;
    }

    private void MoveCamera(Vector3 movement)
    {
        Vector3 newPosition = transform.position + movement
            * _moveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, _minZ, _maxZ);
        transform.position = newPosition;
    }

    private void OnHorizontalInput(float horizontalInput)
    {
        Vector3 movement = new Vector3(-horizontalInput, 0f, 0f);

        MoveCamera(movement);
    }

    private void OnVerticalInput(float verticalInput)
    {
        Vector3 movement = new Vector3(0f, 0f, -verticalInput);

        MoveCamera(movement);
    }
}
