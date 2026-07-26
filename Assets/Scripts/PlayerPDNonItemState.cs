using UnityEngine;

public class PlayerPDNonItemState : PlayerPDBaseState
{
    public override void EnterButton(PlayerPickAndDropSystem manager)
    {
        RaycastHit hit;
        if(!Physics.Raycast(manager.transform.position, manager.transform.forward, out hit, manager.PickDistance))
        {
            return;
        }
        
        if(hit.collider.gameObject == null)
        {
            return;
        }

        if(hit.collider.tag == "Item")
        {
            manager.ItemObject = hit.collider.gameObject;
            GameObject.Destroy(manager.ItemObject);
            manager.SwitchState(manager.holdItemState);
        }
    }

    public override void EnterState(PlayerPickAndDropSystem manager)
    {
        
    }

    public override void ExitState(PlayerPickAndDropSystem manager)
    {
        
    }

    public override void UpdateState(PlayerPickAndDropSystem manager)
    {
        
    }
}