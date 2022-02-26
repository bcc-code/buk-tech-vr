using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace BUK.Players
{
    public interface IPlayer
    {
        string pid { get; set; }

        string username { get; set; }

        void UpdatePosition(Vector3 position);

        void UpdateRotation(Vector3 angles);

        [Command]
        void CmdSetActive(bool active);

        [ClientRpc]
        void RpcSetActive(bool active);
    }
}