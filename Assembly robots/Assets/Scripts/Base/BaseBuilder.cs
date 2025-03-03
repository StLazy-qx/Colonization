using UnityEngine;

public class BaseBuilder : ObjectCreator<Base>  
{
    [SerializeField] private Base _baseTemplate;

    public override CreatableObject Create(Vector3 position)
    {
        if (_baseTemplate == null)
        {
            Debug.LogError("Base template is not set!");
            return null;
        }

        Base newBase = Instantiate(_baseTemplate, position, Quaternion.identity);
        newBase.SetBuild(); // Обнуляем рыцарей и выключаем флаг

        return newBase;
    }
}