using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform itemsParent;
    public GameObject itemSlotPrefab;

    public Text itemNameText;
    public Text itemDescriptionText;

    private Inventory inventory;
    private bool isInventoryOpen = false;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        inventoryPanel.SetActive(false);
        Cursor.visible = false; // Hide cursor at start
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor at start
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

        if (isInventoryOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseInventory();
        }
    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        Time.timeScale = 0f; // Freeze the game
        isInventoryOpen = true;
        Cursor.visible = true; // Show cursor when inventory is open
        Cursor.lockState = CursorLockMode.None; // Unlock cursor when inventory is open
        UpdateUI();
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        Time.timeScale = 1f; // Unfreeze the game
        isInventoryOpen = false;
        Cursor.visible = false; // Hide cursor when inventory is closed
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor when inventory is closed
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
            Button itemButton = itemSlot.GetComponent<Button>();
            itemButton.onClick.AddListener(() => ShowItemDetails(item));
            Image iconImage = itemSlot.transform.Find("ItemIcon").GetComponent<Image>();
            Text nameText = itemSlot.transform.Find("ItemName").GetComponent<Text>();
            iconImage.sprite = item.icon;
            nameText.text = item.itemName;
        }
    }

    public void ShowItemDetails(InventoryItem item)
    {
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;
    }
}
