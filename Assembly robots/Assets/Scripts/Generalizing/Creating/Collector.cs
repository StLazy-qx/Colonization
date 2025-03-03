using System.Collections.Generic;
using UnityEngine;

public class Collector<T> : MonoBehaviour where T : CreatableObject
{
    private readonly List<T> _objectsList = new List<T>();

    [SerializeField] private Transform _itemsContainer;

    public void Add(T @object)
    {
        if (@object == null)
            return;

        _objectsList.Add(@object);
        @object.transform.SetParent(_itemsContainer);
    }

    public void RemoveObject(T @object)
    {
        if (@object == null)
            return;

        _objectsList.Remove(@object);
        @object.transform.SetParent(null);
    }

    public void Clear()
    {
        foreach (T @object in _objectsList)
        {
            if (@object != null)
                Object.Destroy(@object.gameObject);
        }

        _objectsList.Clear();
    }

    public List<T> GetActiveObjects()
    {
        return _objectsList;
    }

    public int Count => _objectsList.Count;
}
