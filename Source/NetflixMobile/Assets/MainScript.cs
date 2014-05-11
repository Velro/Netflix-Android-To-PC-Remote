using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour {
    string str = "";
    public GUISkin mySkin;


    void Awake()
    {       
         networkView.group = 1;
        MasterServer.RequestHostList("NetflixSpacebar");
        networkView.group = 1;
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        str = Network.TestConnectionNAT().ToString();
        GUI.Label(new Rect(0, Screen.height - 100, Screen.width, 50),"Info: "+str, mySkin.label);
        HostData[] data = MasterServer.PollHostList();

        foreach (HostData element in data)
        {
            GUI.skin = mySkin;
            string name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
            GUILayout.Label(name);
            GUILayout.Space(5);
            string hostInfo = "[";
            foreach (string host in element.ip)
            {
                hostInfo = hostInfo + host + ":" + element.port + "";
            }
            hostInfo = hostInfo + "]";
            GUILayout.Label(hostInfo);
            GUILayout.Space(5);
            GUILayout.Label(element.comment);
            GUILayout.Space(5);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Connect"))
            {
                Network.Connect(element);
            }
        }

        if (MasterServer.PollHostList().Length == 0)
        {
            GUILayout.Label("No hosting servers found");
        }

        if (Network.peerType == NetworkPeerType.Connecting)
            GUILayout.Label("Connecting...");
        if (Network.peerType != NetworkPeerType.Client)
            return;

        GUILayout.Space(10);
        if (GUI.Button(new Rect((Screen.width/2) - 300, (Screen.height/2) - 200, 600, 400),"Pause/Unpause"))
        {
            networkView.RPC("SendSpacebarToApp", RPCMode.Server);
        }
    }

    void OnFailedToConnect(NetworkConnectionError error) {
		str = "Could not connect to server: "+ error;
	}

    [RPC]
    void SendSpacebarToApp()
    {

    }
}
