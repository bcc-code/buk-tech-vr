using Mirror;
using Mirror.Discovery;
using System.Collections.Generic;
using System;
using UnityEngine;
using BUK.Players;
using TMPro;

namespace BUK.Multiplayer
{
    public class BioNetworkManager : NetworkManager
    {
        public GameObject serverDiscoveryRow;
        public Transform menuServers;
        public TMP_InputField serverNameInput;

        public TMP_InputField usernameInput;

        public GameObject lobbyUi;
        public GameObject loginUi;

        public string name;
        public string username;

        private Dictionary<DiscoveryResponse, ServerRow> servers = new Dictionary<DiscoveryResponse, ServerRow>();

        public GameObject ReplacePlayer(NetworkConnection conn, GameObject newPlayer, bool destroyOld, bool instantiateNew)
        {
            GameObject oldPlayer = conn.identity.gameObject;

            NetworkServer.ReplacePlayerForConnection(conn, instantiateNew ? Instantiate(newPlayer) : newPlayer, true);

            if (destroyOld)
                NetworkServer.Destroy(oldPlayer);

            return oldPlayer;
        }

        public void SetPlayerUsername()
        {
            if (usernameInput != null && !String.IsNullOrEmpty(usernameInput.text))
            {
                this.username = usernameInput.text;
                this.loginUi.SetActive(false);
                this.lobbyUi.SetActive(true);
            }
        }

        public void ClearUsername()
        {
            this.username = null;
            this.lobbyUi.SetActive(false);
            this.loginUi.SetActive(true);
        }
        
        public void OnDiscoveredServer(DiscoveryResponse response)
        {
            if (serverDiscoveryRow != null && menuServers != null)
            {
                if (!servers.ContainsKey(response))
                {
                    var newRow = Instantiate(serverDiscoveryRow, menuServers).GetComponent<ServerRow>();
                    servers.Add(response, newRow);
                    newRow.SetValues(response);
                }
                else
                {
                    servers[response].SetValues(response);
                }
            }
        }

        public void BeginHost()
        {
            if (serverNameInput != null && !String.IsNullOrEmpty(serverNameInput.text))
            {
                this.name = serverNameInput.text;

                this.StartHost();
                GetComponent<BioNetworkDiscovery>().AdvertiseServer();
            }
        }

        public void FindGames()
        {
            var networkDiscovery = GetComponent<BioNetworkDiscovery>();
            ClearServers();
            networkDiscovery.StartDiscovery();
        }

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