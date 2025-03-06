using System;
using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private RaycastInteractor _raycastInteractor;
    [SerializeField] private Flag _flagTemplate;

    private Flag _flag;
    private Base _selectedBase;

    public event Action Disabled;

    private void Start()
    {
        _flag = Instantiate(_flagTemplate);
        _flag.gameObject.SetActive(false);
    }

    public void OnInstallFlag()
    {
        if (_raycastInteractor.TryGetRaycastHit(Input.mousePosition,
            out GameObject hitObject,
            out Vector3 hitPoint) == false)
        {
            return;
        }

        if (_selectedBase == null && 
            hitObject.TryGetComponent(out Base selectedBase))
        {
            selectedBase.PlayClikAnimation();

            if (selectedBase.HasKnights)
            {
                _selectedBase = selectedBase;
            }
        }
        else if (_selectedBase != null && 
            _raycastInteractor.IsValidPlacement(hitObject))
        {
            SetFlagPosition(hitPoint);

            _selectedBase.SetBuildPosition(hitPoint);
            _selectedBase.DisableResourceCollection();

            _selectedBase = null;
        }
        else if (_raycastInteractor.IsFlag(hitObject))
        {
            hitObject.gameObject.SetActive(false);
            Disabled?.Invoke();
        }
    }

    private void SetFlagPosition(Vector3 position)
    {
        float flagHeightOffset = _flag.transform.localScale.y;
        _flag.transform.position = new Vector3
            (position.x, position.y + flagHeightOffset, position.z);

        _flag.gameObject.SetActive(true);
    }
}
