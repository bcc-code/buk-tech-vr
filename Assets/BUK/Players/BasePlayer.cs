using System;
using UnityEngine;
using Mirror;
using BUK.Multiplayer;
using System.Collections.Generic;

namespace BUK.Players
{
    public class BasePlayer : NetworkBehaviour
    {
        [SyncVar(hook = "OnChangeUsername")]
        public string username = null;
        [SyncVar]
        public string pid = null;

        public List<Behaviour> locallyEnabled = new List<Behaviour>();

        public GameObject usernameObject;

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
                var username = ((BioNetworkManager) NetworkManager.singleton).username;
                CmdSetUsername(username);
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

        protected virtual void ActivateLocalPlayer()
        {
            this.usernameObject.SetActive(false);
            foreach (var behaviour in locallyEnabled)
            {
                behaviour.enabled = true;
            }
        }

        protected virtual void DeactivateLocalPlayer()
        {
            this.usernameObject.SetActive(true);
            foreach (var behaviour in locallyEnabled)
            {
                behaviour.enabled = false;
            }
        }

        private void OnChangeUsername(string oldUsername, string username)
        {
            this.usernameObject.GetComponent<FloatingUsername>().SetUsername(username);
        }
    }
}