using UnityEngine;
using System.Collections;

public class DetecteurGras : MonoBehaviour {


	public Transform Coin1;
	public Transform Coin2;
	public Transform Coin3;
	public Transform Coin4;

	public Transform PourInstance;

	public GameObject maTache;
	public GameObject maTache2;
	public GameObject maTache3;
	public GameObject maTache4;
	public GameObject maTache5;

	private GameObject maTacheAInstantier;

	public int nbTaches = 5;


	public float longueurRayons = 0.05f;

	private bool BoolCoin1 = false;
	private bool BoolCoin2 = false;
	private bool BoolCoin3 = false;
	private bool BoolCoin4 = false;

	private RaycastHit hit;
	private RaycastHit hit2;
	private RaycastHit hit3;
	private RaycastHit hit4;
	private RaycastHit hit5;

	private Vector3 VecteurBas= new Vector3 (0, -1, 0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		BoolCoin1 = false;
		BoolCoin2 = false;
		BoolCoin3 = false;
		BoolCoin4 = false;


		if (Physics.Raycast(Coin1.position, VecteurBas, out hit, longueurRayons))
		{
			Debug.Log ("hit coin 1");
			Debug.Log (hit.transform.tag);
			if (hit.transform.tag != "tache")
			{
				BoolCoin1 = true;
			}
		}

		Debug.DrawRay (Coin1.position,new Vector3(0, -longueurRayons, 0), Color.red);

		if (Physics.Raycast(Coin2.position, VecteurBas, out hit2, longueurRayons))
		{
			Debug.Log ("hit coin 2");
			Debug.Log (hit2.transform.tag);
			if (hit2.transform.tag != "tache")
			{
				BoolCoin2 = true;
			}
		}

		Debug.DrawRay (Coin2.position,new Vector3(0, -longueurRayons, 0), Color.red);



		if (Physics.Raycast(Coin3.position, VecteurBas, out hit3, longueurRayons))
		{
			Debug.Log ("hit coin 3");
			Debug.Log (hit3.transform.tag);
			if (hit3.transform.tag != "tache")
			{
				BoolCoin3 = true;
			}
		}

		Debug.DrawRay (Coin3.position,new Vector3(0, -longueurRayons, 0), Color.red);



		if (Physics.Raycast(Coin4.position, VecteurBas, out hit4, longueurRayons))
		{
			Debug.Log ("hit coin 4");
			Debug.Log (hit4.transform.tag);
			if (hit4.transform.tag != "tache")
			{
				BoolCoin4 = true;
			}
		}

		Debug.DrawRay (Coin4.position,new Vector3(0, -longueurRayons, 0), Color.red);


		if (BoolCoin1 && BoolCoin2 && BoolCoin3 && BoolCoin4)
		{
			Debug.Log ("oui");
			if (Physics.Raycast(PourInstance.position, VecteurBas, out hit5, longueurRayons))
			{
				if (hit5.transform.tag != "tache")
				{
					Vector3 EndroitInstance = hit5.point + new Vector3(0f, 0.005f, 0f);
					//Vector3 NewV3 = Vector3(EndroitInstance.x, EndroitInstance.y, EndroitInstance.z);
					//EndroitInstance = (EndroitInstance.x, EndroitInstance.y + 0.5f, EndroitInstance.z);
					Instantiate(AuHasard (), EndroitInstance, transform.rotation);
				}
			}
		}





	}

	GameObject AuHasard ()
	{
		int NbAleatoire = Random.Range(1,nbTaches+1);

		if (NbAleatoire==1)
		{
			return maTache;
		}

		if (NbAleatoire==2)
		{
			return maTache2;
		}

		if (NbAleatoire==3)
		{
			return maTache3;
		}

		if (NbAleatoire==4)
		{
			return maTache4;
		}

		if (NbAleatoire==5)
		{
			return maTache5;
		}

		else 
		{
			return maTache;
		}

	}

}
