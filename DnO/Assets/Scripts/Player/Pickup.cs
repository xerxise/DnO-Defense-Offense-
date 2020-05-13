using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    private Inventory inventory;
    public GameObject itemButton;
    private void Start()
    {
       inventory = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("PLAYER")) {
            for (int i = 0; i < inventory.items.Length; i++)
            {
                if (inventory.items[i] == 0)
                { 
                    inventory.items[i] = 1;
                    Instantiate(itemButton, inventory.slots[i].transform, false);
                    Destroy(gameObject);
                    break;
                }
                
            }
        }
    }
}
