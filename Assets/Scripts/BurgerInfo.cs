using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurgerInfo : MonoBehaviour
{
    Dictionary<string, int> _burgerStackData;

    GameObject[] _burgerStack;

    BurgerStackSystem _burgerStackSystem;

    public void UnwrapPaper()
    {
        foreach (GameObject burgerItem in _burgerStack)
        {
            burgerItem.SetActive(true);
        }

        _burgerStackSystem.transform.position = transform.position;
        
        _burgerStackSystem.UnwrapPaper();

        PlayerHoverHandler.Instance.ResetHover();
        Destroy(gameObject);
    }

    public void FillInfoData(GameObject[] burgerStack, BurgerStackSystem burgerStackSystem)
    {
        _burgerStackData = new Dictionary<string, int>();

        foreach (GameObject burgerItem in burgerStack)
        {
            ItemIdentifier burgerItemId;

            if(!burgerItem.TryGetComponent<ItemIdentifier>(out burgerItemId))
            {
                if(burgerItem.name.Contains("Vegetable-stacking-points"))
                {
                    Transform collectionPoint = burgerItem.transform.GetChild(0);

                    Debug.Log(collectionPoint);

                    foreach (Transform vegetablePoint in collectionPoint)
                    {
                        if(vegetablePoint.childCount > 0)
                        {
                            GameObject vegetableBurgerItem = vegetablePoint.GetChild(0).gameObject;

                            ItemIdentifier vegetableBurgerItemId = vegetableBurgerItem.GetComponent<ItemIdentifier>();

                            if (_burgerStackData.ContainsKey(vegetableBurgerItemId.ItemName))
                            {
                                _burgerStackData[vegetableBurgerItemId.ItemName]++;
                            }
                            else
                            {
                                _burgerStackData.Add(vegetableBurgerItemId.ItemName, 1);
                            }
                        }
                    }
                }
                else if(burgerItem.name.Contains("SauceStackPoint"))
                {
                    Transform collectionPoint = burgerItem.transform.GetChild(0);

                    Debug.Log(collectionPoint);

                    foreach(Transform sauceBurgerItem in collectionPoint)
                    {
                        ItemIdentifier sauceBurgerItemId = sauceBurgerItem.GetComponent<ItemIdentifier>();

                        if (_burgerStackData.ContainsKey(sauceBurgerItemId.ItemName))
                        {
                            _burgerStackData[sauceBurgerItemId.ItemName]++;
                        }
                        else
                        {
                            _burgerStackData.Add(sauceBurgerItemId.ItemName, 1);
                        }
                    }
                }
            }
            else
            {
                if (burgerItemId.CurrentBurgerType == TypeOfBurgerStack.Paper)
                {
                    continue;
                }

                if (_burgerStackData.ContainsKey(burgerItemId.ItemName))
                {
                    _burgerStackData[burgerItemId.ItemName]++;
                }
                else
                {
                    _burgerStackData.Add(burgerItemId.ItemName, 1);
                }
            }
        }

        _burgerStack = burgerStack;

        _burgerStackSystem = burgerStackSystem;


        foreach (KeyValuePair<string, int> burgerInfo in _burgerStackData)
        {
            Debug.Log(burgerInfo.Key + ": " + burgerInfo.Value);
        }
    }
}
