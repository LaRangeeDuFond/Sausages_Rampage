using UnityEngine;
using System.Collections;

public class TacheGras : MonoBehaviour {

	public float tempsDeVie = 10;
	private float compteur;

	// Use this for initialization
	void Start () {
		compteur = tempsDeVie;
	}
	
	// Update is called once per frame
	void Update () {
	
		compteur -= Time.deltaTime;

		if (compteur <= 0)
		{
			Destroy(gameObject);
		}
	}
}
