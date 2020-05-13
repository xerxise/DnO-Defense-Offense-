using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Transform player;

    public Transform[] healthAmount;
    public int healthInt;
    public Text goldText;

    
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PLAYER").transform;
        healthInt = player.GetComponent<MoveRotation1>().crrentHp;
        healthAmount = new Transform[GameObject.Find("HHGP").transform.GetChild(1).childCount];
        goldText = GameObject.Find("HHGP").transform.GetChild(2).GetChild(2).GetComponent<Text>();
        
        for(int i=0; i<healthAmount.Length; i++)
        {
            healthAmount[i] = GameObject.Find("HHGP").transform.GetChild(1).GetChild(i);
        }
    }

    void Update()
    {

    }


    public void PlayerDamaged(int damage)
    {
        if (healthInt - damage < 0) {
            for (int i = 0; i < healthInt; i++)
            {
                healthAmount[i].gameObject.SetActive(false);
            }
            healthInt = 0;
            player.GetComponent<MoveRotation1>().crrentHp = healthInt;
            player.GetComponent<MoveRotation1>().isDie = true;
            player.GetComponent<MoveRotation1>().state = MoveRotation1.State.Die;

        }
        else if(healthInt - damage == 0)
        {
            healthAmount[0].gameObject.SetActive(false);
            healthInt = 0;
            player.GetComponent<MoveRotation1>().crrentHp = healthInt;
            player.GetComponent<MoveRotation1>().isDie = true;
            player.GetComponent<MoveRotation1>().state = MoveRotation1.State.Die;
        }
        else
        {
            for (int i = healthInt - 1; i > healthInt - 1 - damage; i--)
            {
                healthAmount[i].gameObject.SetActive(false);
            }
            healthInt -= damage;
            player.GetComponent<MoveRotation1>().crrentHp = healthInt;
        }
    }

    public void GetGold(int amount)
    {
        int totalGold = int.Parse(goldText.text);
        totalGold += amount;
        goldText.text = totalGold.ToString();
    }

    public void UseGold(int amount)
    {
        if (int.Parse(goldText.text) < amount) return;
        int totalGold = int.Parse(goldText.text);
        totalGold -= amount;
        goldText.text = totalGold.ToString();
    }

    public void RestoreHealth()
    {
        if(healthInt > 6)
        {
            int curHP = healthInt;
            healthInt = 10;
            player.GetComponent<MoveRotation1>().crrentHp = healthInt;
            for(int i=curHP; i<healthInt; i++)
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
