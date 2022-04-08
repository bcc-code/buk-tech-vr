using System;
using UnityEngine;
using Mirror;
using Buk.Multiplayer;
using System.Collections.Generic;

namespace Buk.Multiplayer
{
    /// <summary>
    /// This class handles all Multiplayer related code that any Player object would need
    /// </summary>
    public class NetworkPlayer : NetworkBehaviour
    {
        /// <summary>
        /// The username of the player, this gets set via BukNetworkManager
        /// On change of this variable, call the OnChangeUsername hook
        /// Because this is a SyncVar, it will be synced across all clients connected to a server
        /// </summary>
        [NonSerialized]
        [SyncVar(hook = "OnChangeUsername")]
        public string username = null;

        /// <summary>
        /// This list defines all Behaviours that should be enabled for a local player but not for remote players
        /// This would include things like a Camera, CharacterController, AudioListener, MouseLook scripts
        /// These values can be set through the inspector
        /// </summary>
        public List<Behaviour> locallyEnabled = new List<Behaviour>();

        /// <summary>
        /// This GameObject is the object that displays the player's username above their head for all to see
        /// This can be null if no such object is desired
        /// </summary>
        public GameObject floatingUsernameObject;

        /// <summary>
        /// Only the server is allowed to set SyncVars, therefore setting the username has to be through a Command
        /// </summary>
        [Command]
        public void CmdSetUsername(string username)
        {
            if (!isServer) return;

            this.username = username;
        }

        public void Start()
        {
            if (!isLocalPlayer)
            {
                // If this player object is not the local player but a remote player, disable all local behaviours
                this.DeactivateLocalPlayer();
            }
            else
            {
                if (String.IsNullOrEmpty(username))
                {
                    // If this is the local player, get its username from the BukNetworkManager and set it through CmdSetUsername
                    var username = ((BukNetworkManager)NetworkManager.singleton).username;
                    CmdSetUsername(username);
                }
                // Then activate all local behaviours
                this.ActivateLocalPlayer();
            }
        }

        /// <summary>
        /// The method for activating all things only relevant to a local player object
        /// </summary>
        protected virtual void ActivateLocalPlayer()
        {
            // Do not show the floating username for the local player
            if (this.floatingUsernameObject != null)
                this.floatingUsernameObject.SetActive(false);

            // Enable all behaviours relevant to the local player
            foreach (var behaviour in locallyEnabled)
            {
                behaviour.enabled = true;
            }
        }

        protected virtual void DeactivateLocalPlayer()
        {
            // Activate the floating username for all non-local player objects
            if (this.floatingUsernameObject != null)
                this.floatingUsernameObject.SetActive(true);

            // Disable all behaviours only relevant to the local player on all non-local player objects
            foreach (var behaviour in locallyEnabled)
            {
                behaviour.enabled = false;
            }
        }

        /// <summary>
        /// The callback for the username SyncVar
        /// This method is used for updating the UI with the Synced username value
        /// </summary>
        private void OnChangeUsername(string oldUsername, string username)
        {
            if (this.floatingUsernameObject != null)
                this.floatingUsernameObject.GetComponent<FloatingUsername>().SetUsername(username);
        }
    }
}