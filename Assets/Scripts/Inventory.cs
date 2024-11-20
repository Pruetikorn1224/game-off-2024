using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public Image[] inventorySlots;
    public int maxInventorySize = 5;

    public bool AddItem(Item item)
    {
        if (items.Count < maxInventorySize)
        {
            items.Add(item);
            UpdateInventoryUI();
            Debug.Log("Add " + item.itemName + " to inventory.");
            return true;
        }
        else
        {
            Debug.Log("Inventory is already full.");
            return false;
        }
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            UpdateInventoryUI();
        }
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < items.Count)
            {
                inventorySlots[i].sprite = items[i].itemIcon;
                inventorySlots[i].enabled = true;
            }
            else
            {
                inventorySlots[i].sprite = null;
                inventorySlots[i].enabled = false;
            }
        }
    }
}
