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

        manager.ItemObject.transform.position = hit.point;
        manager.ItemObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        manager.ItemObject.SetActive(true);

        manager.ItemObject = null;

        manager.SwitchState(manager.nonItemState);
    }
}