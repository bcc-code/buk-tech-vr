using Mirror;
using Mirror.Discovery;
using System.Collections.Generic;
using System;
using UnityEngine;
using Buk.Multiplayer;
using TMPro;

namespace Buk.Multiplayer
{

    /// <summary>
    /// We extend Mirror's base NetworkManager to add our custom functionality to the multiplayer server
    /// </summary>
    public class BukNetworkManager : NetworkManager
    {
        /// <summary>
        /// In this block we define several Lobby related UI elements to be set in the inspector
        /// </summary>
        [Header("Lobby Menu Objects")]
        public GameObject serverDiscoveryRow;
        public GameObject serverListContainer;
        public TMP_InputField serverNameInput;
        public TMP_InputField usernameInput;
        public GameObject serverBrowser;
        public GameObject usernameLogin;

        /// <summary>
        /// NonSerialized but public servername and username so other scripts can read these values
        /// </summary>
        [NonSerialized]
        public string name;
        [NonSerialized]
        public string username;

        /// <summary>
        /// This variable keeps track of DiscoveryResponses we received, and the UI elements we rendered for each response
        /// </summary>
        private Dictionary<DiscoveryResponse, ServerRow> servers = new Dictionary<DiscoveryResponse, ServerRow>();

        /// <summary>
        /// This method can be used on the server to replace a player prefab with another one
        /// And example use could be entering a car, then you can replace the Human player with a Car player
        /// This new player must be included in Spawnable Prefabs in the NetworkManager
        /// </summary>
        /// <param name="conn">The NetworkConnection to replace the player for</param>
        /// <param name="newPlayer">The GameObject of the new player</param>
        /// <param name="destroyOld">Whether to destroy the old player or not, this can allow for another script to make it invisible instead of destroying</param>
        /// <param name="instantiateNew">Whether to instantiate the new player here or not, this can allow another script to instantiate the player instead</param>
        /// <returns>The GameObject for the old player</returns>
        public GameObject ReplacePlayer(NetworkConnection conn, GameObject newPlayer, bool destroyOld, bool instantiateNew)
        {
            GameObject oldPlayer = conn.identity.gameObject;

            NetworkServer.ReplacePlayerForConnection(conn, instantiateNew ? Instantiate(newPlayer) : newPlayer, true);

            if (destroyOld)
                NetworkServer.Destroy(oldPlayer);

            return oldPlayer;
        }

        public void Start()
        {
            // On start set the username to null and show the usernameLogin screen and hide the serverBrowser screen
            this.username = null;
            this.serverBrowser.SetActive(false);
            this.usernameLogin.SetActive(true);
        }

        /// <summary>
        /// This method reads the username input and then shows the ServerBrowser UI
        /// </summary>
        public void SetUsername()
        {
            if (usernameInput != null && !String.IsNullOrEmpty(usernameInput.text))
            {
                // Read the username value from the usernameInput UI element
                this.username = usernameInput.text;
                // Hide the usernameLogin screen and show the serverBrowser screen
                this.usernameLogin.SetActive(false);
                this.serverBrowser.SetActive(true);
            }
        }

        /// <summary>
        /// This method gets called whenever a DiscoveryResponse from the BukNetworkDiscovery is received
        /// It then renders the info about the server and allows you to join the server through the UI
        /// </summary>
        public void OnDiscoveredServer(DiscoveryResponse response)
        {
            if (serverDiscoveryRow != null && serverListContainer != null)
            {
                // If a new server is found, spawn UI elements to show the server and add it to the servers dictionary
                if (!servers.ContainsKey(response))
                {
                    var newRow = Instantiate(serverDiscoveryRow, serverListContainer.transform).GetComponent<ServerRow>();
                    servers.Add(response, newRow);
                    newRow.SetValues(response);
                }
                else
                {
                    // If we receive new info about a server we already know, just update the UI values
                    servers[response].SetValues(response);
                }
            }
        }

        /// <summary>
        /// This method is used for hosting a game
        /// </summary>
        public void HostGame()
        {
            // First check if a serverName has been provided
            if (serverNameInput != null && !String.IsNullOrEmpty(serverNameInput.text))
            {
                // Set the server name
                this.name = serverNameInput.text;

                // Start hosting and then advertise this server for anyone looking for servers
                this.StartHost();
                GetComponent<BukNetworkDiscovery>().AdvertiseServer();
            }
        }

        /// <summary>
        /// This methods is for finding games hosted by other people
        /// </summary>
        public void FindGames()
        {
            var networkDiscovery = GetComponent<BukNetworkDiscovery>();
            // First clear the servers we already knew about, they might have gone offline
            ClearServers();
            // Then start the discovery process in Mirror's NetworkDiscovery
            networkDiscovery.StartDiscovery();
        }

        /// <summary>
        /// This methods removes the server UI elements and clears them from the servers dictionary
        /// </summary>
        private void ClearServers()
        {
            foreach (var row in servers.Values)
            {
                Destroy(row.gameObject);
            }
            servers.Clear();
        }
    }
}