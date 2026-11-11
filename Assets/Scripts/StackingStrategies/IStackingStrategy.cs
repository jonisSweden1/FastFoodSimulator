using UnityEngine;

public interface IStackingStrategy
{
    bool TryAddItemOnStack(GameObject item);
    bool TryRemoveFromStack(out GameObject item);
}
