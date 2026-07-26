using UnityEngine;

public class PlayerPDHoldItemState : PlayerPDBaseState
{
    public override void EnterButton(PlayerPickAndDropSystem manager)
    {
        RaycastHit hit;
        if(!Physics.Raycast(manager.transform.position, manager.transform.forward, out hit, manager.DropDistance))
        {
            return;
        }

        manager.ItemObject.transform.position = hit.point;
        manager.ItemObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        manager.ItemObject.SetActive(true);

        manager.ItemObject = null;

        manager.SwitchState(manager.nonItemState);
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