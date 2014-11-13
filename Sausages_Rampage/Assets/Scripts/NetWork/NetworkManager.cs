using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

	public string IP = "127.0.0.1";
	public string IP2 = "193.54.159.108";//193.54.159.64
	public int Port = 25001;
	//private const string typeName = "UniqueGameName";
	//private const string gameName = "RoomName";
	private HostData[] hostList;
	public GameObject player;
	private GameObject monplayer;
	public GameObject MaCamera;
	public NetworkViewID IDplayer;

	private GameObject instanceCam;

	//public TextMesh monTexte;
	public string stringToEdit = "" ;
	public string stringAffiche = "";
	public int NbMessagesAffiches = 5;
	public List<string> monChat = new List<string>();
	public string login = "A.N.Onyme";

	public int NbConnexionsServer = 0;

	// Use this for initialization

	void Start () 
	{
		Input.eatKeyPressOnTextFieldFocus = false;
		//MasterServer.ipAddress = "127.0.0.1";
	}

	/*private void StartServer()
	{
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
		SpawnPlayer();
	}

	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
	}

	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}

	private void RefreshHostList()
	{
		MasterServer.ipAddress = "193.54.159.108";
		MasterServer.port= 25001;
		MasterServer.RequestHostList(typeName);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
	
	void OnConnectedToServer()
	{
		Debug.Log("Server Joined");
		SpawnPlayer();
	}
	*/
	void OnGUI ()
	{
		
		
				if (Network.peerType == NetworkPeerType.Disconnected) 
		{
			
			
						if (GUI.Button (new Rect (100, 100, 100, 25), "Start Client"))
			{
								login = "A.N.Onyme";
								Network.Connect (IP2, Port);
				Debug.Log ("Network.Connect (IP2, Port);");
								//SpawnPlayer();
			}
						if (GUI.Button (new Rect (100, 125, 100, 25), "Start Server")) 
			{
								Network.InitializeServer ( 10, Port);
								login = "Admin";
				SpawnPlayer();
			}
		}
		
				else 
		{
			if (Network.peerType == NetworkPeerType.Client) 
			{
				GUI.Label (new Rect (10, 10, 200, 30), "Pseudo : ");
				GUI.Label (new Rect (10, 30, 200, 30), "Chat   : ");
				stringToEdit = GUI.TextField (new Rect (70, 30, 200, 20), stringToEdit, 200);
				login = GUI.TextField (new Rect (70, 10, 80, 20), login, 200);
				GUI.Label (new Rect (300, 30, 200, 100), stringAffiche);
				
				GUI.Label (new Rect (100, 100, 100, 25), "Client");
				
				if (GUI.Button (new Rect (100, 125, 100, 25), "Logout")) 
				{
					Network.Disconnect (250);
				}
			}
			
			if (Network.peerType == NetworkPeerType.Server) 
			{
				//login = "admin";
				
				GUI.Label (new Rect (10, 30, 200, 30), "Chat   : ");
				stringToEdit = GUI.TextField (new Rect (70, 30, 200, 20), stringToEdit, 200);
				GUI.Label (new Rect (300, 30, 200, 100), stringAffiche);
				
				GUI.Label (new Rect (10, 10, 200, 30), "Pseudo : ");
				login = GUI.TextField (new Rect (70, 10, 80, 20), login, 200);
				
				
				GUI.Label (new Rect (100, 100, 100, 25), "Server");
				GUI.Label (new Rect (100, 125, 100, 25), "Connecions : " + Network.connections.Length);
				
				if (GUI.Button (new Rect (90, 60, 100, 25), "Logout")) 
				{
					Network.Disconnect (250);
				}

				
			}
		}
	}

	private void SpawnPlayer()
	{

		monplayer = Network.Instantiate(player, new Vector3(0f, 5f, 0f), Quaternion.identity,0) as GameObject;
		IDplayer = monplayer.networkView.viewID;
		networkView.RPC("ChangeLogin",RPCMode.All,login,IDplayer );
		instanceCam = Instantiate(MaCamera, new Vector3(0f, 5f, -10f), Quaternion.identity) as GameObject;
		instanceCam.transform.parent = monplayer.transform;

	}
	void Affiche (List<string> maList)
	{
		int i = 0;
		
		stringAffiche = "";
		
		for (i=0; i < maList.Count; i++)
		{
			//Debug.Log (maList [i]);
			
			if (i>maList.Count-NbMessagesAffiches)
			{
				stringAffiche = stringAffiche  + maList [i] + "\n";
			}
		}	
	}

	void OnConnectedToServer()
	{

		Debug.Log("Server Joined");
		SpawnPlayer();
		networkView.RPC("ChangeLogin",RPCMode.All,login,IDplayer );
	}
	// Update is called once per frame
	void Update ()
	{


		if (Network.peerType == NetworkPeerType.Server) 
		{
			if (NbConnexionsServer != Network.connections.Length)
			{
				networkView.RPC("EnvoyerChangeLogin",RPCMode.All);
				NbConnexionsServer = Network.connections.Length;
			}
		}

		//monTexte.text = GUI.TextField (new Rect (70, 10, 80, 20), login, 200);


		if (Input.GetKeyDown (KeyCode.Return))
		{
			//monChat.Add (stringToEdit);
			networkView.RPC("EnvoiText", RPCMode.All, login, stringToEdit);
			stringToEdit = "";	
			networkView.RPC("ChangeLogin",RPCMode.All,login,IDplayer );
		}	
	}
	[RPC]
	void EnvoiText (string log ,string nouvelleEntree)
	{
		
		monChat.Add (System.DateTime.Now.ToString("hh:mm:ss")+"   " + log+" : "+nouvelleEntree);
		Affiche (monChat);
	}

	[RPC]
	void EnvoyerChangeLogin ()
	{
		networkView.RPC("ChangeLogin",RPCMode.All,login,IDplayer );
	}

	[RPC]
	void ChangeLogin (string login2 ,NetworkViewID ID)
	{
		Debug.Log (login2);
		Debug.Log (ID);
		//NetworkView.Find(IDplayer).GetComponent<LoginTxt>().login3= login2;
		GameObject[] listeSaucisse = GameObject.FindGameObjectsWithTag ("UneSaucisse");

		foreach (GameObject maSaucisse in listeSaucisse) 
		{
			if (maSaucisse.networkView.viewID == ID)
			{
				maSaucisse.GetComponent<LoginTxt>().login3= login2;
			}
		}

	}

}
