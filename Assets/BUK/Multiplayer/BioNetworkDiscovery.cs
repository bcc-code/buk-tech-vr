using System.Net;
using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using Mirror.Discovery;
using BUK.Multiplayer;

/*
    Documentation: https://mirror-networking.gitbook.io/docs/components/network-discovery
    API Reference: https://mirror-networking.com/docs/api/Mirror.Discovery.NetworkDiscovery.html
*/

public class DiscoveryRequest : NetworkMessage
{
    // Add properties for whatever information you want sent by clients
    // in their broadcast messages that servers will consume.
}

public class DiscoveryResponse : NetworkMessage
{
    // Add properties for whatever information you want the server to return to
    // clients for them to display or consume for establishing a connection.
    public Uri uri;
    public IPEndPoint endpoint { get; set; }
    public long serverId;
    public string serverName;
    public string sceneName;
    public int totalPlayers;

    public override bool Equals(object obj)
    {
        var item = obj as DiscoveryResponse;

        if (item == null)
            return false;

        return this.uri.Equals(item.uri);
    }

    public override int GetHashCode()
    {
        return this.uri.GetHashCode();
    }
}

[Serializable]
public class ServerFoundUnityEvent : UnityEvent<DiscoveryResponse> {};

public class BioNetworkDiscovery : NetworkDiscoveryBase<DiscoveryRequest, DiscoveryResponse>
{
    #region Server

    public long ServerId { get; private set; }

    [Tooltip("Transport to be advertised during discovery")]
    public Transport transport;

    [Tooltip("Invoked when a server is found")]
    public ServerFoundUnityEvent OnServerFound;

    public override void Start()
    {
        ServerId = RandomLong();

        // active transport gets initialized in awake
        // so make sure we set it here in Start()  (after awakes)
        // Or just let the user assign it in the inspector
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
        var networkManager = (BioNetworkManager) GetComponent<NetworkManager>();
        try
        {
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

        OnServerFound.Invoke(response);
    }

    #endregion
}
