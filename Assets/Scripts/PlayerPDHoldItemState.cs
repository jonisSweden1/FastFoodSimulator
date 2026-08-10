using UnityEngine;

public class PlayerPDHoldItemState : PlayerPDBaseState
{
    public override void EnterButton(PlayerPickAndDropSystem manager, Transform headTransform)
    {
        RaycastHit hit;
        if(!Physics.Raycast(headTransform.position, headTransform.forward, out hit, manager.DropDistance))
        {
            return;
        }

        if(hit.collider.tag == "Item")
        {
            Transform item = hit.collider.transform.parent;

            if(item.TryGetComponent<ItemIdentifier>(out ItemIdentifier identifier))
            {
                if(identifier.CurrentType == TypeOfItem.Paper)
                {
                    if(manager.ItemObject.GetComponent<ItemIdentifier>().CurrentType == TypeOfItem.BottomBun)
                    {
                        item.GetComponent<PaperActivationBurgerStack>().AddBottomBun(manager.ItemObject.transform);
                        manager.ItemObject.SetActive(true);
                        manager.ItemObject = null;
                        manager.SwitchState(manager.nonItemState);
                        return;
                    }
                    else
                    {
                        return;
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