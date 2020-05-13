using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairManager : MonoBehaviour
{
    public int wallHealth;
    public int wallMaxHealth = 30;
    public int listNum;

    private BuildManager bManager;
    private UIManager uiManager;
    public bool isSelected = true;
    public bool isRepair = false;
    public bool isSettible = false;

    private void Awake()
    {
        bManager = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<BuildManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        wallHealth = wallMaxHealth;
    }

    private void Update()
    {
        if(wallHealth <= 0)
        {
            bManager.obstacleList.RemoveAt(listNum);
            bManager.DestroyedObstacle(listNum);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("OBSTACLE"))
        {
            other.GetComponent<RepairManager>().isSettible = true;
        }
        if (wallHealth == wallMaxHealth) return;
        if(other.CompareTag("PLAYER"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        if (Input.GetMouseButtonDown(0))
        {
            isRepair = true;
            Repair();
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
            isRepair = false;
        }
        if (other.CompareTag("OBSTACLE"))
        {
            other.GetComponent<RepairManager>().isSettible = false;
        }
    }

    void Repair()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.name == "Repair")
            {
                StartCoroutine(RepairCoroutine());
            }
        }
    }

    public void WallDamaged(int damage)
    {
        wallHealth -= damage;
    }

    IEnumerator RepairCoroutine()
    {
        while(isRepair == true)
        {
            wallHealth += 4;
            uiManager.UseGold(10);
            if(wallHealth >= wallMaxHealth)
            {
                wallHealth = wallMaxHealth;
                transform.GetChild(0).gameObject.SetActive(false);
                break;
            }
            yield return new WaitForSeconds(1.5f);
        }
    }
    
    private Vector3 mOffset;
    private Vector3 mousePoint;
    private Vector3 goOffset;
    private float mZCoord;

    public void OnMouseDown()

    {
        mZCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        mOffset = transform.position - GetMouseAsWorldPoint();
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        mousePoint = Input.mousePosition;

        mousePoint.z = mZCoord;


        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    public void OnMouseDrag()
    {
        if (isSelected)
        {
            transform.position = new Vector3((GetMouseAsWorldPoint() + mOffset).x, 0.5f, (GetMouseAsWorldPoint() + mOffset).z);
        }
    }

    public void OnMouseUp()
    {
        if(isSettible == true)
        {
            return;
        }
        else
        {
            uiManager.UseGold(100);
            isSelected = false;
        }
    }
}
