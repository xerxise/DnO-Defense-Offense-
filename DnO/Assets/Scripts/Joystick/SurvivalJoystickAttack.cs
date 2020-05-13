using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SurvivalJoystickAttack : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image outLine;
    public Image inLine;
    public Vector2 dir;
    public Vector3 rotDir;
    public Vector3 _axis;
    public bool survAttack = false;

    private float rotSpeed = 80.0f;
    [SerializeField]
    private MoveRotation1 player;

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

    public void OnPointerDown(PointerEventData eventData)
    {
        survAttack = true;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inLine.rectTransform.anchoredPosition = Vector3.zero;
        _axis = Vector3.zero;
        dir = Vector2.zero;
        survAttack = false;
    }

    public Vector3 SJoystickMove()
    {
        return dir;
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<MoveRotation1>();
    }

    void Update()
    {
        SJoyStickCtrl();
    }

    private void SJoyStickCtrl()
    {
        rotDir = new Vector3(SJoystickMove().normalized.x, 0, SJoystickMove().normalized.y);
        if (rotDir.x != 0 || rotDir.z != 0)
        {
            Rotation();
        }
        else if (rotDir.x == 0 && rotDir.z == 0)
        {

        }  
        else return;

    }
    
    //Rotation Method
    private void Rotation()
    {
        float y = Mathf.Atan2(rotDir.x, rotDir.z) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, y, 0);
        player.gameObject.transform.GetChild(0).rotation =
        Quaternion.Slerp(player.gameObject.transform.GetChild(0).rotation, rot, Time.deltaTime * rotSpeed);


       // player.gameObject.transform.rotation =
       //Quaternion.Slerp(player.gameObject.transform.rotation, rot, Time.deltaTime * rotSpeed);
    }
}
