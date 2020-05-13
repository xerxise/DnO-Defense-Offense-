using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gost : MonoBehaviour
{
	[SerializeField] float m_angle = 0f;
	[SerializeField] float m_distance = 0f;
	[SerializeField] LayerMask m_layerMask = 0;
	//void Singt()
	//{
	//	Collider[] t_cols = Physics.OverlapSphere(transform.position, m_distance, m_layerMask);//주변에 콜라이더 검출
	//	if(t_cols.Length > 0)
	//	{
	//		Transform t_tfPlayer = t_cols[0].transform;//플레이어는 1명뿐 그래서 렝스값이 0
	//		Vector3 t_direction = (t_tfPlayer.position - transform.position).normalized;//플레이어가 어디있는지 방향을 구함
	//		float t_angle = Vector3.Angle(t_direction, transform.forward);//AI &  플레이어 방향의 각도차이가 시야각 보다 작은지 
	//		if(t_angle < m_angle * 0.5f)//시야각 0.5 절반 (왼쪽 + 오른쪽)
	//		{
	//			if(Physics.Raycast(transform.forward,t_direction, out RaycastHit t_hit, m_distance))//시야안에 있으면 hit를 쏴라
	//			{
	//				if(t_hit.transform.name == "Human")
	//				{
	//					Debug.Log("사람");
	//					transform.position = Vector3.Lerp(transform.position, t_hit.transform.position, 0.02f);
	//				}
	//			}
	//		}
	//	}
	//}
	void Update()
	{
		//Singt();
	}
	private void Start()
	{
		StartCoroutine(Rader());
	}
	IEnumerator Rader()
	{
		Collider[] coll = Physics.OverlapSphere(transform.position, m_distance, m_layerMask);
		while (true)
		{
			for (int i = 0; i < coll.Length; i++)
			{
				if (coll.Length > 0)
				{
					Transform Player = coll[0].transform;
					//Debug.Log(coll[0].name);
					Vector3 t_direction = (Player.position - transform.position).normalized;
					Vector3 t_direction_a = Player.position - transform.position;
					//Debug.Log(t_direction);
					float t_angle = Vector3.Angle(t_direction, transform.forward);
					//Debug.Log(t_angle);
					Debug.DrawRay(transform.position, t_direction_a, Color.red, 1000f);
					if (t_angle < m_angle * 1f)
					{
						Debug.Log("ffffff" + m_angle * 1f);
						RaycastHit hit;
						if (Physics.Raycast(transform.position, t_direction_a, out hit, 300f, 1 << 9))
						{
							Debug.DrawRay(transform.position + Vector3.up * 0.5f, t_direction * 3f, Color.green, 1000f);
							Debug.Log(hit.collider.name);
							if (hit.transform.name == "Human")
							{
								Debug.Log("사람");
								transform.position = Vector3.Lerp(transform.position, hit.transform.position, 0.02f);
							}
						}
					}

					yield return new WaitForSeconds(0.5f);
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, 3f);
		for (int i = 0; i < 45; i++)
		{

		}
	}
}
