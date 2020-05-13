using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {


    private Inventory inventory;
    private Transform PalyerWeapon;
    public int index;

    private void Start()
    {
        
        inventory = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Inventory>();
        //PalyerWeapon = GameObject.FindGameObjectWithTag("R_hand_container").transform;
    }

    private void LateUpdate()
    {
         
        if (transform.childCount <= 0)
        {
           inventory.items[index] = 0;
        }
       
    }

    public void Cross() {
        foreach (Transform child in transform) {
            child.GetComponent<Spawn>().SpawnItem();
            GameObject.Destroy(child.gameObject);
        }
    }
}
