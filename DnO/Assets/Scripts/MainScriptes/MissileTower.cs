using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTower : MonoBehaviour
{
	[SerializeField] GameObject m_goMissile = null;
	[SerializeField] Transform m_tfMissileSpawn = null;
	float height;
	float gravite;
	Vector3 s_pos;
	Vector3 e_pos;
	float tx;
	float ty;
	float tz;
	float dat;
	public GameObject S_pos;
	public GameObject E_pos;

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
		//{
		//	GameObject t_misslie = Instantiate(m_goMissile, m_tfMissileSpawn.position, Quaternion.identity);
		//	t_misslie.GetComponent<Rigidbody>().velocity = t_misslie.transform.forward * 5f;
		//}
    }
	
	void Start()
	{
		S_pos = GameObject.Find("Cylinder");
		E_pos = GameObject.Find("Woman");
		gravite = 9.81f;
		height = 20;
		s_pos = E_pos.transform.position;
		e_pos = S_pos.transform.position;
		m_goMissile.transform.position = transform.position;
		ShotTest(m_goMissile.transform, s_pos, e_pos, gravite, height);//NaN값이 발생 
		StartCoroutine(ArrowCoroutien());
	}

	void ShotTest(Transform bull, Vector3 s, Vector3 e, float g, float h)
	{
		float sah = e.y - s.y;
		float mah = h - s.y;
		if (mah <= 0) mah = mah * -1f;
		ty = Mathf.Sqrt(2 * g * mah);

		float a = gravite;
		float b = -2 * ty;
		float c = 2 * sah;

		dat = (-b + Mathf.Sqrt(b * b - 4 * a * c * c)) / (2 * a);

		tx = (e_pos.x - s_pos.x) / dat;
		tz = (e_pos.z - s_pos.z) / dat;

		StartCoroutine(ArrowCoroutien());
	}
	IEnumerator ArrowCoroutien()
	{
		float t = 0;
		while (true)
		{
			t += Time.deltaTime;
			float x = s_pos.x + tx * t;

			float y = s_pos.y + ty * t - (0.5f * gravite * t * t);
			if (height < 0)
			{
				y = y * -1f;
			}
			float z = s_pos.z + tz * t;

			Vector3 pos;
			pos = new Vector3(x, y, z);
			m_goMissile.transform.position = pos;
			yield return null;
			if (t >= dat) break;
		}

	}

	IEnumerator Bullet_fire()
	{
		while (true)
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				yield return new WaitForSeconds(2f);
				Instantiate(m_goMissile);
			}
			
		}
	}
}
