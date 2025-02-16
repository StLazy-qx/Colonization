using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _restrictedLayer;
    [SerializeField] private LayerMask _flagLayer;
    
    private Flag _flag;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _input.FlagModeSetting += OnTryPlaceFlag;
    }

    private void OnDisable()
    {
        _input.FlagModeSetting -= OnTryPlaceFlag;
    }

    private void OnTryPlaceFlag()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) == false)
            return;

        GameObject hitObject = hit.collider.gameObject;

        if (_flag == null && hit.collider.TryGetComponent(out Base tempBase))
        {
            tempBase.PlayClikAnimation();
            _flag = tempBase.GetFlag();
        }
        else if (_flag != null && IsValidPlacement(hitObject))
        {
            SetFlag(hit.point);
        }
        else if (IsOnLayer(hitObject, _flagLayer) && 
            hit.collider.TryGetComponent(out Flag clickedFlag))
        {
            clickedFlag.Deactivate();
        }
    }

    private bool IsValidPlacement(GameObject gameObject)
    {
        return IsOnLayer(gameObject, _groundLayer) && 
            IsOnLayer(gameObject, _restrictedLayer) == false;
    }

    private void SetFlag(Vector3 position)
    {
        float flagHeightOffset = _flag.transform.localScale.y;

        _flag.Activate();

        _flag.transform.position = new Vector3(position.x, position.y + flagHeightOffset, position.z);
        _flag = null;
    }

    private bool IsOnLayer(GameObject obj, LayerMask layerMask)
    {
        return ((1 << obj.layer) & layerMask) != 0;
    }
}
