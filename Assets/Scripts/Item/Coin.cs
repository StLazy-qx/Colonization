using UnityEngine;

public class Coin : PoolableObject
{
    [SerializeField] private float _speedRotation;

    private bool _isRotating;
    private Transform _coinPoolContaner;
    private Quaternion _beginRotation = Quaternion.Euler(90f, 0f, 0f);
    private Quaternion _layRotation = Quaternion.Euler(0f, 0f, 0f);

    private void Start()
    {
        SetBeginState();
    }

    private void Update()
    {
        if (_isRotating)
            Rotate();
    }

    public void StopHolded()
    {
        transform.SetParent(_coinPoolContaner.transform);
        SetBeginState();
        Deactivate();
    }

    public void Initialize(Transform contaner)
    {
        _coinPoolContaner = contaner;
    }

    public void SetHoldState(Vector3 position)
    {
        _isRotating = false;
        transform.position = position;
        transform.rotation = _layRotation;
    }

    private void SetBeginState()
    {
        transform.rotation = _beginRotation;
        _isRotating = true;
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.forward * _speedRotation * Time.deltaTime);
    }
}
