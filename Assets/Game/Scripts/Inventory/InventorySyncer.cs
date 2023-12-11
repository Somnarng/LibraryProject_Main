using MoreMountains.InventoryEngine;
using MoreMountains.Tools;
using UnityEngine;

public class InventorySyncer : MonoBehaviour, MMEventListener<MMInventoryEvent>
{
    [SerializeField] private int HotbarSize;
    [SerializeField] private InventorySlot[] hotbarSlots;
    [SerializeField] private Inventory TargetInventory;
    private void Start()
    {
        OnInventoryChanged();
    }
   void OnInventoryChanged()
    {
        for (int i = 0; i < HotbarSize; i++)
        {
            hotbarSlots[i].DrawIcon(TargetInventory.Content[i], i);
            Debug.Log("Hotbar Updated");
        }
    }

    /// <summary>
    /// When we catch a MMInventoryEvent, we filter on its type and display info about the item used
    /// </summary>
    /// <param name="inventoryEvent"></param>
    public virtual void OnMMEvent(MMInventoryEvent inventoryEvent)
    {
        if(inventoryEvent.TargetInventoryName != TargetInventory.name) { return; }
        else if (inventoryEvent.InventoryEventType == MMInventoryEventType.ContentChanged)
        {
            OnInventoryChanged();
        }
    }

    /// <summary>
    /// On enable we start listening for MMInventoryEvents 
    /// </summary>
    protected virtual void OnEnable()
    {
        this.MMEventStartListening<MMInventoryEvent>();
    }

    /// <summary>
    /// On disable we stop listening for MMInventoryEvents 
    /// </summary>
    protected virtual void OnDisable()
    {
        this.MMEventStopListening<MMInventoryEvent>();
    }
}