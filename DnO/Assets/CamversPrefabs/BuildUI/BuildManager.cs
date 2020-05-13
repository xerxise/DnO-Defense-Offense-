// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;
// using UnityEditor;

// public class BuildManager : MonoBehaviour
// {
//     private GraphicRaycaster gr;

//     public GameObject obstacle;
//     private GameObject go;
//     private GameObject player;

//     public List<Transform> obstacleList;
//     public int count = 0;

//     private void Awake()
//     {
//         gr = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
//         player = GameObject.FindGameObjectWithTag("PLAYER");
//         obstacleList = new List<Transform>();
//     }

//     private void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             var ped = new PointerEventData(null)
//             {
//                 position = Input.mousePosition
//             };
//             List<RaycastResult> results = new List<RaycastResult>();
//             gr.Raycast(ped, results);

//             if (results.Count <= 0) return;
//             RaycastHit hitInfo;
//             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//             if (Physics.Raycast(ray, out hitInfo))
//             {
//                 go = Instantiate(obstacle, new Vector3(hitInfo.point.x + 2.0f, hitInfo.point.y + 0.5f, hitInfo.point.z), transform.rotation);
//                 obstacleList.Add(go.transform);
//                 go.GetComponent<RepairManager>().listNum = count;
//                 count++;
//             }
//         }
//     }

//     public void DestroyedObstacle(int index)
//     {
//         if (obstacleList.Count == 0)
//         {
//             count = 0;
//         }
//         else if(obstacleList.Count == 1)
//         {
//             obstacleList[0].GetComponent<RepairManager>().listNum = 0;
//             count = 1;
//         }
//         else
//         {
//             for (int i = index; i < obstacleList.Count; i++)
//             {
//                 obstacleList[i].GetComponent<RepairManager>().listNum -= 1;
//                 count = obstacleList.Count - 1;
//             }
//         }
//     }
// }
