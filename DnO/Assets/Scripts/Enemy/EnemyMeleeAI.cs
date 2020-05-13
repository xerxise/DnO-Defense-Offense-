using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAI : MonoBehaviour
{
    public enum STATE
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    public STATE state = STATE.IDLE;

    private Transform playerTr;
    private Transform bListTr;
    private List<Transform> barricadeList;
    private Transform enemyTr;
    Transform compare;

    public float attackDist = 2.0f;
    public bool isDie = false;

    private WaitForSeconds wSecond;
    private MoveAgent moveAgent;
    private EnemyMeleeAttack emAttack;
    private BuildManager bManager;
    private SpawnManager sManager;
    private GraveSpawnManager gsManager;
    private UIManager uiManager;
    private GameManager gManager;

    public int enemyHealth = 10;
    public int erNum;

    private Animator animator;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        if (player != null) playerTr = player.GetComponent<Transform>();
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemyTr = GetComponent<Transform>();
        moveAgent = GetComponent<MoveAgent>();
        emAttack = GetComponent<EnemyMeleeAttack>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        bManager = GameObject.FindGameObjectWithTag("BUILD").GetComponent<BuildManager>();
        if (GameObject.Find("SpawnManager") != null) sManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (GameObject.Find("GraveSpawnManager") != null) gsManager = GameObject.Find("GraveSpawnManager").GetComponent<GraveSpawnManager>();
        animator = GetComponent<Animator>();
        wSecond = new WaitForSeconds(0.2f);
    }

    void Update()
    {
        if (enemyHealth <= 0)
        {
            if (playerTr.GetComponent<MoveRotation1>().isSurvival == false)
            {
                uiManager.GetGold(50);
                sManager.enemyMeleeList.RemoveAt(erNum);
                sManager.DestroyedMeleeEnemy(erNum);
                gManager.killCount++;
                moveAgent.ItemDrop();
            }
            else if(playerTr.GetComponent<MoveRotation1>().isSurvival == true)
            {
                gsManager.enemyList.RemoveAt(erNum);
                gsManager.SurvivalDestroyedEnemy(erNum);
                gManager.killCount++;
                moveAgent.ItemDrop();
            }
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        while (!isDie)
        {
            if (state == STATE.DIE) yield break;
            barricadeList = bManager.obstacleList;
            float distance = Vector3.Distance(playerTr.position, enemyTr.position);
            float bcListDistance = 0.0f;
            if (barricadeList.Count > 0)
            {
                if (barricadeList.Count > 1)
                {
                    for (int i = 0; i < barricadeList.Count; i++)
                    {
                        if (i == 0) compare = barricadeList[i];
                        else
                        {
                            if (Vector3.Distance(compare.position, enemyTr.position) > Vector3.Distance(barricadeList[i].position, enemyTr.position))
                            {
                                compare = barricadeList[i];
                            }
                        }
                        bListTr = compare;
                    }
                }
                else if (barricadeList.Count == 1) bListTr = barricadeList[0];
                bcListDistance = Vector3.Distance(enemyTr.position, bListTr.position);
            }
            if (distance <= attackDist)
            {
                state = STATE.ATTACK;
            }
            else if (bListTr != null && bcListDistance <= attackDist)
            {
                state = STATE.ATTACK;
            }
            else if (playerTr.gameObject != null || bListTr.gameObject != null)
            {
                state = STATE.TRACE;
            }
            else
            {
                state = STATE.IDLE;
            }
            yield return wSecond;
        }
    }

    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return wSecond;
            barricadeList = bManager.obstacleList;
            float distance = Vector3.Distance(playerTr.position, enemyTr.position);
            float bcListDistance = 0.0f;
            if (barricadeList.Count > 0)
            {
                if (barricadeList.Count > 1)
                {
                    for (int i = 0; i < barricadeList.Count; i++)
                    {
                        if (i == 0) compare = barricadeList[i];
                        else
                        {
                            if (Vector3.Distance(compare.position, enemyTr.position) > Vector3.Distance(barricadeList[i].position, enemyTr.position))
                            {
                                compare = barricadeList[i];
                            }
                        }
                        bListTr = compare;
                    }
                }
                else if (barricadeList.Count == 1) bListTr = barricadeList[0];
                bcListDistance = Vector3.Distance(enemyTr.position, bListTr.position);
            }
            switch (state)
            {
                case STATE.IDLE:
                    emAttack.isFire = false;
                    animator.SetInteger("Condition",0);
                    moveAgent.ideling = true;
                    break;
                case STATE.TRACE:
                    emAttack.isFire = false;
                    if (distance < bcListDistance || bListTr == null) 
                    {
                        animator.SetInteger("Condition",1);
                        moveAgent.traceTarget = playerTr.position;
                    }
                    else if (bcListDistance < distance) moveAgent.traceTarget = bListTr.position;
                    break;
                case STATE.ATTACK:
                    moveAgent.Stop();
                    if (emAttack.isFire == false) emAttack.isFire = true;
                    if (bListTr == null) moveAgent.traceTarget = playerTr.position;
                    if (bListTr != null && moveAgent.traceTarget != bListTr.position && moveAgent.traceTarget != playerTr.position) moveAgent.traceTarget = playerTr.position;
                    if (bListTr != null && moveAgent.traceTarget == bListTr.position && distance <= attackDist) moveAgent.traceTarget = playerTr.position;
                    if (bListTr != null && moveAgent.traceTarget == playerTr.position && bcListDistance < distance && distance > attackDist) moveAgent.traceTarget = bListTr.position;
                    if (bListTr != null && bListTr.GetComponent<RepairManager>().wallHealth <= 0) state = STATE.TRACE;
                    break;
                case STATE.DIE:
                    animator.SetInteger("Condition",4);
                    moveAgent.Stop();
                    break;
            }
        }
    }
}
