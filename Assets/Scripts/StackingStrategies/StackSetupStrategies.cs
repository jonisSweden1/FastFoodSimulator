using System.Collections.Generic;
using UnityEngine;

public class RowColumnStackSetupStrategy : IStackSetupStrategy
{
    public void SetupStack(Transform stackTransform, TypeOfBurgerStack allowedBurgerType, StackingSystem stackingSystem, List<Transform> stackedItems)
    {
        if (stackingSystem.transform.childCount > 1)
        {
            Transform collectionPoint = stackingSystem.transform.GetChild(1);
            foreach (Transform row in collectionPoint)
            {
                foreach (Transform pointItem in row)
                {
                    Debug.Log($"StackingSystem: Adding point item {pointItem.name} to stacked items.");
                    stackedItems.Add(pointItem);
                }
            }
        }
    }
}

public class RowStackSetupStrategy : IStackSetupStrategy
{
    public void SetupStack(Transform stackTransform, TypeOfBurgerStack allowedBurgerType, StackingSystem stackingSystem, List<Transform> stackedItems)
    {
        if (stackingSystem.transform.childCount > 1)
        {
            Transform collectionPoint = stackingSystem.transform.GetChild(1);
            foreach (Transform pointItem in collectionPoint)
            {
                Debug.Log($"StackingSystem: Adding point item {pointItem.name} to stacked items.");
                stackedItems.Add(pointItem);
            }
        }
    }
}
