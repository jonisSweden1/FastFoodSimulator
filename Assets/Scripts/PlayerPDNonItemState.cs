using UnityEngine;

public class PlayerPDNonItemState : PlayerPDBaseState
{
    public override void EnterButton(PlayerPickAndDropSystem manager, Transform headTransform)
    {
        RaycastHit hit;
        if(!Physics.Raycast(headTransform.position, headTransform.forward, out hit, manager.PickDistance))
        {
            Debug.LogError($"Raycast could hit anything.");
            return;
        }
        
        if(hit.collider.gameObject == null)
        {
            Debug.LogError($"Object couldn't be found");
            return;
        }

        if(hit.collider.tag == "Item")
        {
            manager.ItemObject = hit.collider.transform.parent.gameObject;
            manager.ItemObject.SetActive(false);
            manager.SwitchState(manager.holdItemState);
        }
    }
}