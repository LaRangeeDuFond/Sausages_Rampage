using UnityEngine;
using System.Collections;

public class LoginTxt : MonoBehaviour {
	public string login3;
	public TextMesh monTextMesh;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		monTextMesh.text=login3;
	
	}
}
