using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform itemsParent;
    public GameObject itemSlotPrefab;

    private Inventory inventory;
    private bool isInventoryOpen = false;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isInventoryOpen)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        Time.timeScale = 0f; // Freeze the game
        Cursor.visible = true; // Show the cursor
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        isInventoryOpen = true;
        UpdateUI();
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        Time.timeScale = 1f; // Unfreeze the game
        Cursor.visible = false; // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        isInventoryOpen = false;
    }

    public void UpdateUI()
    {
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in inventory.items)
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab, itemsParent);
            Image iconImage = itemSlot.transform.Find("ItemIcon").GetComponent<Image>();
            Text nameText = itemSlot.transform.Find("ItemName").GetComponent<Text>();
            iconImage.sprite = item.icon;
            nameText.text = item.itemName;
        }
    }
}
