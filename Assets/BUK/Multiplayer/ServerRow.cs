using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Mirror;

namespace BUK.Multiplayer 
{
    public class ServerRow : MonoBehaviour
    {
        private TextMeshProUGUI serverName;
        private TextMeshProUGUI players;
        private TextMeshProUGUI ip;

        private Uri serverUri;

        void Awake()
        {
            serverName = transform.Find("Vertical/ServerName").GetComponent<TextMeshProUGUI>();
            players = transform.Find("Players").GetComponent<TextMeshProUGUI>();
            ip = transform.Find("Vertical/IP").GetComponent<TextMeshProUGUI>();
        }
    
        public void JoinGame()
        {
            if (serverUri == null)
                return;
            var networkManager = (BioNetworkManager) NetworkManager.singleton;
            networkManager.StartClient(serverUri);
        }

        public void SetValues(DiscoveryResponse response)
        {
            serverName.text = response.serverName;
            players.text = response.totalPlayers.ToString();
            ip.text = response.uri.Host;
            serverUri = response.uri;
        }
    }
}
