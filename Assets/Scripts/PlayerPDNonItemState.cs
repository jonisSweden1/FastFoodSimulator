using UnityEngine;

public class PlayerPDNonItemState : PlayerPDBaseState
{
    public override void EnterButton(PlayerPickAndDropSystem manager)
    {
        RaycastHit hit;
        if(!Physics.Raycast(manager.transform.position, manager.transform.forward, out hit, manager.PickDistance))
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
            manager.ItemObject = hit.collider.gameObject;
            manager.ItemObject.SetActive(false);
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