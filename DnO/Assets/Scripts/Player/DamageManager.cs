using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    private bool isAttack;
    private bool isSurvival;
    private List<Transform> enList;
    private List<Transform> enMeleeList;
    private MoveRotation1 player;
    private Transform target;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<MoveRotation1>();
    }

    void Update()
    {
        isSurvival = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<MoveRotation1>().isSurvival;
        if (gameObject.CompareTag("PlayerMeleeWeapon") )
        {
            isAttack = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<MoveRotation1>().isAttack;
            if (isAttack == true && isSurvival == false)
            {
                enList = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().enemyList;
                enMeleeList = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().enemyMeleeList;
                for (int i = 0; i < enList.Count; i++)
                {
                    if (Vector3.Distance(transform.position, enList[i].position) < 2.0f)
                    {
                        target = enList[i];
                        target.GetComponent<MoveAgent>().Damage(1);
                        GameObject.FindGameObjectWithTag("PLAYER").GetComponent<MoveRotation1>().isAttack = false;
                        break;
                    }
                }
                for (int i = 0; i < enMeleeList.Count; i++)
                {
                    if (Vector3.Distance(transform.position, enMeleeList[i].position) < 2.0f)
                    {
                        target = enMeleeList[i];
                        target.GetComponent<MoveAgent>().Damage(1);
                        GameObject.FindGameObjectWithTag("PLAYER").GetComponent<MoveRotation1>().isAttack = false;
                        break;
                    }
                }

            }
            else if (isAttack == true && isSurvival == true)
            {
                enList = GameObject.Find("GraveSpawnManager").GetComponent<GraveSpawnManager>().enemyList;
                for (int i = 0; i < enList.Count; i++)
                {
                    if (Vector3.Distance(transform.position, enList[i].position) < 2.0f)
                    {
                        target = enList[i];
                        target.GetComponent<MoveAgent>().Damage(1);
                        GameObject.FindGameObjectWithTag("PLAYER").GetComponent<MoveRotation1>().isAttack = false;
                        break;
                    }
                }
            }
        }
        else if (gameObject.CompareTag("PlayerBullet"))
        {
            if (isSurvival == false)
            {
                enList = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().enemyList;
                enMeleeList = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().enemyMeleeList;
                for (int i = 0; i < enList.Count; i++)
                {
                    if (Vector3.Distance(transform.position, enList[i].position) < 2.5f)
                    {
                        target = enList[i];
                        target.GetComponent<MoveAgent>().Damage(1);
                        Destroy(gameObject);
                    }
                }
                for (int i = 0; i < enMeleeList.Count; i++)
                {
                    if (Vector3.Distance(transform.position, enMeleeList[i].position) < 2.5f)
                    {
                        target = enMeleeList[i];
                        target.GetComponent<MoveAgent>().Damage(1);
                        Destroy(gameObject);
                    }
                }
            }
            else if (isSurvival == true)
            {
                enList = GameObject.Find("GraveSpawnManager").GetComponent<GraveSpawnManager>().enemyList;
                for (int i = 0; i < enList.Count; i++)
                {
                    if (Vector3.Distance(transform.position, enList[i].position) < 2.5f)
                    {
                        target = enList[i];
                        target.GetComponent<MoveAgent>().Damage(1);
                        Destroy(gameObject);
                    }
                }
            }
        }
        else if (gameObject.CompareTag("FriendWeapon"))
        {
            if (isSurvival == false)
            {
                enList = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().enemyList;
                enMeleeList = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().enemyMeleeList;
                for (int i = 0; i < enList.Count; i++)
                {
                    if (Vector3.Distance(transform.position, enList[i].position) < 3.0f)
                    {
                        target = enList[i];
                        target.GetComponent<MoveAgent>().Damage(1);
                        Destroy(gameObject);
                        break;
                    }
                }
                for (int i = 0; i < enMeleeList.Count; i++)
                {
                    if (Vector3.Distance(transform.position, enMeleeList[i].position) < 3.0f)
                    {
                        target = enMeleeList[i];
                        target.GetComponent<MoveAgent>().Damage(1);
                        Destroy(gameObject);
                        break;
                    }
                }
            }
            else if (isSurvival == true)
            {
                enList = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().enemyList;
                enMeleeList = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().enemyMeleeList;
                for (int i = 0; i < enList.Count; i++)
                {
                    if (Vector3.Distance(transform.position, enList[i].position) < 3.0f)
                    {
                        target = enList[i];
                        target.GetComponent<MoveAgent>().Damage(1);
                        Destroy(gameObject);
                        break;
                    }
                }
                for (int i = 0; i < enMeleeList.Count; i++)
                {
                    if (Vector3.Distance(transform.position, enMeleeList[i].position) < 3.0f)
                    {
                        target = enMeleeList[i];
                        target.GetComponent<MoveAgent>().Damage(1);
                        Destroy(gameObject);
                        break;
                    }
                }
            }
        }
    }
}
