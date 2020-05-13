using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    private UIManager uiManager;
    private Transform player;

    void Start()
    {
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        player = GameObject.FindGameObjectWithTag("PLAYER").transform;
        
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < 1.0f)
        {
            uiManager.PlayerDamaged(1);
        }
    }
}
