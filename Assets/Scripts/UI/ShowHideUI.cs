using UnityEngine;

namespace RPG.UI.Inventories
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] private GameObject _uiContainer = null;
        [SerializeField] private KeyCode _toggleKey = KeyCode.I;

        private void Awake()
        {
            if (_uiContainer != null)
            {
                _uiContainer.SetActive(false);
            }
        }

        private void Update()
        {
            if (_uiContainer == null) return;

            if (Input.GetKeyDown(_toggleKey))
            {
                _uiContainer.SetActive(!_uiContainer.activeSelf);
            }
        }
    }
}
