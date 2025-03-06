using UnityEngine;

public class BaseBuilder : ObjectCreator<Base>
{
    public void SetTemplate(Base @base)
    {
        if (@base == null)
            return;

        Template = @base;
    }

    public override CreatableObject Create(Vector3 position)
    {
        Base newBase = (Base)base.Create(position);
        newBase.InItializeBuild();

        return newBase;
    }
}