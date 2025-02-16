using UnityEngine;

public abstract class PoolableObject : MonoBehaviour
{
    public bool IsActive => gameObject.activeSelf;

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
