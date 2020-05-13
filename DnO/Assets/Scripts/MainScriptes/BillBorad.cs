using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBorad : MonoBehaviour
{
	private Transform camTr;
	private Transform tr;
    void Start()
    {
		camTr = Camera.main.transform;
		tr = transform;
    }

    // Update is called once per frame
    void Update()
    {
		tr.LookAt(camTr.position);
    }
}
