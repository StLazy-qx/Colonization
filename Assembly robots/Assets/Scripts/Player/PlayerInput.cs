using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private readonly string _horizontalCameraMove = "Horizontal";
    private readonly string _verticalCameraMove = "Vertical";
    private readonly int _keySetNewBaseCreationMode = 0;

    private float _deltaMove = 0.01f;

    public event Action FlagModeSetting;
    public event Action<float> HorizontalamCameraMoving;
    public event Action<float> VerticalCameraMoving;

    private void Update()
    {
        float moveX = Input.GetAxis(_horizontalCameraMove);
        float moveZ = Input.GetAxis(_verticalCameraMove);

        if (Input.GetMouseButtonDown(_keySetNewBaseCreationMode))
        {
            FlagModeSetting?.Invoke();
        }

        if (Mathf.Abs(moveX) > _deltaMove || Mathf.Abs(moveZ) > _deltaMove)
        {
            HorizontalamCameraMoving?.Invoke(moveX);
            VerticalCameraMoving?.Invoke(moveZ);
        }
    }
}
