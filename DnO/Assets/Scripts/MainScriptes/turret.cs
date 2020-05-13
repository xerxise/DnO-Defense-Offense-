using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour
{
	[SerializeField] Transform m_tfGunbody = null;
	[SerializeField] float m_range = 0f;
	[SerializeField] LayerMask m_layerMask = 0;
	[SerializeField] float m_spinSpeed = 0f;
	[SerializeField] float m_fireRate = 0;
	float m_currentFireRate;
	Transform m_tfTarget = null;


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
		m_currentFireRate = m_fireRate;
		InvokeRepeating("SearchEnemy", 0f, 0.5f);
	}
	void Update()
	{
		if (m_tfTarget == null)
		{
			m_tfGunbody.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
		}
		else
		{
			Quaternion t_lookRotation = Quaternion.LookRotation(m_tfTarget.position);
			Vector3 t_euler = Quaternion.RotateTowards(m_tfGunbody.rotation, t_lookRotation, m_spinSpeed * Time.deltaTime).eulerAngles;
			m_tfGunbody.rotation = Quaternion.Euler(0, t_euler.y, 0);
			//Debug.Log("사람");

			Quaternion t_fireRotaion = Quaternion.Euler(0, t_lookRotation.eulerAngles.y, 0);
			if (Quaternion.Angle(m_tfGunbody.rotation, t_fireRotaion) < 5f)
			{
				m_currentFireRate -= Time.deltaTime;
				if (m_currentFireRate <= 0)
				{
					m_currentFireRate = m_fireRate;
					//Debug.Log("발사");
				}
			}
		}
	}
}
