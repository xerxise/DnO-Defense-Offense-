using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyRange;
    public GameObject enemyMelee;
    private Vector3[] spawnLocation;
    
    public List<Transform> enemyList;
    public List<Transform> enemyMeleeList;

    public int count = 0;
    public int meleeCount = 0;
    public int spawnCount = 0;
    public int howManySpawn = 2;

    public bool isDefenseEnd = false;

    [SerializeField]
    public float wsFloat = 10.0f;
    [SerializeField]
    private WaitForSeconds wSecond;
    public GameManager gameManager;
    

    private Transform curStage;
    void Start()
    {
        wSecond = new WaitForSeconds(wsFloat);
        enemyList = new List<Transform>();
        enemyMeleeList = new List<Transform>();
        curStage = GameObject.FindGameObjectWithTag("PLAYER").transform;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawnLocation = new Vector3[5];
        spawnLocation[0] = new Vector3(-10.0f, 0.5f, -40.0f);
        spawnLocation[1] = new Vector3(-20.0f, 0.5f, -40.0f);
        spawnLocation[2] = new Vector3(0.0f, 0.5f, -40.0f);
        spawnLocation[3] = new Vector3(10.0f, 0.5f, -40.0f);
        spawnLocation[4] = new Vector3(20.0f, 0.5f, -40.0f);
    }

    void Update()
    {
        if (spawnCount > 20) isDefenseEnd = true;
        if(isDefenseEnd == true && enemyList.Count == 0 && enemyMeleeList.Count == 0)
        {
            gameManager.GoToSurvival();
        }
    }

    public IEnumerator SpawnEnemy()
    {
        while (isDefenseEnd == false && spawnCount <= 20)
        {
            if (gameManager.isStarted == false) yield break;
            yield return wSecond;
            for(int i=0; i< Mathf.Round(howManySpawn/2); i++)
            {
                GameObject go = Instantiate(enemyRange, spawnLocation[Random.Range(0, 5)], Quaternion.identity);
                enemyList.Add(go.transform);
                go.GetComponent<EnemyRangeAI>().erNum = count;
                count++;
            }
            spawnCount++;
            if (spawnCount % 5 == 0)
            {
                wsFloat -= 1.5f;
                wSecond = new WaitForSeconds(wsFloat);
            }
            if (spawnCount % 10 == 0)
            {
                howManySpawn += 1;
            }
        }
    }

    public IEnumerator SpawnMeleeEnemy()
    {
        while (isDefenseEnd == false && spawnCount <= 20)
        {
            if (gameManager.isStarted == false) yield break;
            yield return wSecond;
            for (int i = 0; i < Mathf.Round(howManySpawn / 2); i++)
            {
                GameObject go = Instantiate(enemyMelee, spawnLocation[Random.Range(0, 5)], Quaternion.identity);
                enemyMeleeList.Add(go.transform);
                go.GetComponent<EnemyMeleeAI>().erNum = meleeCount;
                meleeCount++;
            }
        }
    }

    public void DestroyedEnemy(int index)
    {
        if (enemyList.Count == 0)
        {
            count = 0;
        }
        else if (enemyList.Count == 1)
        {
            enemyList[0].GetComponent<EnemyRangeAI>().erNum = 0;
            count = 1;
        }
        else
        {
            for (int i = index; i < enemyList.Count; i++)
            {
                enemyList[i].GetComponent<EnemyRangeAI>().erNum -= 1;
                count = enemyList.Count - 1;
            }
        }
    }

    public void DestroyedMeleeEnemy(int index)
    {
        if (enemyMeleeList.Count == 0)
        {
            meleeCount = 0;
        }
        else if (enemyMeleeList.Count == 1)
        {
            enemyMeleeList[0].GetComponent<EnemyMeleeAI>().erNum = 0;
            meleeCount = 1;
        }
        else
        {
            for (int i = index; i < enemyMeleeList.Count; i++)
            {
                enemyMeleeList[i].GetComponent<EnemyMeleeAI>().erNum -= 1;
                meleeCount = enemyMeleeList.Count - 1;
            }
        }
    }
}
