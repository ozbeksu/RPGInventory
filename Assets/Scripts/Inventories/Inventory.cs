using RPG.Saving;
using System;
using UnityEngine;

namespace RPG.Inventories
{
    /// <summary>
    /// Provides storage for the player inventory. A configurable number of
    /// slots are available.
    ///
    /// This component should be placed on the GameObject tagged "Player".
    /// </summary>
    public class Inventory : MonoBehaviour, ISaveable
    {
        // CONFIG DATA
        [Tooltip("Allowed size")]
        [SerializeField] int inventorySize = 16;

        // STATE
        InventorySlot[] slots;

        struct InventorySlot
        {
            public InventoryItem item;
            public int count;
        }

        [System.Serializable]
        struct InventorySlotRecord
        {
            public string itemID;
            public int count;
        }

        // PUBLIC

        /// <summary>
        /// Broadcasts when the items in the slots are added/removed.
        /// </summary>
        public event Action inventoryUpdated;

        /// <summary>
        /// Convenience for getting the player's inventory.
        /// </summary>
        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Inventory>();
        }

        /// <summary>
        /// Could this item fit anywhere in the inventory?
        /// </summary>
        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }

        /// <summary>
        /// How many slots are in the inventory?
        /// </summary>
        public int GetSize()
        {
            return slots.Length;
        }

        /// <summary>
        /// Attempt to add the items to the first available slot.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>Whether or not the item could be added.</returns>
        public bool AddToFirstEmptySlot(InventoryItem item, int count)
        {
            int i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }

            slots[i].item = item;
            slots[i].count += count;

            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }

            return true;
        }

        /// <summary>
        /// Is there an instance of the item in the inventory?
        /// </summary>
        public bool HasItem(InventoryItem item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Return the item type in the given slot.
        /// </summary>
        public InventoryItem GetItemInSlot(int slot)
        {
            return slots[slot].item;
        }

        /// <summary>
        /// Return the item type in the given slot.
        /// </summary>
        public int GetItemCountInSlot(int slot)
        {
            return slots[slot].count;
        }

        /// <summary>
        /// Remove the item from the given slot.
        /// </summary>
        public void RemoveFromSlot(int slot, int count)
        {

            slots[slot].count -= count;

            if (slots[slot].count <= 0)
            {
                slots[slot].count = 0;
                slots[slot].item = null;
            }

            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
        }

        /// <summary>
        /// Will add an item to the given slot if possible. If there is already
        /// a stack of this type, it will add to the existing stack. Otherwise,
        /// it will be added to the first empty slot.
        /// </summary>
        /// <param name="slot">The slot to attempt to add to.</param>
        /// <param name="item">The item type to add.</param>
        /// <returns>True if the item was added anywhere in the inventory.</returns>
        public bool AddItemToSlot(int slot, InventoryItem item, int count)
        {
            if (slots[slot].item != null)
            {
                return AddToFirstEmptySlot(item, count);
            }

            var i = FindStack(item);
            if (i >= 0)
            {
                slot = i;
            }

            slots[slot].item = item;
            slots[slot].count += count;

            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }

            return true;
        }

        // PRIVATE

        private void Awake()
        {
            slots = new InventorySlot[inventorySize];
        }

        /// <summary>
        /// Find a slot that can accomodate the given item.
        /// </summary>
        /// <returns>-1 if no slot is found.</returns>
        private int FindSlot(InventoryItem item)
        {
            int i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }

            return i;
        }

        private int FindStack(InventoryItem item)
        {
            if (!item.IsStackable()) return -1;

            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Find an empty slot.
        /// </summary>
        /// <returns>-1 if all slots are full.</returns>
        private int FindEmptySlot()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    return i;
                }
            }

            return -1;
        }

        object ISaveable.CaptureState()
        {
            var slotRecords = new InventorySlotRecord[inventorySize];

            for (int i = 0; i < inventorySize; i++)
            {
                if (slots[i].item != null)
                {
                    slotRecords[i].itemID = slots[i].item.GetItemID();
                    slotRecords[i].count = slots[i].count;
                }
            }

            return slotRecords;
        }

        void ISaveable.RestoreState(object state)
        {
            var slotRecords = (InventorySlotRecord[])state;

            for (int i = 0; i < inventorySize; i++)
            {
                slots[i].item = InventoryItem.GetFromID(slotRecords[i].itemID);
                slots[i].count = slotRecords[i].count;
            }

            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
        }
    }
}