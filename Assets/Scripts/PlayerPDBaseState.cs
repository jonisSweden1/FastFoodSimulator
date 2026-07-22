using UnityEngine;

public abstract class PlayerPDBaseState
{
    public abstract void EnterState(PlayerPickAndDropSystem manager);

    public abstract void ExitState(PlayerPickAndDropSystem manager);
    public abstract void EnterButton(PlayerPickAndDropSystem manager);

    public abstract void UpdateState(PlayerPickAndDropSystem manager);
    
}