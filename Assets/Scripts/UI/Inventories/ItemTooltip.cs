using RPG.Inventories;
using TMPro;
using UnityEngine;

namespace RPG.UI.Inventories
{
    /// <summary>
    /// Root of the tooltip prefab to expose properties to other classes 
    /// </summary>
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _titleText;
        [SerializeField] TextMeshProUGUI _bodyText;

        public void Setup(InventoryItem item)
        {
            _titleText.text = item.GetDisplayName();
            _bodyText.text = item.GetDescription();
        }
    }
}