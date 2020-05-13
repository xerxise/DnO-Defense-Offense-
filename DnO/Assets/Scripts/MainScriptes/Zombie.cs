using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
	[SerializeField] Transform m_tfGunbody = null;
	[SerializeField] float m_range = 0f;
	[SerializeField] LayerMask m_layerMask = 0;
	[SerializeField] float m_spinSpeed = 0f;
	[SerializeField] Animator animator;
	Transform m_tfTarget = null;
	private bool is_walk = false;
	private bool is_attck = false;
	private Transform tr;
	Transform HumanTr;
	void SearchEnemy()
	{
		Collider[] t_cols = Physics.OverlapSphere(transform.position, m_range, m_layerMask);
		Transform t_shortestTarget = null;
		if (t_cols.Length > 0)
		{
			float t_shortestDistance = Mathf.Infinity;
			foreach (Collider t_colTarget in t_cols)
			{
				float t_distance = Vector3.SqrMagnitude(transform.position - t_colTarget.transform.position);
				if (t_shortestDistance > t_distance)
				{
					t_shortestDistance = t_distance;
					t_shortestTarget = t_colTarget.transform;
				}
			}
		}
		m_tfTarget = t_shortestTarget;
	}
	void Start()
	{
		tr = GetComponent<Transform>();
		HumanTr = GameObject.FindGameObjectWithTag("ZOM").GetComponent<Transform>();
		InvokeRepeating("SearchEnemy", 0f, 0.5f);
		animator = transform.GetChild(0).GetComponent<Animator>();
		
	}
	void Update()
	{
		
		if (m_tfTarget == null)
		{
			animator.SetBool("is_walk", false);
			animator.SetBool("is_attack", true);
		}
		else
		{
			Quaternion t_lookRotation = Quaternion.LookRotation(m_tfTarget.position);
			Vector3 t_euler = Quaternion.RotateTowards(m_tfGunbody.rotation, t_lookRotation, m_spinSpeed * Time.deltaTime).eulerAngles;
			m_tfGunbody.rotation = Quaternion.Euler(0, t_euler.y, 0);
			Debug.Log("사람");
			Quaternion t_fireRotaion = Quaternion.Euler(0, t_lookRotation.eulerAngles.y, 0);

			transform.position = Vector3.MoveTowards(transform.position, m_tfTarget.position, Time.deltaTime * 2f);
			animator.SetBool("is_walk", true);

			Vector3 dir = m_tfTarget.position - transform.position;
			float dist = Vector3.Distance(dir, m_tfTarget.position);
			if (dist <= 1.0f)
			{
				is_attck = true;
			}
			else if (dist <= 5.0f)
			{
				tr.position = HumanTr.position;
				is_attck = false;
			}
			else
			{
				//tr.position =
				//	is_attck = false;
			}
			if (!is_attck)
			{
				animator.SetBool("is_walk", true);

			}
			Quaternion rototation = Quaternion.LookRotation(dir);
			float y = Mathf.Atan2(dir.x
			, dir.z) * Mathf.Rad2Deg;//밑변 높이 삼각비 tan
			Quaternion rot = Quaternion.Euler(0, y, 0);
			transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, rototation, Time.deltaTime * 20f);

			
		}
		
		
	}
	
}
