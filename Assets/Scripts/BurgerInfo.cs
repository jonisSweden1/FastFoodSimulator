using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurgerInfo : MonoBehaviour
{
    List<BurgerItemData> _burgerStackData;

    GameObject[] _burgerStack;

    BurgerStackSystem _burgerStackSystem;

    public void UnwrapPaper()
    {
        foreach (GameObject burgerItem in _burgerStack)
        {
            burgerItem.SetActive(true);
        }

        _burgerStackSystem.UnwrapPaper();

        Destroy(gameObject);
    }

    public void FillInfoData(GameObject[] burgerStack, BurgerStackSystem burgerStackSystem)
    {
        // Initialize the list of burger stack info

        _burgerStackData = new List<BurgerItemData>();

        _burgerStack = burgerStack;

        _burgerStackSystem = burgerStackSystem;

        // Go through each of the item in the GameObject[] burgerStack
        
        foreach (GameObject burgerItem in burgerStack)
        {
            // Check if it is an item, otherwise, check the name of the game object
            ItemIdentifier burgerItemId;

            if(!burgerItem.TryGetComponent<ItemIdentifier>(out burgerItemId))
            {
                // The name should include the corresponding points prefab

                // If it is a vegetable stack point, then check each of the limited items in it

                if (burgerItem.name.Contains("Vegetable-stacking-point"))
                {
                    Transform collectionPoint = burgerItem.transform.GetChild(0);

                    foreach (Transform child in collectionPoint)
                    {
                        GameObject vegetableInBurger = child.GetChild(0).gameObject;
                        ItemIdentifier vegetableInBurgerId = vegetableInBurger.GetComponent<ItemIdentifier>();

                        // Check if an item has the same name as the other item that exist to increase the amount of it.
                        if (_burgerStackData.Count > 0)
                        {
                            for (int i = 0; i < _burgerStackData.Capacity; i++)
                            {
                                if (_burgerStackData[i].Name == burgerItemId.ItemName)
                                {
                                    _burgerStackData[i].Amount++;
                                    break;
                                }
                            }
                        }

                        // If the item doesn't exist, then create a new item data
                        var burgerVegetableData = new BurgerItemData { Name = burgerItemId.ItemName, Amount = 1 };
                        _burgerStackData.Add(burgerVegetableData);
                    }

                    continue;
                }

                // If it is a sauce stack point, check every single item in it
                else if (burgerItem.name.Contains("SauceStackPoint"))
                {
                    Transform collectionPoint = burgerItem.transform.GetChild(0);

                    foreach (Transform child in collectionPoint)
                    {
                        GameObject sauceInBurger = child.gameObject;
                        ItemIdentifier sauceInBurgerId = sauceInBurger.GetComponent<ItemIdentifier>();

                        // Check if an item has the same name as the other item that exist to increase the amount of it.

                        // Check if an item has the same name as the other item that exist to increase the amount of it.
                        if (_burgerStackData.Count > 0)
                        {
                            for (int i = 0; i < _burgerStackData.Capacity; i++)
                            {
                                if (_burgerStackData[i].Name == sauceInBurgerId.ItemName)
                                {
                                    _burgerStackData[i].Amount++;
                                    break;
                                }
                            }
                            continue;
                        }

                        // If the item doesn't exist, then create a new item data
                        var burgerSauceData = new BurgerItemData { Name = burgerItemId.ItemName, Amount = 1 };
                        _burgerStackData.Add(burgerSauceData);
                    }

                    continue;
                }
                
            }

            // Check if an item has the same name as the other item that exist to increase the amount of it.
            if (_burgerStackData.Count > 0)
            {
                for(int i = 0; i < _burgerStackData.Count; i++)
                {
                    Debug.Log(_burgerStackData[i].Name);

                    if (_burgerStackData[i].Name == burgerItemId.ItemName)
                    {
                        _burgerStackData[i].Amount++;
                        break;
                    }
                }
                continue;
            }

            // If the item doesn't exist, then create a new item data
            var burgerData = new BurgerItemData { Name = burgerItemId.ItemName, Amount = 1};
            _burgerStackData.Add(burgerData);

        }
    }
}

public class BurgerItemData
{
    public string Name;
    public int Amount;
    // Fill other info later
}
