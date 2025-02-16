using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool<T> : MonoBehaviour where T : PoolableObject
{
    [SerializeField] private T _template;
    [SerializeField] private Transform _container;
    [SerializeField] private int _capacity;

    private List<T> _objectsPool = new List<T>();

    public int CountActiveObjects => GetListActiveObjects().Count;
    public int Count => _objectsPool.Count;

    protected T Template => _template;
    protected List<T> ObjectsPool => _objectsPool;
    protected Transform Container => _container;

    public void Initialize()
    {
        if (_capacity > 0)
        {
            for (int i = 0; i < _capacity; i++)
                CreateNewObject();
        }
    }

    public T GetObject(Vector3 position)
    {
        T newObject = _objectsPool.FirstOrDefault(subject => subject.IsActive == false);

        if (newObject == null)
            newObject = CreateNewObject();

        ActivateObject(newObject, position);

        return newObject;
    }

    public List<T> GetListActiveObjects()
    {
        return _objectsPool.Where(subject => subject.IsActive).ToList();
    }

    public void AddUnit(T unit)
    {
        if (unit != null)
        {
            _objectsPool.Add(unit);
            unit.transform.SetParent(_container);
        }
    }

    public void RemoveUnit(T unit)
    {
        if (unit != null)
        {
            _objectsPool.Remove(unit);
            unit.transform.SetParent(null);
        }
    }

    public bool IsAllObjectsActive()
    {
        return _objectsPool.All(subject => subject.IsActive);
    }

    public List<T> GetListObjects()
    {
        return new List<T>(_objectsPool);
    }

    protected virtual T CreateNewObject()
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
