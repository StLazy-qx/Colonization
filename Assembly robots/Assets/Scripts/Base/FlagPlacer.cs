using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private RaycastInteractor _raycastInteractor;

    private Flag _flag;
    private Base _selectedBase;

    public void OnInstallFlag()
    {
        if (_raycastInteractor.TryGetRaycastHit(Input.mousePosition,
            out GameObject hitObject, out Vector3 hitPoint) == false)
        {
            return;
        }

        if (_flag == null && hitObject.TryGetComponent(out Base @base))
        {
            _selectedBase = @base;
            _selectedBase.PlayClikAnimation();
            _flag = _selectedBase.GetFlag();
        }
        else if (_flag != null && _raycastInteractor.IsValidPlacement(hitObject))
        {
            _selectedBase.IncludeBuildMode();
            SetFlagPosition(hitPoint);
        }
        else if (_raycastInteractor.IsFlag(hitObject) &&
            hitObject.TryGetComponent(out Flag clickedFlag))
        {
            clickedFlag.gameObject.SetActive(false);
        }
    }

    private void SetFlagPosition(Vector3 position)
    {
        float flagHeightOffset = _flag.transform.localScale.y;

        _flag.transform.position = new Vector3
            (position.x, position.y + flagHeightOffset, position.z);
        _flag = null;
    }
}
