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
        inventory.OnInventoryChanged += UpdateUI; // Subscribe to the inventory change event
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
        isInventoryOpen = true;
        UpdateUI();
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
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

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged -= UpdateUI; // Unsubscribe to avoid memory leaks
        }
    }
}
