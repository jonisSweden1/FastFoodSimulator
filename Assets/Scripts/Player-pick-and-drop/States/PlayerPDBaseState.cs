using UnityEngine;

// The base and current state
public abstract class PlayerPDBaseState
{
    public abstract void InitializeState(PlayerPickAndDropSystem manager);

    public abstract void EnterButton(PlayerPickAndDropSystem manager, Transform headTransform);
}