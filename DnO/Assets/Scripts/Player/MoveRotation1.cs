using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRotation1 : MonoBehaviour
{
    public enum State{Idle,Walk,Attack,Run,Damage,Die}
    public enum ActionState{Idle,Walk,Attack,Heal,Run,Damage,Die}
    public State state;
    public ActionState actionState;

    private float h,v = 0;
    private float moveSpeed =0f;
    private float rotSpeed = 80f;
    private float NextTime = 0.5f;
    

    private Vector3 axis;
    private Vector3 dir;

    private Transform tr;
    [SerializeField]
    private Transform currentWeapon;
    private Animator animator;

    public bool isAttack=false;
    public bool isRun=false;
    public bool isDie = false;

    private WaitForSeconds wts;

    private float Distance;
    private float crrentDis;
    private float attackPos =1.2f;
    private float nextFire = 0.0f;

    public  GameObject targetPos;
    public  GameObject hitPos;
    public GameObject playerBullet;

    public JoyStick joyStick;

    public int initHp = 10;
    public int crrentHp = 10;

    public bool isSurvival = false;
    public string itemName;

    void Awake()
    {
        tr = transform.GetComponent<Transform>();
        currentWeapon = GameObject.FindGameObjectWithTag("R_hand_container").transform;
        hitPos = GameObject.Find("HitPostion");
        animator = transform.GetChild(0).GetComponent<Animator>();
        state = State.Idle; 
        actionState = ActionState.Idle;
        crrentHp = initHp;//Crrent Hp in it Hp 
    }
    
    void Start()
    {
        wts = new WaitForSeconds(1.0f);
        StartCoroutine(WaitButton());
    }

    private IEnumerator WaitButton()
    {
        while (isAttack == true)
        {
            isAttack = true;
            yield return wts;
            isAttack = false;
        }
    }

    //switch 한번 인식후 유지 계속 입력값이 들어가지 않음
    void MyStateSwitch()
    {
        switch(state)
        {
            case State.Idle:
                animator.SetInteger("Condition",0);
                break;
            case State.Walk:
                SoundManager.soundManager.PlayerAction("Walk");
                animator.SetInteger("Condition",1);
                state = State.Idle;
                break;
            case State.Attack:
                animator.SetTrigger("Attack");
                state =State.Idle;
                break;
            case State.Run:
                SoundManager.soundManager.PlayerAction("Run");
                animator.SetInteger("Condition",2);
                state = State.Idle;      
                break;
            case State.Damage:
                SoundManager.soundManager.PlayerAction("Hit");
                animator.SetTrigger("Hit");
                state = State.Idle;
                break;
            case State.Die:
                SoundManager.soundManager.PlayerAction("Die");
                animator.SetInteger("Condition",4);
                break;   
        }
    }

    void ActionStateSwitch()
    {
        switch (actionState)
        {
            case ActionState.Idle:
                break;
            case ActionState.Walk:
                break;
            case ActionState.Attack:
                break;
            case ActionState.Heal:
                break;
            case ActionState.Run:
                break;
            case ActionState.Damage:
                break;
            case ActionState.Die:
                break;
        }
    }

    void Update()
    {
        //if (isDie) return;
        JoyStickCtrl();
        ACtrl();
        MyStateSwitch();
        ActionStateSwitch();
    }
   
    //조이스틱 조작 
    private void JoyStickCtrl()
    {
        dir = new Vector3(joyStick.JoystickMove().normalized.x,0,joyStick.JoystickMove().normalized.y);
        if(isRun==false)
        {
            if(isAttack==false)
            {
                if(dir.x != 0 || dir.z != 0)
                {
                actionState = ActionState.Walk;
                state = State.Walk;
                WalkSelect(3f);     
                }
                else if(dir.x == 0 &&dir.z == 0)
                {
                state = State.Idle;
                actionState = ActionState.Idle;
                }
            }
            else return;
        }else Run();  
    }
    //Move Method
    private void WalkSelect(float moveSpeed)
    {
        tr.Translate(tr.transform.forward*Time.deltaTime * moveSpeed ,Space.World);
        Rotation();
    }
    void DIR(float moveSpeed)
    {
        if (dir.x != 0 || dir.z != 0)
        {
            tr.Translate(tr.transform.forward * Time.deltaTime * moveSpeed, Space.World);
            Rotation();
        }
        else return;
    }
    //UI ButtonCtrl
     public void BCtrlDown(string type)
    {
        switch(type)
        {
            case "A":
            isAttack=true;
            break;
            case "B":
            isRun = true;
            break;
        }
    }
    //UI ButtonCtrl
    public void BCtrlUp(string type)
    {
        switch(type)
        {
            case "A":
            isAttack =false;
            break;
            case "B":
            isRun = false;
            break;    
        }
    }
    //Run Swich 
    private void Run()
    {
        if (isRun == true)
        {
            DIR(6f);
            state = State.Run;
        }
        else return;
    }
    //Rotation Method
    private void Rotation()
    {
        float y = Mathf.Atan2(dir.x,dir.z)* Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0,y,0);
        tr.rotation =
        Quaternion.Slerp(tr.rotation,rot,Time.deltaTime* rotSpeed);
    }
    //Attack Switch
    public void ACtrl()
    {
        if (isAttack == true)
        {
            if (currentWeapon.childCount == 0)
            {
                SoundManager.soundManager.PlayerAction("Punch");
                animator.SetTrigger("Punch");
            }
            else if (currentWeapon.GetChild(0).CompareTag("PlayerMeleeWeapon"))
            {
                if(currentWeapon.GetChild(0).name.Equals("knife(Clone)"))
                {
                    animator.SetTrigger("Knife");
                    SoundManager.soundManager.PlayerAction("Knife");
                }
                else if(currentWeapon.GetChild(0).name.Equals("baseballbat(Clone)"))
                {
                    animator.SetTrigger("Bat");
                    SoundManager.soundManager.PlayerAction("Bat");
                }
                state = State.Attack;
            }
            else if (currentWeapon.GetChild(0).CompareTag("PlayerRangeWeapon"))
            {
                if (currentWeapon.GetChild(0).name.Equals("shotgun(Clone)"))
                {
                    if (Time.time >= nextFire)
                    {
                        animator.SetTrigger("Gun");
                        SoundManager.soundManager.PlayerAction("Bayonet");
                        StartCoroutine(ShotgunFire());
                        StartCoroutine(ShotgunFireLeft());
                        StartCoroutine(ShotgunFireRight());
                        nextFire = Time.time + 0.8f;
                    }
                }
                else if (currentWeapon.GetChild(0).name.Equals("rifle(Clone)"))
                {
                    if (Time.time >= nextFire)
                    {
                        animator.SetTrigger("Gun");
                        SoundManager.soundManager.PlayerAction("Rifle");
                        StartCoroutine(RifleFire());
                        nextFire = Time.time + 0.4f;
                    }
                }
                else if (currentWeapon.GetChild(0).name.Equals("handgun(Clone)"))
                {
                    if (Time.time >= nextFire)
                    {
                        animator.SetTrigger("Gun");
                        SoundManager.soundManager.PlayerAction("Handgun");
                        StartCoroutine(PlayerFire());
                        nextFire = Time.time + 0.5f;
                    }
                }
            }
            //isAttack = false;
        }
        else return;
    }

    IEnumerator RifleFire()
    {
        float time = 0.0f;
        List<Transform> list = new List<Transform>();
        if (isSurvival == true) list = GameObject.Find("GraveSpawnManager").GetComponent<GraveSpawnManager>().enemyList;
        else if (isSurvival == false) list = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().enemyList;
        GameObject go = Instantiate(playerBullet, currentWeapon.position, transform.GetChild(0).rotation);
        while (go != null)
        {
            time += Time.deltaTime * 2.0f;
            go.transform.Translate(Vector3.forward * Time.deltaTime * 7.0f);
            if (time >= 8.0f)
            {
                Destroy(go);
                break;
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (Vector3.Distance(go.transform.position, list[i].position) < 1.5f)
                {
                    Transform target = list[i];
                    target.GetComponent<MoveAgent>().Damage(1);
                    Destroy(go);
                    break;
                }
            }
            yield return null;
        }
    }

    IEnumerator ShotgunFire()
    {
        float time = 0.0f;
        List<Transform> list = new List<Transform>();
        if (isSurvival == true) list = GameObject.Find("GraveSpawnManager").GetComponent<GraveSpawnManager>().enemyList;
        else if (isSurvival == false) list = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().enemyList;
        GameObject go = Instantiate(playerBullet, currentWeapon.GetChild(0).position, transform.GetChild(0).rotation);
        while (go != null)
        {
            time += Time.deltaTime * 2.0f;
            go.transform.Translate(Vector3.forward * Time.deltaTime * 7.0f);
            if (time >= 8.0f)
            {
                Destroy(go);
                break;
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (Vector3.Distance(go.transform.position, list[i].position) < 1.5f)
                {
                    Transform target = list[i];
                    target.GetComponent<MoveAgent>().Damage(1);
                    Destroy(go);
                    break;
                }
            }
            yield return null;
        }
    }
    IEnumerator ShotgunFireLeft()
    {
        float time = 0.0f;
        List<Transform> list = new List<Transform>();
        if (isSurvival == true) list = GameObject.Find("GraveSpawnManager").GetComponent<GraveSpawnManager>().enemyList;
        else if (isSurvival == false) list = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().enemyList;
        GameObject go1 = Instantiate(playerBullet, currentWeapon.GetChild(0).position, Quaternion.Euler(0, transform.localEulerAngles.y + 20f, 0));
        while (go1 != null)
        {
            time += Time.deltaTime * 2.0f;
            go1.transform.Translate(Vector3.forward * Time.deltaTime * 7.0f);
            if (time >= 8.0f)
            {
                Destroy(go1);
                break;
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (Vector3.Distance(go1.transform.position, list[i].position) < 1.5f)
                {
                    Transform target = list[i];
                    target.GetComponent<MoveAgent>().Damage(1);
                    Destroy(go1);
                    break;
                }
            }
            yield return null;
        }
    }
    IEnumerator ShotgunFireRight()
    {
        float time = 0.0f;
        List<Transform> list = new List<Transform>();
        if (isSurvival == true) list = GameObject.Find("GraveSpawnManager").GetComponent<GraveSpawnManager>().enemyList;
        else if (isSurvival == false) list = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().enemyList;
        GameObject go2 = Instantiate(playerBullet, currentWeapon.GetChild(0).position, Quaternion.Euler(0, transform.localEulerAngles.y + 340f, 0));
        while (go2 != null)
        {
            time += Time.deltaTime * 2.0f;
            go2.transform.Translate(Vector3.forward * Time.deltaTime * 7.0f);
            if (time >= 6.0f)
            {
                Destroy(go2);
                break;
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (Vector3.Distance(go2.transform.position, list[i].position) < 1.5f)
                {
                    Transform target = list[i];
                    target.GetComponent<MoveAgent>().Damage(1);
                    Destroy(go2);
                    break;
                }
            }
            yield return null;
        }
    }

    IEnumerator PlayerFire()
    {
        float time = 0.0f;
        List<Transform> list = new List<Transform>();
        if (isSurvival == true) list = GameObject.Find("GraveSpawnManager").GetComponent<GraveSpawnManager>().enemyList;
        else if(isSurvival == false) list = GameObject.Find("SpawnManager").GetComponent<SpawnManager>().enemyList;
        GameObject go = Instantiate(playerBullet, currentWeapon.position, transform.GetChild(0).rotation);
        while (go != null)
        {
            time += Time.deltaTime * 2.0f;
            go.transform.Translate(Vector3.forward * Time.deltaTime * 7.0f);
            if(time >= 8.0f)
            {
                Destroy(go);
                break;
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (Vector3.Distance(go.transform.position, list[i].position) < 1.5f)
                {
                    Transform target = list[i];
                    target.GetComponent<MoveAgent>().Damage(1);
                    Destroy(go);
                    break;
                }
            }
            yield return null;
        }
    }
}
