using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendManager : MonoBehaviour
{
    private GameObject player;
    public bool isHired = false;

    public enum STATE
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }
    public STATE fState = STATE.IDLE;

    private WaitForSeconds wSecond;
    private MoveAgent agent;
    private FriendAttack frAttack;
    private UIManager uiManager;

    public bool isFriendDie = false;
    public float attackDist = 15.0f;
    public Transform playerTr;
    public Transform targetTr;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PLAYER");
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
        agent = GetComponent<MoveAgent>();
        frAttack = GetComponent<FriendAttack>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        wSecond = new WaitForSeconds(0.5f);
    }

    void Update()
    {
        if(isHired == false)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 4.0f)
            {
                transform.GetChild(1).gameObject.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100.0f))
                    {
                        if (hit.collider.gameObject.name.Equals("handshake"))
                        {
                            uiManager.UseGold(50);
                            transform.GetChild(1).gameObject.SetActive(false);
                            isHired = true;
                        } 
                    }
                }
            }
            else transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        while (!isFriendDie)
        {
            if (fState == STATE.DIE) yield break;
            float distance = Vector3.Distance(transform.position, playerTr.position);
            float enemyDistance = 0;
            List<Transform> eList = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().enemyList;
            List<Transform> emList = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().enemyMeleeList;
            if (eList.Count > 1)
            {
                for (int i = 0; i < eList.Count; i++)
                {
                    if (i == 0) targetTr = eList[i];
                    else
                    {
                        if (Vector3.Distance(targetTr.position, transform.position) > Vector3.Distance(eList[i].position, transform.position))
                        {
                            targetTr = eList[i];
                        }
                    }
                }
                enemyDistance = Vector3.Distance(targetTr.position, transform.position);
            }
            else if (eList.Count == 1) 
            {
                targetTr = eList[0];
                enemyDistance = Vector3.Distance(targetTr.position, transform.position);
            } 
            
            if (emList.Count > 1)
            {
                for (int i = 0; i < emList.Count; i++)
                {
                    if (i == 0) targetTr = emList[i];
                    else
                    {
                        if (Vector3.Distance(targetTr.position, transform.position) > Vector3.Distance(emList[i].position, transform.position))
                        {
                            targetTr = emList[i];
                        }
                    }
                }
                enemyDistance = Vector3.Distance(targetTr.position, transform.position);
            }
            else if (emList.Count == 1)
            {
                targetTr = emList[0];
                enemyDistance = Vector3.Distance(targetTr.position, transform.position);
            }
            if (targetTr != null && enemyDistance <= attackDist && isHired == true)
            {
                fState = STATE.ATTACK;
            }
            else
            {
                fState = STATE.IDLE;
            }
            yield return wSecond;
        }
    }

    IEnumerator Action()
    {
        while (!isFriendDie)
        {
            yield return wSecond;
            if (fState == STATE.DIE) yield break;
            float distance = Vector3.Distance(transform.position, playerTr.position);
            
            switch (fState)
            {
                case STATE.IDLE:
                    frAttack.isFire = false;
                    break;
                case STATE.TRACE:
                    
                    break;
                case STATE.ATTACK:
                    if (frAttack.isFire == false) frAttack.isFire = true;
                    agent.traceTarget = targetTr.position;
                    if(targetTr.GetComponent<EnemyRangeAI>() != null)
                    {
                        if (targetTr.GetComponent<EnemyRangeAI>().enemyHealth <= 0) fState = STATE.IDLE;
                    }
                    else if (targetTr.GetComponent<EnemyMeleeAI>() != null)
                    {
                        if (targetTr.GetComponent<EnemyMeleeAI>().enemyHealth <= 0) fState = STATE.IDLE;
                    }
                    break;
                case STATE.DIE:
                    
                    break;
            }
        }
    }
}
