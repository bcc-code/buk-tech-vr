using System;
using Buk.PhysicsLogic.Interfaces;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Buk.Multiplayer
{
  public class MultiplayerGunController : NetworkBehaviour
  {
    public GameObject gunObject;
    public InputAction trigger;

    /// <summary>
    /// The code that should run immediately after this object is loaded
    /// </summary>
    public void Awake()
    {
      // If the gunObject is not set or it does not have an IGun, then give feedback by way of an exception
      if (gunObject == null || gunObject.GetComponent<IGun>() == null)
      {
        throw new Exception("The multiplayer gun script requires a Gun Object with an IGun component");
      }

      if (trigger != null)
      {
        // Add a callback to the trigger input and enable the input action
        trigger.performed += TriggerPressed;
        trigger.Enable();
      }
    }

    /// <summary>
    /// When this object is destroyed, stop listening to inputs
    /// </summary>
    public void OnDestroy()
    {
      if (trigger != null)
      {
        trigger.performed -= TriggerPressed;
        trigger.Disable();
      }
    }

    // This is what the server communication looks like for this controller
    // This communication is done with Remote Actions, read more here
    // https://mirror-networking.gitbook.io/docs/guides/communications/remote-actions
    /*
                   ┌───────────────────────┐
                   │Server                 │
                   │ ┌────────┐ ┌────────┐ │
            ┌──────┼─┤Server  | |Server  | │
            │  ┌───┼─►Player 1| |Player 2| │
            │  │   │ └───────┬┘ └────────┘ │
            │  │   └─────────┼─────────────┘
            │  │             │
    RpcShoot│  │CmdShoot     └──────────┐RpcShoot
            │  │                        │
     ┌──────┼──┼─────────────┐   ┌──────┼────────────────┐
     │Client│1 │             │   │Client|2               │
     │ ┌────▼──┴┐ ┌────────┐ │   │ ┌────▼───┐ ┌────────┐ │
     │ │Client  │ │Remote  │ │   │ │Remote  │ |Client  │ │
     │ │Player 1│ │Player 2│ │   │ │Player 1│ |Player 2│ │
     │ └────────┘ └────────┘ │   │ └────────┘ └────────┘ │
     └───────────────────────┘   └───────────────────────┘
    */

    /// <summary>
    /// This function only gets run on the server.
    /// The server can tell all instances of this player to all fire a bullet
    /// </summary>
    [Command]
    public void CmdShoot()
    {
      if (!isServer) return;

      // Call the function to tell all instances of this player to shoot a bullet
      RpcShoot();
    }

    /// <summary>
    /// This function gets run on all instances of a player at the same time
    /// This is the code that actually spawns and fires the bullet for that player on every other player's, and its own, screen
    /// </summary>
    [ClientRpc]
    public void RpcShoot()
    {
      if (gunObject == null) return;

      // Get the gun component from the gunObject
      var gun = gunObject.GetComponent<IGun>();

      // If the gun component exits, shoot a bullet from the gun
      if (gun != null)
        // TODO: Fix velocity
        gun.Shoot(5.0f);
    }

    /// <summary>
    /// This function gets called when a local player presses the shoot input.
    /// This should only get called by the local player if this script is properly disabled on remote players.
    /// </summary>
    private void TriggerPressed(InputAction.CallbackContext _)
    {
      if (isLocalPlayer)
        // Tell the server that we are shooting a gun
        CmdShoot();
    }
  }
}
