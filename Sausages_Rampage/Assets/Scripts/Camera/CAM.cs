using UnityEngine;
using System.Collections;

public class CAM : MonoBehaviour {

	Vector3 NewPos;
	public float SpeedCam=10f;
	public GameObject Target;
	public float DistZ=10f;
	public float DistY=10f;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		NewPos = new Vector3 (Target.GetComponent <Transform> ().position.x, DistY,Target.GetComponent <Transform> ().position.z-DistZ) ;
		
		//transform.position = Vector3.Lerp (transform.position, NewPos, SpeedCam * Time.deltaTime);
		
		transform.LookAt (Target.GetComponent <Transform> ().position);
		
	}
}