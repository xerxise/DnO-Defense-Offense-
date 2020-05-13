using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SurvivalJoystickMove : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image outLine;
    public Image inLine;
    public Vector2 dir;
    public Vector3 rotDir;
    public Vector3 _axis;

    private float rotSpeed = 80.0f;

    public MoveRotation1 player;
    public Animator animator;
    [SerializeField]
    private bool isSurvAttack;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle
        (outLine.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            dir = pos;
            if (pos.x <= 120f && pos.y >= -120f && pos.x >= -120f && pos.y <= 120f)
            {
                inLine.rectTransform.anchoredPosition = pos;
            }
            else
            {
                inLine.rectTransform.anchoredPosition = pos.normalized * 120f;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inLine.rectTransform.anchoredPosition = Vector3.zero;
        _axis = Vector2.zero;
        dir = Vector2.zero;
    }

    public Vector3 SJoystickMove()
    {
        return dir;
    }

    void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("PLAYER").transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        isSurvAttack = GameObject.Find("SurvivalJoystick").transform.GetChild(1).GetComponent<SurvivalJoystickAttack>().survAttack;
        SJoyStickCtrl();
        MyStateSwitch();
    }

    private void SJoyStickCtrl()
    {
        rotDir = new Vector3(SJoystickMove().normalized.x, 0, SJoystickMove().normalized.y);
        if (player.isRun == false)
        {
            if (rotDir.x != 0 || rotDir.z != 0)
            {
                player.state = MoveRotation1.State.Walk;
                WalkSelect2(rotDir);
            }
            else if (rotDir.x == 0 && rotDir.z == 0)
            {
                player.state = MoveRotation1.State.Idle;
            }
            else return;
        }
    }
    //Move Method

    private void WalkSelect2(Vector3 moveSpeed)
    {
        player.gameObject.transform.Translate(moveSpeed * Time.deltaTime * 4.5f, Space.World);
        //player.gameObject.transform.Translate(rotDir * Time.deltaTime * moveSpeed, Space.World);
        if (isSurvAttack == false) Rotation();
    }
    private void WalkSelect(float moveSpeed)
    {
        player.gameObject.transform.Translate(player.gameObject.transform.forward * Time.deltaTime * moveSpeed, Space.World);
        //player.gameObject.transform.Translate(rotDir * Time.deltaTime * moveSpeed, Space.World);
        if (isSurvAttack == false) Rotation();
    }
    
    //Rotation Method
    private void Rotation()
    {
        float y = Mathf.Atan2(rotDir.x, rotDir.z) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, y, 0);
        player.transform.GetChild(0).localRotation = Quaternion.Euler(0,0,0);
        player.transform.rotation =
        Quaternion.Slerp(player.transform.rotation, rot, Time.deltaTime * rotSpeed);
    }

    void MyStateSwitch()
    {
        switch (player.state)
        {
            case MoveRotation1.State.Idle:
                animator.SetInteger("Condition", 0);
                break;
            case MoveRotation1.State.Walk:
                animator.SetInteger("Condition", 1);
                player.state = MoveRotation1.State.Idle;
                break;
            case MoveRotation1.State.Attack:
                Debug.Log("MyAtk");
                animator.SetTrigger("Attack");
                player.state = MoveRotation1.State.Idle;
                break;
            case MoveRotation1.State.Run:
                animator.SetInteger("Condition", 2);
                player.state = MoveRotation1.State.Idle;
                break;
            case MoveRotation1.State.Damage:
                animator.SetTrigger("Damage");
                player.state = MoveRotation1.State.Idle;
                break;
            case MoveRotation1.State.Die:
                animator.SetInteger("Condition", 4);
                break;
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
