using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool<T> : MonoBehaviour where T : PoolableObject
{
    [SerializeField] private T _template;
    [SerializeField] private Transform _container;
    [SerializeField] private int _capacity;

    private List<T> _objectsPool = new List<T>();

    protected T Template => _template;
    protected List<T> ObjectsPool => _objectsPool;
    protected Transform Container => _container;

    public void Initialize()
    {
        if (_capacity > 0)
        {
            for (int i = 0; i < _capacity; i++)
                CreateObject();
        }
    }

    public T GetObject(Vector3 position)
    {
        T newObject = _objectsPool.FirstOrDefault(subject => subject.IsActive == false);

        if (newObject == null)
            newObject = CreateObject();

        ActivateObject(newObject, position);

        return newObject;
    }

    public bool IsAllObjectsActive()
    {
        return _objectsPool.All(subject => subject.IsActive);
    }

    protected virtual T CreateObject()
    {
        T newObject = Instantiate(_template, _container);

        newObject.Deactivate();
        _objectsPool.Add(newObject);

        return newObject;
    }

    private void ActivateObject(T subject, Vector3 position)
    {
        subject.transform.position = position;

        subject.Activate();
    }
}
