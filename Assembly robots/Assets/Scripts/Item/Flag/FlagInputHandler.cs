using UnityEngine;

public class FlagInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private FlagPlacer _flagPlacer;

    private void OnEnable()
    {
        _input.FlagModeSetting += OnInstallFlag;
    }

    private void OnDisable()
    {
        _input.FlagModeSetting -= OnInstallFlag;
    }

    private void OnInstallFlag()
    {
        _flagPlacer.OnInstallFlag();
    }
}
