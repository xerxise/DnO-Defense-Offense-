using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public Transform player;
    public GameObject item;

    WeaponItem weaponItme;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("R_hand_container").transform;
        weaponItme = GetComponent<WeaponItem>();
    }

    public void SpawnItem() 
    {
        Item_Angles();
        DestroyItem();
    }

    Vector3 Angles(float angles)
    {
        angles += player.transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angles *Mathf.Deg2Rad),0,Mathf.Cos(angles*Mathf.Deg2Rad));
    }
    public List<Vector3> pos_list;
    void Item_Angles()
    {
        pos_list.Clear();
        for (int i = -2; i <= 2; i++)
        {
            Vector3 pos = Angles(i*18f);
            pos = pos*1.5f;
            pos_list.Add(pos);
        }
        int num = Random.Range(0,pos_list.Count);
        GameObject itempos = Instantiate(item,pos_list[num]+ player.position,Quaternion.identity);
        Debug.Log(player.transform.position);
        Debug.Log(pos_list[num]);
    }
    
    public void DestroyItem()
    {
        if(weaponItme.playerPos.childCount <= 0) return;
        else Destroy(weaponItme.playerPos.GetChild(0).gameObject);
    }

}
