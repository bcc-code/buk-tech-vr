using Mirror;
using UnityEngine;
using BUK.Players;

namespace BUK.Multiplayer
{
    public class BioNetworkManager : NetworkManager
    {
        public GameObject ReplacePlayer(NetworkConnection conn, GameObject newPlayer, bool destroyOld, bool instantiateNew)
        {
            GameObject oldPlayer = conn.identity.gameObject;

            NetworkServer.ReplacePlayerForConnection(conn, instantiateNew ? Instantiate(newPlayer) : newPlayer, true);

            if (destroyOld)
                NetworkServer.Destroy(oldPlayer);

            return oldPlayer;
        }
    }
}