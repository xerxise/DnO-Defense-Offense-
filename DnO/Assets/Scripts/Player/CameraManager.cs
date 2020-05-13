using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;
    Vector3 offset;
    int max;

    void Start()
    {
        offset = target.position - transform.position;
        target = GameObject.FindGameObjectWithTag("PLAYER").transform;
    }

    private void LateUpdate()
    {
        if(target.GetComponent<MoveRotation1>().isDie == true)
        {
            //StartCoroutine(Zoom());
        }
        else
        {
            Vector3 pos = transform.position;
            Vector3 targetPos = target.position;
            if (target.GetComponent<MoveRotation1>().isSurvival == false)
            {
                pos.x = Mathf.Clamp(pos.x, -20.0f, 21.0f);
                pos.z = Mathf.Clamp(pos.z, -20.0f, 40.0f);

                targetPos.x = Mathf.Clamp(targetPos.x, -30.0f, 34.0f);
                targetPos.z = Mathf.Clamp(targetPos.z, -12.0f, 45.0f);
                target.position = targetPos;

                transform.position = pos;
                transform.position = Vector3.MoveTowards(transform.position, target.position - offset, Time.deltaTime * 5.0f);
            }
            else
            {
                pos.x = Mathf.Clamp(pos.x, 79.0f, 134.0f);
                pos.z = Mathf.Clamp(pos.z, -23.0f, 41.0f);

                targetPos.x = Mathf.Clamp(targetPos.x, 78.0f, 135.0f);
                targetPos.z = Mathf.Clamp(targetPos.z, -18.0f, 40.5f);
                target.position = targetPos;

                transform.position = pos;
                transform.position = Vector3.MoveTowards(transform.position, target.position - offset, Time.deltaTime * 5.0f);
            }
        }
        //if (Input.GetMouseButton(1)) StartCoroutine(Zoom());
    }

    IEnumerator Zoom()
    {
        while (true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z + 0.05f);
            if (transform.position.y <= 8.0f)
            {
                break;
            }
            yield return null;
        }
    }
}
