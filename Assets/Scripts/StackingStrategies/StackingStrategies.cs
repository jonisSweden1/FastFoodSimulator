using UnityEngine;

public class RowColumnStackingStrategy : IStackingStrategy
{
    public bool TryAddItemOnStack(GameObject item)
    {
        // Implementation for row-column stacking strategy
        return false;
    }

    public bool TryRemoveFromStack(out GameObject item)
    {
        // Implementation for removing item from stack
        item = null;
        return false;
    }
}

public class RowStackingStrategy : IStackingStrategy
{
    public bool TryAddItemOnStack(GameObject item)
    {
        // Implementation for row stacking strategy
        return false;
    }
    public bool TryRemoveFromStack(out GameObject item)
    {
        // Implementation for removing item from stack
        item = null;
        return false;
    }
}
