using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour {

    public Transform playerPos;
    public GameObject sword;
    public Spawn spawn;
    public MoveRotation1 player;
    public int healthInt;
    public Transform[] healthAmount;
    
    private void Start()
    {
        
        healthAmount = new Transform[GameObject.Find("HHGP").transform.GetChild(1).childCount];
        playerPos = GameObject.FindGameObjectWithTag("R_hand_container").transform;
        player = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<MoveRotation1>();
        spawn = GameObject.FindObjectOfType<Spawn>();
        healthInt = player.GetComponent<MoveRotation1>().crrentHp;
    }

    public void Use() {

        switch (playerPos.childCount)
        {
            case 0:
            ItemInstantiate(); 
            StartCoroutine("FindItenName");
            break;
            case 1:
            spawn.DestroyItem();
            ItemInstantiate();    
             StartCoroutine("FindItenName");
            break;
            default:
            return; 
        }
    }
     void ItemInstantiate()
    {
        GameObject a = Instantiate(sword, playerPos.position, sword.transform.rotation, playerPos.transform);
        a.transform.localPosition = Vector3.zero;
        a.transform.localRotation = Quaternion.identity;
        a.GetComponent<BoxCollider>().enabled = false;
        
    }
    IEnumerator FindItenName()
    {
        yield return new WaitForSeconds(0.2f);
        if(playerPos.GetChild(0).gameObject.tag == "Knife")
        {
            player.itemName = "knife";
        }
        if(playerPos.GetChild(0).gameObject.tag == "Bat")
        {

            player.itemName = "bat";
        }
        if(playerPos.GetChild(0).gameObject.tag == "Handgun")
        {
           player.itemName = "handgun";
        }
    }

    public void RestoreHealth()
    {
        if (healthInt > 6)
        {
            int curHP = healthInt;
            healthInt = 10;
            player.GetComponent<MoveRotation1>().crrentHp = healthInt;
            for (int i = curHP; i < healthInt; i++)
            {
                healthAmount[i].gameObject.SetActive(true);
            }
        }
        else
        {
            int curHP = healthInt;
            healthInt += 4;
            player.GetComponent<MoveRotation1>().crrentHp = healthInt;
            for (int i = curHP; i < healthInt; i++)
            {
                healthAmount[i].gameObject.SetActive(true);
            }
        }
    }

}
