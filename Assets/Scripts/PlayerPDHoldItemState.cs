using UnityEngine;
using UnityEngine.Identifiers;

public class PlayerPDHoldItemState : PlayerPDBaseState
{
    public override void EnterButton(PlayerPickAndDropSystem manager, Transform headTransform)
    {
        RaycastHit hit;
        if (!Physics.Raycast(headTransform.position, headTransform.forward, out hit, manager.DropDistance))
        {
            return;
        }

        if (hit.collider.tag == "Item")
        {
            Transform raycastItem = hit.collider.transform.parent;
            Transform holdItem = manager.ItemObject.transform;

            ItemIdentifier raycastItemId = raycastItem.GetComponent<ItemIdentifier>();
            ItemIdentifier holdItemId = holdItem.GetComponent<ItemIdentifier>();

            if (raycastItemId.CurrentType == TypeOfItem.BurgerItem)
            {
                if (raycastItemId.CurrentBurgerType == TypeOfBurgerStack.Paper)
                {
                    if (holdItemId.CurrentBurgerType == TypeOfBurgerStack.BottomBun)
                    {
                        raycastItem.GetComponent<PaperActivationBurgerStack>().AddBottomBun(holdItem);
                        manager.ItemObject.SetActive(true);

                        raycastItemId.HasAddedBurgerItemToStack(holdItem.gameObject);
                    }
                }
            }
        }

        manager.ItemObject.transform.position = hit.point;
        manager.ItemObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        manager.ItemObject.SetActive(true);

        manager.ItemObject = null;

        manager.SwitchState(manager.nonItemState);
    }
}