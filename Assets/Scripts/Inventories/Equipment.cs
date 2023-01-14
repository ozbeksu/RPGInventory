using RPG.Saving;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    public class Equipment : MonoBehaviour, ISaveable
    {
        Dictionary<EquipLocation, EquipableItem> _equippedItems = new();

        public event Action equipmentUpdated;

        public EquipableItem GetItemInSlot(EquipLocation equipLocation)
        {
            if (!_equippedItems.ContainsKey(equipLocation)) return null;

            return _equippedItems[equipLocation];
        }

        public void AddItem(EquipLocation slot, EquipableItem item)
        {
            Debug.Assert(item.GetAllowedEquipLocation() == slot);

            _equippedItems[(EquipLocation)slot] = item;

            if (equipmentUpdated != null)
            {
                equipmentUpdated();
            }
        }

        public void RemoveItem(EquipLocation slot)
        {
            _equippedItems.Remove((EquipLocation)slot);

            if (equipmentUpdated != null)
            {
                equipmentUpdated();
            }
        }

        public object CaptureState()
        {
            var equippedItemsForSerialization = new Dictionary<EquipLocation, string>();

            foreach (var pair in _equippedItems)
            {
                equippedItemsForSerialization[pair.Key] = pair.Value.GetItemID();
            }

            return equippedItemsForSerialization;
        }

        public void RestoreState(object state)
        {
            _equippedItems = new Dictionary<EquipLocation, EquipableItem>();

            var equippedItemsForSerialization = (Dictionary<EquipLocation, string>)state;

            foreach (var pair in equippedItemsForSerialization)
            {
                var item = (EquipableItem)InventoryItem.GetFromID(pair.Value);
                if (item != null)
                {
                    _equippedItems[pair.Key] = item;
                }
            }
        }
    }
}
