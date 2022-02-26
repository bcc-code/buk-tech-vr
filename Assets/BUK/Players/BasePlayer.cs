using System;
using UnityEngine;
using Mirror;
using System.Collections.Generic;

namespace BUK.Players
{
    public class BasePlayer : NetworkBehaviour
    {
        [SyncVar]
        public string _username = null;
        [SyncVar]
        public string _pid = null;

        public string username
        {
            get => _username;
            set => _username = value;
        }

        public string pid
        {
            get => _pid;
            set => _pid = value;
        }

        public List<Behaviour> locallyEnabled = new List<Behaviour>();

        void Update()
        {
        }

        [Command]
        public void CmdSetUsername(string username)
        {
            this.username = username;
        }

        public override void OnStartServer()
        {
            if (String.IsNullOrEmpty(pid))
            {
                this.pid = Guid.NewGuid().ToString();
            }
            this.DeactivateLocalPlayer();
        }
    
        public override void OnStartLocalPlayer()
        {
            if (String.IsNullOrEmpty(username))
            {
                CmdSetUsername("Johnson-" + netId);
            }
            this.ActivateLocalPlayer();
        }
    
        public string GetUsername()
        {
            return this.username;
        }

        public string GetPid()
        {
            return this.pid;
        }
    
        public void UpdatePosition(Vector3 position)
        {
            this.transform.position = position;
        }

        public void UpdateRotation(Vector3 angles)
        {
            transform.localEulerAngles = angles;
        }

        [Command]
        public void CmdSetActive(bool active)
        {
            RpcSetActive(active);
        }

        [ClientRpc]
        public void RpcSetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public virtual void ActivateLocalPlayer()
        {
            foreach (var behaviour in locallyEnabled)
            {
                behaviour.enabled = true;
            }
        }

        public virtual void DeactivateLocalPlayer()
        {
            foreach (var behaviour in locallyEnabled)
            {
                behaviour.enabled = false;
            }
        }
    }
}