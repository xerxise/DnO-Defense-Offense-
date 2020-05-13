using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendAttack : MonoBehaviour
{
    float tx, ty, tz;
    float dat, gravity, _height;

    Vector3 _sPos, _ePos;

    public GameObject bullet;

    private GameObject go;

    private Transform playerTr;
    private Transform barricadeTr;
    private Transform friendTr;

    private MoveAgent agent;
    private BuildManager bManager;

    private float nextFire = 0.0f;
    private readonly float fireRate = 0.5f;
    private readonly float damping = 10.0f;
    public bool isFire = false;

    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        friendTr = GetComponent<Transform>();
        agent = GetComponent<MoveAgent>();
        bManager = GameObject.Find("Build").GetComponent<BuildManager>();
        gravity = 9.81f;
        _height = 5.0f;
    }

    void Update()
    {
        if (isFire)
        {
            if (Time.time >= nextFire)
            {
                Shoot(transform.position, agent.traceTarget, gravity, _height);
                nextFire = Time.time + fireRate + Random.Range(1.0f, 1.5f);
            }

            Quaternion rot = Quaternion.LookRotation(agent.traceTarget - friendTr.position);
            friendTr.rotation = Quaternion.Slerp(friendTr.rotation, rot, Time.deltaTime * damping);
        }
    }

    public void Shoot(Vector3 _startPos, Vector3 _endPos, float _g, float _maxHeight)
    {
        _sPos = _startPos;
        _ePos = _endPos;
        gravity = _g;
        _height = _maxHeight;
        go = Instantiate(bullet);

        float dh = _ePos.y - _sPos.y;
        float mh = _height - _sPos.y;
        ty = Mathf.Sqrt(2 * gravity * Mathf.Abs(mh));
        if (mh < 0)
        {
            ty *= -1;
        }
        float a = gravity;
        float b = -2 * ty;
        float c = 2 * dh;

        if (b > 0)
        {
            dat = (b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a); // time fomula
        }
        else if (b == 0)
        {
            dat = 1.0f;
        }
        else
        {
            dat = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a); // time fomula
        }
        tx = -(_sPos.x - _ePos.x) / dat;
        tz = -(_sPos.z - _ePos.z) / dat;

        StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        float time2 = 0.0f;
        if (_height < 0)
        {
            while (go != null)
            {
                time2 += Time.deltaTime * 2.0f;
                var tx = _sPos.x + this.tx * time2;
                var ty = _sPos.y + this.ty * time2 + 0.5f * gravity * time2 * time2;
                var tz = _sPos.z + this.tz * time2;
                var tpos = new Vector3(tx, ty, tz);
                go.transform.LookAt(tpos);
                go.transform.position = tpos;
                if (time2 >= dat)
                {
                    if (Vector3.Distance(go.transform.position, agent.traceTarget) < 0.2f)
                    {
                        
                    }
                    Destroy(go);
                    break;
                }
                yield return null;
            }
        }
        else
        {
            while (go != null)
            {
                time2 += Time.deltaTime * 2.0f;
                var tx = _sPos.x + this.tx * time2;
                var ty = _sPos.y + this.ty * time2 - 0.5f * gravity * time2 * time2;
                var tz = _sPos.z + this.tz * time2;
                var tpos = new Vector3(tx, ty, tz);
                go.transform.LookAt(tpos);
                go.transform.position = tpos;
                if (time2 >= dat)
                {
                    Destroy(go);
                    break;
                }
                yield return null;
            }
        }
    }
}
