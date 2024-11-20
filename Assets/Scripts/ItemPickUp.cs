using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;
    public float maxDistance;
    private bool isPlayerInRange = false;
    private GameObject player;

    private void Update()
    {
        if (isPlayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.G)) PickupItem();

            if (player != null)
            {
                float dist = Vector3.Distance(player.transform.position, transform.position);
                if (dist > maxDistance)
                {
                    isPlayerInRange = false;
                    player = null;
                    Debug.Log("Player out of range");
                } 
            }
            else
            {
                Debug.LogError("Player are not detected!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            isPlayerInRange = true;

            Debug.Log("Press 'G' to pick up the item!");
        }
    }

    private void PickupItem()
    {
        Inventory inventory = FindAnyObjectByType<Inventory>();
        if (inventory != null)
        {
            if (inventory.AddItem(item))
            {
                Debug.Log($"Picked up {item.itemName}!");
                Destroy(gameObject); 
            }
            else
            {
                Debug.Log("Inventory is full!");
            }
        }
    }
}
