using UnityEngine;

public class RaycastInteractor : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _restrictedLayer;
    [SerializeField] private LayerMask _flagLayer;

    private Camera _mainCamera;

    public void Awake()
    {
        _mainCamera = Camera.main;
    }

    public bool TryGetRaycastHit(Vector3 screenPosition, 
        out GameObject hitObject, out Vector3 hitPoint)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            hitObject = hit.collider.gameObject;
            hitPoint = hit.point;

            return true;
        }

        hitObject = null;
        hitPoint = Vector3.zero;

        return false;
    }

    public bool IsValidPlacement(GameObject @object)
    {
        return IsOnLayer(@object, _groundLayer) && 
            IsOnLayer(@object, _restrictedLayer) == false;
    }

    public bool IsFlag(GameObject @object)
    {
        return IsOnLayer(@object, _flagLayer);
    }

    private bool IsOnLayer(GameObject @object, LayerMask layerMask)
    {
        return ((1 << @object.layer) & layerMask) != 0;
    }
}
