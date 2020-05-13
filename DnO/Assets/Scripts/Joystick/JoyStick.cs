using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
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
			if(pos.x <=120f && pos.x >=-120f&&  pos.y >=-120f&&pos.y<=120f)
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
