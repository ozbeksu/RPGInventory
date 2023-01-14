using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = "RPG/Inventory/Equipable Item")]
    public class EquipableItem : InventoryItem
    {
        [Tooltip("Where are we allowed to put this item?")]
        [SerializeField] private EquipLocation _allowedEquipLocation = EquipLocation.Weapon;

        public EquipLocation GetAllowedEquipLocation()
        {
            return _allowedEquipLocation;
        }
    }
}
