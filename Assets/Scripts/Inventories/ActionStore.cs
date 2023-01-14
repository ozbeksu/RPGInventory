using RPG.Saving;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    public class ActionStore : MonoBehaviour, ISaveable
    {
        private class DockedItemSlot
        {
            public ActionItem item;
            public int number;
        }

        private Dictionary<int, DockedItemSlot> _dockedItems = new();

        public event Action storeUpdated;

        public ActionItem GetAction(int index)
        {
            if (!_dockedItems.ContainsKey(index)) return null;

            return _dockedItems[index].item;
        }

        public int GetNumber(int index)
        {
            if (!_dockedItems.ContainsKey(index)) return 0;

            return _dockedItems[index].number;
        }

        public bool Use(int index, GameObject user)
        {
            if (!_dockedItems.ContainsKey(index)) return false;

            _dockedItems[index].item.Use(user);

            if (_dockedItems[index].item.isConsumable())
            {
                RemoveItems(index, 1);
            }

            return true;
        }

        public void AddAction(InventoryItem item, int index, int number)
        {
            if (_dockedItems.ContainsKey(index))
            {
                if (object.ReferenceEquals(item, _dockedItems[index]))
                {
                    _dockedItems[index].number += number;
                }
            }
            else
            {
                _dockedItems[index] = new DockedItemSlot
                {
                    item = item as ActionItem,
                    number = number
                };
            }

            if (storeUpdated != null)
            {
                storeUpdated();
            }
        }

        public void RemoveItems(int index, int number)
        {
            if (!_dockedItems.ContainsKey(index)) return;

            _dockedItems[index].number -= number;

            if (_dockedItems[index].number <= 0)
            {
                _dockedItems.Remove(index);
            }

            if (storeUpdated != null)
            {
                storeUpdated();
            }
        }

        public int MaxAcceptable(InventoryItem item, int index)
        {
            var actionItem = item as ActionItem;

            if (!actionItem) return 0;
            if (_dockedItems.ContainsKey(index) && !object.ReferenceEquals(item, _dockedItems[index].item)) return 0;
            if (actionItem.isConsumable()) return int.MaxValue;
            if (_dockedItems.ContainsKey(index)) return 0;

            return 1;
        }

        [System.Serializable]
        private struct DockedItemRecord
        {
            public string itemID;
            public int number;
        }

        public object CaptureState()
        {
            var state = new Dictionary<int, DockedItemRecord>();

            foreach (var pair in _dockedItems)
            {
                state[pair.Key] = new DockedItemRecord
                {
                    itemID = pair.Value.item.GetItemID(),
                    number = pair.Value.number
                };
            }

            return state;
        }

        public void RestoreState(object state)
        {
            var stateDict = state as Dictionary<int, DockedItemRecord>;

            foreach (var pair in stateDict)
            {
                AddAction(InventoryItem.GetFromID(pair.Value.itemID), pair.Key, pair.Value.number);
            }
        }
    }
}
