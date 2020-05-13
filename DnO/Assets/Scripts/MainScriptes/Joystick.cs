using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    public Image Outline;
	public Image Inline;
	public Vector2 dir;
	Vector3 _axis;
    void Start()
    {
		
    }

	public void OnDrag(PointerEventData eventData)
	{
		Vector2 pos;
		if(RectTransformUtility.ScreenPointToLocalPointInRectangle
		(Outline.rectTransform,eventData.position,eventData.pressEventCamera,out pos))
		{
			dir =pos;
			if(pos.x <=120f && pos.y >=-120f&&pos.x >=-120f&&pos.y<=120f)
			{
				Inline.rectTransform.anchoredPosition= pos;
			}
			else
			{
				Inline.rectTransform.anchoredPosition = pos.normalized *120f;
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
    {
		
    }

    public void OnPointerUp(PointerEventData eventData)
    {
		Inline.rectTransform.anchoredPosition = Vector3.zero;
		_axis =Vector2.zero;
		dir = Vector2.zero;
	}

	public Vector3 JoystickMove()
	{
		return dir;
	}
	

	 public void Update()
    {

	}
    
	public void Rotation(Transform tr, float speed, Vector3 _axis = new Vector3())
	{
		if(_axis.x != 0 ||_axis.y !=0)
		{
			float y = Mathf.Atan2(_axis.x,_axis.z) *Mathf.Rad2Deg;
			Quaternion rot = Quaternion.Euler(0,y,0); 
			tr.rotation =Quaternion.Slerp(tr.rotation, rot, Time.deltaTime *speed);
		}
		else if(_axis.x ==0 ||_axis.z ==0)
		{
			Vector3 dir = _axis - tr.parent.transform.position;
			if(dir.magnitude <0.1f)
			{
				return;
			}
			Quaternion rot = Quaternion.LookRotation(dir);
			tr.rotation = Quaternion.Slerp(tr.rotation,rot,Time.deltaTime *speed *3f);
		}
	}

}
