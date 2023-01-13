using RPG.Inventories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Inventories
{
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        [SerializeField] GameObject _textContainer = null;
        [SerializeField] TextMeshProUGUI _itemCount = null;

        public void SetItem(InventoryItem item, int count)
        {
            Image iconImage = GetComponent<Image>();
            if (item == null)
            {
                iconImage.enabled = false;

            }
            else
            {
                iconImage.sprite = item.GetIcon();
                iconImage.enabled = true;
            }

            if (_itemCount)
            {
                if (count > 1)
                {
                    _itemCount.text = count.ToString();
                    _textContainer.SetActive(true);
                }
                else
                {
                    _textContainer.SetActive(false);
                }
            }
        }
    }
}