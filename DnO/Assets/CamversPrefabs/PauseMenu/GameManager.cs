using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEditor.Experimental.GraphView;
using TMPro;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    enum State
    {
        Ready,
        Play,
        GameOver
    }
    State state;

    public static bool isPause = false;
    public bool isStarted = false;

    public int dayCount = 1;
    public int playerHealth;
    public int killCount = 0;

    public Camera mCamera;
    public Transform player;
    public Transform gameOverUI;
    public Transform survivalText;
    public Transform killedText;
    public Transform whatDay;

    private Transform survivalJoystick;
    private Transform defenseJoystick;
    private Transform dLight;
    private Transform lantern;
    private GameObject[] friends;

    private SpawnManager sManager;
    private GraveSpawnManager gsManager;

    void Awake()
    {
        Ready();
        mCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("PLAYER").transform;
        survivalJoystick = GameObject.Find("SurvivalJoystick").transform;
        survivalJoystick.gameObject.SetActive(false);
        defenseJoystick = GameObject.Find("Joystick").transform;
        dLight = GameObject.Find("Directional Light").transform;
        sManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        gsManager = GameObject.Find("GraveSpawnManager").GetComponent<GraveSpawnManager>();
        gsManager.gameObject.SetActive(false);
        lantern = GameObject.FindGameObjectWithTag("Bip001 L Hand").transform;
        friends = GameObject.FindGameObjectsWithTag("FRIEND");
        lantern.GetChild(0).gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
    }

    [System.Obsolete]
    private void LateUpdate()
    {
        switch (state)
        {
            case State.Ready:
                if (Input.GetButtonDown("Fire1"))
                {
                    GameStart();
                }
                break;
            case State.Play:
                if (player.GetComponent<MoveRotation1>().isDie)
                {
                    GameOver();
                }
                break;
            case State.GameOver:
                
                break;
        }
    }

    void Ready()
    {
        state = State.Ready;
        whatDay.gameObject.SetActive(true);
        if(player.GetComponent<MoveRotation1>().isSurvival == false)
        {
            whatDay.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Day " + dayCount.ToString();
        }
        else if (player.GetComponent<MoveRotation1>().isSurvival == true)
        {
            whatDay.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Night " + dayCount.ToString();
        }
        isStarted = false;
    }

    void GameStart()
    {
        state = State.Play;
        isStarted = true;
        whatDay.gameObject.SetActive(false);
        if (player.GetComponent<MoveRotation1>().isSurvival == false)
        {
            StartCoroutine(sManager.SpawnEnemy());
            StartCoroutine(sManager.SpawnMeleeEnemy());
        }
        else
        {
            StartCoroutine(gsManager.GraveSpawn());
        }
    }

    void GameOver()
    {
        state = State.GameOver;
        isStarted = false;
        if(player.GetComponent<MoveRotation1>().isSurvival == true)
        {
            for (int i = 0; i < gsManager.enemyList.Count; i++)
            {
                gsManager.enemyList[i].GetComponent<NavMeshAgent>().isStopped = true;
            }
            GameObject.Find("SurvivalJoystick").SetActive(false);
        }
        else
        {
            for (int i = 0; i < sManager.enemyList.Count; i++)
            {
                sManager.enemyList[i].GetComponent<NavMeshAgent>().isStopped = true;
            }
            for (int i = 0; i < sManager.enemyMeleeList.Count; i++)
            {
                sManager.enemyMeleeList[i].GetComponent<NavMeshAgent>().isStopped = true;
            }
            GameObject.Find("Canvas").SetActive(false);
            GameObject.Find("Joystick").SetActive(false);
        }
        GameObject.Find("Inventory").SetActive(false);
        GameObject.Find("HHGP").SetActive(false);
        survivalText.gameObject.GetComponent<TextMeshProUGUI>().text = "You Survived '" + dayCount.ToString() + "' Days";
        killedText.gameObject.GetComponent<TextMeshProUGUI>().text = "You Killed '" + killCount.ToString() + "' Zombies";
        gameOverUI.gameObject.SetActive(true);
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MenuScene"));
        SceneManager.LoadScene("MenuScene");
    }

    public void GoToSurvival()
    {
        for (int i = 0; i < friends.Length; i++)
        {
            friends[i].SetActive(false);
        }
        sManager.gameObject.SetActive(false);
        gsManager.gameObject.SetActive(true);
        player.GetComponent<MoveRotation1>().isSurvival = true;
        gsManager.isSurvivalEnd = false;
        player.position = new Vector3(100.0f, 0.5f, 0.0f);
        mCamera.transform.position = new Vector3(100.0f, mCamera.transform.position.y, mCamera.transform.position.z);
        survivalJoystick.gameObject.SetActive(true);
        defenseJoystick.gameObject.SetActive(false);
        dLight.gameObject.SetActive(false);
        GameObject.Find("Build").GetComponent<Image>().enabled = false;
        GameObject.Find("Build").transform.GetChild(0).GetComponent<Image>().enabled = false;
        GameObject.Find("Build").transform.GetChild(1).GetComponent<Image>().enabled = false;
        lantern.GetChild(0).gameObject.SetActive(true);
        sManager.howManySpawn = Mathf.RoundToInt(sManager.howManySpawn * 1.1f);
        Ready();
    }

    public void GoToDefense()
    {
        player.GetComponent<MoveRotation1>().isAttack = false;
        gsManager.gameObject.SetActive(false);
        sManager.gameObject.SetActive(true);
        for (int i = 0; i < friends.Length; i++)
        {
            friends[i].SetActive(true);
        }
        sManager.spawnCount = 0;
        sManager.wsFloat = 10.0f;
        player.GetComponent<MoveRotation1>().isSurvival = false;
        sManager.isDefenseEnd = false;
        player.GetComponent<MoveRotation1>().crrentHp = 10;
        player.position = new Vector3(0.0f, 0.5f, 0.0f);
        mCamera.transform.position = new Vector3(0.0f, mCamera.transform.position.y, mCamera.transform.position.z);
        GameObject.Find("Canvas").SetActive(true);
        survivalJoystick.gameObject.SetActive(false);
        defenseJoystick.gameObject.SetActive(true);
        dLight.gameObject.SetActive(true);
        GameObject.Find("Build").GetComponent<Image>().enabled = true;
        GameObject.Find("Build").transform.GetChild(0).GetComponent<Image>().enabled = true;
        GameObject.Find("Build").transform.GetChild(1).GetComponent<Image>().enabled = true;
        lantern.GetChild(0).gameObject.SetActive(false);
        gsManager.spawnCount = Mathf.RoundToInt(gsManager.spawnCount * 1.1f);
        //player.GetChild(0).Rotate(Vector3.zero);
        player.GetChild(0).localRotation = Quaternion.identity;
        dayCount++;
        Ready();
    }
}
