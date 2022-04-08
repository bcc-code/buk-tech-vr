using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Mirror;

namespace Buk.Multiplayer 
{
    /// <summary>
    /// The component for the UI element displaying information about a server after discovery
    /// </summary>
    public class ServerRow : MonoBehaviour
    {
        private TextMeshProUGUI serverName;
        private TextMeshProUGUI players;
        private TextMeshProUGUI ip;

        private Uri serverUri;

        /// <summary>
        /// On Awake find the relevant UI components for the ServerRow
        /// </summary>
        void Awake()
        {
            serverName = transform.Find("Vertical/ServerName").GetComponent<TextMeshProUGUI>();
            players = transform.Find("Players").GetComponent<TextMeshProUGUI>();
            ip = transform.Find("Vertical/IP").GetComponent<TextMeshProUGUI>();
        }
    
        /// <summary>
        /// Called when clicking the Join button for a server
        /// </summary>
        public void JoinGame()
        {
            if (serverUri == null)
                return;
            // Get the BukNetworkManager and StartClient with the remote serverUri to join the server
            var networkManager = (BukNetworkManager) NetworkManager.singleton;
            networkManager.StartClient(serverUri);
        }

        /// <summary>
        /// Set the relevant values for the ServerRow based on the DiscoveryResponse
        /// </summary>
        public void SetValues(DiscoveryResponse response)
        {
            serverName.text = response.serverName;
            players.text = response.totalPlayers.ToString();
            ip.text = response.uri.Host;

            // The serverUri is needed for joining the game with the Join button
            serverUri = response.uri;
        }
    }
}
