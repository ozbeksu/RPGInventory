using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = "RPG/Inventory/Action Item")]
    public class ActionItem : InventoryItem
    {
        [Tooltip("Is the action item is consumable?")]
        [SerializeField] private bool _consumable;

        public virtual void Use(GameObject user)
        {
            Debug.Log($"Using {name}");
        }

        public bool isConsumable()
        {
            return _consumable;
        }
    }
}
