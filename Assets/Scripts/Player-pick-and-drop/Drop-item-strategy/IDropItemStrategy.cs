using UnityEngine;

public interface IDropItemStrategy
{
    public void DropItem(PlayerPickAndDropSystem manager, Transform raycastItem, Transform holdItem, ItemIdentifier raycastItemId, ItemIdentifier holdItemId);
}
