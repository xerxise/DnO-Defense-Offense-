using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindManager : MonoBehaviour
{
	public static GameObject FindObject(string _name)
	{
		return GameObject.Find(_name);
	}
}
