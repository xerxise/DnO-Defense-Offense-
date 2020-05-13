using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    private GameObject go;

    private Transform playerTr;
    private Transform enemyTr;
    private Transform attackHand;

    private MoveAgent agent;
    private UIManager uiManager;

    private float nextFire = 0.0f;
    private readonly float fireRate = 0.5f;
    private readonly float damping = 10.0f;
    public bool isFire = false;
    private Animator animator;

    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        enemyTr = GetComponent<Transform>();
        agent = GetComponent<MoveAgent>();
        animator =GetComponent<Animator>();
    }

    void Update()
    {
        if (isFire)
        {
            if (Time.time >= nextFire)
            {
                StartCoroutine(Fire());
                animator.SetTrigger("Attack");
                nextFire = Time.time + fireRate + Random.Range(1.0f, 1.5f);
            }

            Quaternion rot = Quaternion.LookRotation(agent.traceTarget - enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
    }

    IEnumerator Fire()
    {
        float time2 = 0.0f;
        while (true)
        {
            time2 += Time.deltaTime * 2.0f;
            if (Vector3.Distance(transform.position, playerTr.position) < 2.5f)
            {
                uiManager.PlayerDamaged(1);
                break;
            }
            if (time2 >= 5.0f)
            {
                break;
            }
            yield return null;
        }
    }
}
