using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
	public Transform player;
	public GameObject gridprefab;
	public Vector3 pos;
	Transform[,] grid;
	int max;
	public Vector3 startPos;
	public Vector3 endPos;
	public List<Transform> path;

	void Creat_grid()
	{
		grid = new Transform[max, max];//메모리 잡기
		for (int i = 0; i < max; i++)
		{
			for (int j = 0; j < max; j++)
			{
				GameObject obj = Instantiate(gridprefab);
				obj.name = i + ":" + j;
				obj.layer = 10;
				obj.AddComponent<Node>();
				obj.GetComponent<Node>().x = i;
				obj.GetComponent<Node>().z = j;
				obj.transform.position = pos + new Vector3(i, 0, j);
				obj.transform.SetParent(transform);
				grid[i, j] = obj.transform;
			}
		}
	}
	void Init_visited()
	{

		foreach (Transform t in grid)
		{
			t.GetComponent<Node>().visited = -1;
			//t.GetChild(0).GetChild(0).GetComponent<Text>().text = (-1).ToString();
		}
		grid[(int)startPos.x, (int)startPos.z].GetComponent<Node>().visited = 0;
		//grid[(int)startPos.x, (int)startPos.z].GetChild(0).GetChild(0).GetComponent<Text>().text = (0).ToString();
	}
	bool is_visited(int x, int z, int _visited, int dir)
	{
		switch (dir)
		{
			case 1:
				if (x - 1 > -1 && grid[x - 1, z].GetComponent<Node>().visited == _visited && grid[x - 1, z].gameObject.layer == 10)
					return true;
				else return false;
			case 2:
				if (z - 1 > -1 && grid[x, z - 1].GetComponent<Node>().visited == _visited && grid[x, z - 1].gameObject.layer == 10)
					return true;
				else return false;
			case 3:
				if (x + 1 < max && grid[x + 1, z].GetComponent<Node>().visited == _visited && grid[x + 1, z].gameObject.layer == 10)
					return true;
				else return false;
			case 4:
				if (z + 1 < max && grid[x, z + 1].GetComponent<Node>().visited == _visited && grid[x, z + 1].gameObject.layer == 10)
					return true;
				else return false;
		}
		return false;
	}
	void Set_visited(int x, int z, int _visited)
	{
		if (grid[x, z])
		{
			grid[x, z].GetComponent<Node>().visited = _visited;
			//grid[x, z].GetChild(0).GetChild(0).GetComponent<Text>().text = _visited.ToString();
		}
	}
	void Set_direction_visited(int x, int z, int _visited)
	{
		if (is_visited(x, z, -1, 1)) Set_visited(x - 1, z, _visited);
		if (is_visited(x, z, -1, 2)) Set_visited(x, z - 1, _visited);
		if (is_visited(x, z, -1, 3)) Set_visited(x + 1, z, _visited);
		if (is_visited(x, z, -1, 4)) Set_visited(x, z + 1, _visited);
	}
	void Set_distance()
	{
		for (int i = 1; i < grid.Length; i++)//1로 시작
		{
			foreach (Transform t in grid)//처음부터 흝어주기
			{
				if (t.GetComponent<Node>().visited == i - 1)//0이라면
					Set_direction_visited(t.GetComponent<Node>().x, t.GetComponent<Node>().z, i);
			}
		}
	}
	void Find_path()
	{
		int step = 0;
		int x = (int)endPos.x;
		int z = (int)endPos.z;
		if (path.Count > 0)
		{
			for (int i = 0; i < path.Count; i++)
			{
				path[i].GetComponent<MeshRenderer>().material.color = Color.white;
			}
		}
		path.Clear();
		List<Transform> text_list = new List<Transform>();
		if (grid[(int)endPos.x, (int)endPos.z].GetComponent<Node>().visited > 0)
		{
			path.Add(grid[(int)endPos.x, (int)endPos.z]);
			step = grid[x, z].GetComponent<Node>().visited - 1;
		}
		else
		{
			Debug.Log("endPos값이 없습니다");
			return;
		}
		for (int i = step; i > -1; i--)
		{
			if (is_visited(x, z, i, 1))
				text_list.Add(grid[x - 1, z]);
			if (is_visited(x, z, i, 2))
				text_list.Add(grid[x, z - 1]);
			if (is_visited(x, z, i, 3))
				text_list.Add(grid[x + 1, z]);
			if (is_visited(x, z, i, 4))
				text_list.Add(grid[x, z + 1]);

			GameObject a = find_obj(grid[(int)endPos.x, (int)endPos.z], text_list);
			x = a.GetComponent<Node>().x;
			z = a.GetComponent<Node>().z;
			path.Add(a.transform);
			text_list.Clear();
		}
		path.Reverse();
	}

	GameObject find_obj(Transform target, List<Transform> _list)
	{
		int count = 0;
		float distance = max * max;
		for (int i = 0; i < _list.Count; i++)
		{
			if (Vector3.Distance(target.position, _list[i].position) < distance)
			{
				distance = Vector3.Distance(target.position, _list[i].position);
				count = i;
			}
		}
		return _list[count].gameObject;
	}

	void Color_path()
	{
		for (int i = 0; i < path.Count; i++)
		{
			path[i].GetComponent<MeshRenderer>().material.color = Color.cyan;
		}
	}

	void Start()
	{
		max = 50;
		Creat_grid();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			number = 0;
			startPos = endPos;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100f, 1 << 10))
			{
				endPos.x = hit.collider.GetComponent<Node>().x;
				endPos.z = hit.collider.GetComponent<Node>().z;
				Inital_test();
				Debug.Log(hit.collider.name);
			}
		}
		if (Input.GetMouseButtonDown(1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100f, 1 << 10))
			{
				hit.collider.GetComponent<MeshRenderer>().material.color = Color.red;
				hit.collider.gameObject.layer = 11;
			}
		}
		if (path.Count > 0)
		{
			if (Vector3.Distance(player.position, path[number].position) < 0.05f)
			{
				number++;
			}
			if (number >= path.Count)
			{
				number = path.Count - 1;
				return;
			}
			player.position = Vector3.MoveTowards(player.position, path[number].position, Time.deltaTime * 10f);
		}
	}
	int number;
	void Inital_test()
	{
		Init_visited();
		Set_distance();
		Find_path();
		Color_path();
	}
}
