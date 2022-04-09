using System.Net;
using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using Mirror.Discovery;
using Buk.Multiplayer;

/*
    Documentation: https://mirror-networking.gitbook.io/docs/components/network-discovery
    API Reference: https://mirror-networking.com/docs/api/Mirror.Discovery.NetworkDiscovery.html
*/

namespace Buk.Multiplayer
{
    /// <summary>
    /// When a client wants to find servers, it sends this DiscoveryRequest to every server
    /// You can put whatever you want in here, for example the username of the player joining
    /// For our case this is not needed
    /// </summary>
    public class DiscoveryRequest : NetworkMessage
    {
        // Add properties for whatever information you want sent by clients
        // in their broadcast messages that servers will consume.
    }
    
    /// <summary>
    /// When a server tells a client about its existence, it can send the DiscoveryResponse to the client
    /// This response can contain info like the name of the server, how many players have joined, etc...
    /// </summary>
    public class DiscoveryResponse : NetworkMessage
    {
        /// <summary>
        /// The uri of the server, usually something like kcp://192.168.xxx.xxx
        /// </summary>
        public Uri uri;

        /// <summary>
        /// The endpoint of the server, the uri can also contain names like kcp://servername
        /// The endpoint however always contains info about the server's IP address
        /// </summary>
        public IPEndPoint endpoint { get; set; }

        /// <summary>
        /// A unique server ID to be able to differentiate between servers
        /// The IP could also be used as a differentiator as it should be unique per server
        /// </summary>
        public long serverId;

        /// <summary>
        /// The name of the server
        /// </summary>
        public string serverName;

        /// <summary>
        /// The name of the scene the server is running on
        /// This is not used as of right now
        /// </summary>
        public string sceneName;

        /// <summary>
        /// The amount of players currently in the server
        /// </summary>
        public int totalPlayers;
    
        /// <summary>
        /// This methods defines how it should be determined whether a DiscoveryResponse is the same as another
        /// This is done based on the uri, as the uri should be unique for the server
        /// </summary>
        public override bool Equals(object obj)
        {
            var item = obj as DiscoveryResponse;
    
            if (item == null)
                return false;
    
            return this.uri.Equals(item.uri);
        }
    
        /// <summary>
        /// The GetHashCode, we just return the hash code of the uri
        /// </summary>
        public override int GetHashCode()
        {
            return this.uri.GetHashCode();
        }
    }
    
    [Serializable]
    public class ServerFoundUnityEvent : UnityEvent<DiscoveryResponse> {};
    
    /// <summary>
    /// We extend Mirror's NetworkDiscoveryBase to add our custom functionality to the network discovery system
    /// We will use this to populate the values in our own DiscoveryResponse
    /// </summary>
    public class BukNetworkDiscovery : NetworkDiscoveryBase<DiscoveryRequest, DiscoveryResponse>
    {
        #region Server
    
        public long ServerId { get; private set; }
    
        [Tooltip("Transport to be advertised during discovery")]
        public Transport transport;
    
        /// <summary>
        /// This unity event can be used to assign callbacks and run functions when the BukNetworkDiscovery finds a server
        /// It provides the DiscoveryResponse to whatever function will handle this response
        /// </summary>
        [Tooltip("Invoked when a server is found")]
        public ServerFoundUnityEvent OnServerFound;
    
        public override void Start()
        {
            // On start we set the ID our server will use if we host one to a random long number
            ServerId = RandomLong();
    
            // The transport we use for discovery is already initialized in Awake
            // Therefore we just need to set our transport variable here
            if (transport == null)
                transport = Transport.activeTransport;
    
            base.Start();
        }
    
        /// <summary>
        /// Process the request from a client
        /// </summary>
        /// <remarks>
        /// Override if you wish to provide more information to the clients
        /// such as the name of the host player
        /// </remarks>
        /// <param name="request">Request coming from client</param>
        /// <param name="endpoint">Address of the client that sent the request</param>
        /// <returns>A message containing information about this server</returns>
        protected override DiscoveryResponse ProcessRequest(DiscoveryRequest request, IPEndPoint endpoint) 
        {
            // We used our own network manager BukNetworkManager to populate some DiscoveryResponse values
            // When we type in a server name and select a scene, that will be set in the BukNetworkManager, which we read here
            var networkManager = (BukNetworkManager) GetComponent<NetworkManager>();
            try
            {
                // Populate the DiscoveryResponse values to send to clients searching for servers
                return new DiscoveryResponse
                {
                    serverId = ServerId,
                    uri = transport.ServerUri(),
                    serverName = networkManager.name,
                    sceneName = networkManager.onlineScene,
                    totalPlayers = NetworkServer.connections.Count
                };
            }
            catch (NotImplementedException)
            {
                Debug.LogError($"Transport {transport} does not support network discovery");
                throw;
            }
        }
    
        #endregion
    
        #region Client
    
        /// <summary>
        /// Create a message that will be broadcasted on the network to discover servers
        /// </summary>
        /// <remarks>
        /// Override if you wish to include additional data in the discovery message
        /// such as desired game mode, language, difficulty, etc... </remarks>
        /// <returns>An instance of ServerRequest with data to be broadcasted</returns>
        protected override DiscoveryRequest GetRequest()
        {
            return new DiscoveryRequest();
        }
    
        /// <summary>
        /// Process the answer from a server
        /// </summary>
        /// <remarks>
        /// A client receives a reply from a server, this method processes the
        /// reply and raises an event
        /// </remarks>
        /// <param name="response">Response that came from the server</param>
        /// <param name="endpoint">Address of the server that replied</param>
        protected override void ProcessResponse(DiscoveryResponse response, IPEndPoint endpoint) {
            // we received a message from the remote endpoint
            response.endpoint = endpoint;
    
            // although we got a supposedly valid url, we may not be able to resolve
            // the provided host
            // However we know the real ip address of the server because we just
            // received a packet from it,  so use that as host.
            UriBuilder realUri = new UriBuilder(response.uri)
            {
                Host = response.endpoint.Address.ToString()
            };
            response.uri = realUri.Uri;
    
            // Now we invoke the OnServerFound event with the DiscoveryReponse
            // Any event listener we set in the inspector can now handle this response
            OnServerFound.Invoke(response);
        }
    
        #endregion
    }
}