using UnityEngine;

public class ObjectCreator <T> : MonoBehaviour where T : CreatableObject
{
    [SerializeField] protected T Template;

    public virtual CreatableObject Create(Vector3 position)
    {
        if (Template != null)
        {
            var newObject = Instantiate(Template, position, Quaternion.identity);

            return newObject;
        }

        return null;
    }
}
