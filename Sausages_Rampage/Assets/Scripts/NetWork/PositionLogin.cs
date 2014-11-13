using UnityEngine;
using System.Collections;

public class PositionLogin : MonoBehaviour {
	public GameObject hips;
	public float offsetX = 0.02f;
	public float offsetY = 0.75f;

	// Use this for initialization
	void Start () 
	{

	
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = new Vector3(hips.transform.position.x+offsetX,hips.transform.position.y+offsetY,hips.transform.position.z);
	}
}
