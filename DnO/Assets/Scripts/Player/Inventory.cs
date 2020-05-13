using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int[] items;
    public GameObject[] slots;

    void Start()
    {
        for(int i = 0; i < items.Length; i++)
        {
            slots[i].GetComponent<Slot>().index = i;
        }
    }
}
