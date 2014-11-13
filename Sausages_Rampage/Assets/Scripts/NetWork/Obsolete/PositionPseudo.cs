using UnityEngine;
using System.Collections;

public class PositionPseudo : MonoBehaviour {

	public Transform HipsSaucisse;

	private Vector3 Ecart;

	// Use this for initialization
	void Start () {
	
		Ecart = transform.position - HipsSaucisse.position;

	}
	
	// Update is called once per frame
	void Update () {
	
		transform.position = HipsSaucisse.position + Ecart;

	}
}
