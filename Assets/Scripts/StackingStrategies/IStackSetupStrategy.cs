using System.Collections.Generic;
using UnityEngine;

public interface IStackSetupStrategy
{
    public void SetupStack(Transform stackTransform, TypeOfBurgerStack allowedBurgerType, StackingSystem stackingSystem, List<Transform> stackedItems);
}
