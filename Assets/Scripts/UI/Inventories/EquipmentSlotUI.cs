using RPG.Core.UI.Dragging;
using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] private InventoryItemIcon _icon = null;
        [SerializeField] private EquipLocation _equipLocation = EquipLocation.Weapon;

        Equipment _playerEquipmemt;

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _playerEquipmemt = player.GetComponent<Equipment>();
            _playerEquipmemt.equipmentUpdated += RedrawUI;
        }

        private void Start()
        {
            RedrawUI();
        }

        public void AddItems(InventoryItem item, int number)
        {
            _playerEquipmemt.AddItem(_equipLocation, (EquipableItem)item);
        }

        public InventoryItem GetItem()
        {
            return _playerEquipmemt.GetItemInSlot(_equipLocation);
        }

        public int GetNumber()
        {
            if (GetItem() == null) return 0;

            return 1;
        }

        public int MaxAcceptable(InventoryItem item)
        {
            EquipableItem equipableItem = item as EquipableItem;

            if (equipableItem == null) return 0;
            if (equipableItem.GetAllowedEquipLocation() != _equipLocation) return 0;
            if (GetItem() != null) return 0;

            return 1;
        }

        public void RemoveItems(int number)
        {
            _playerEquipmemt.RemoveItem(_equipLocation);
        }

        private void RedrawUI()
        {
            var item = _playerEquipmemt.GetItemInSlot(_equipLocation);

            if (item == null) return;

            _icon.SetItem(item, MaxAcceptable(item));
        }
    }
}

