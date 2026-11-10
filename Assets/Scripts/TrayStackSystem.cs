using System.Collections.Generic;
using UnityEngine;

public class TrayStackSystem : MonoBehaviour
{
    Transform _collectionPoint;

    List<Transform> _stackedItems = new List<Transform>();

    private void Start()
    {
        if (transform.childCount > 1)
        {
            Transform collectionPoint = transform.GetChild(1);

            foreach (Transform row in collectionPoint)
            {
                foreach (Transform item in row)
                {
                    _stackedItems.Add(item);
                }
            }
        }
    }

    public void AddPattyToStack(GameObject burgerItem)
    {
        // Get the ItemIdentifier component from the burger item
        ItemIdentifier itemIdentifier = burgerItem.GetComponent<ItemIdentifier>();
        if (itemIdentifier != null)
        {
            if (itemIdentifier.CurrentType == TypeOfItem.BurgerItem)
            {
                if (itemIdentifier.CurrentBurgerType == TypeOfBurgerStack.Patty)
                {

                }
                else
                {
                    Debug.LogError($"TrayStackSystem: ItemIdentifier is not of type Patty on {burgerItem.name}.");
                }
            }
            else
            {
                Debug.LogError($"TrayStackSystem: ItemIdentifier component not found on {burgerItem.name}.");
            }
        }
    }
}
