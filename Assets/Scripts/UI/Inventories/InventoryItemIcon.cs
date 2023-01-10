using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Inventories
{
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        public void SetItem(Sprite item)
        {
            Image iconImage = GetComponent<Image>();
            if (item == null)
            {
                iconImage.enabled = false;
            }
            else
            {
                iconImage.sprite = item;
                iconImage.enabled = true;
            }
        }

        public Sprite GetItem()
        {
            Image iconImage = GetComponent<Image>();

            if (!iconImage.enabled) return null;

            return iconImage.sprite;
        }
    }
}