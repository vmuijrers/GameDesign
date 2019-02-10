using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSwapper : MonoBehaviour {
    private MeshFilter[] meshFilters;
    public GameObject fbxReference;
	// Use this for initialization
	void Start () {
        meshFilters = GetComponentsInChildren<MeshFilter>();
        foreach(MeshFilter mf in meshFilters) {
            mf.mesh = fbxReference.transform.Find(mf.mesh.name).GetComponent<MeshFilter>().mesh;
        }
	}

}
