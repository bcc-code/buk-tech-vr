using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

namespace Buk.Multiplayer
{
    /// <summary>
    /// A FloatingUsername component for a 3d space Label showing the username
    /// All FloatingUsername components will always locally be pointing towards the main local camera
    /// </summary>
    public class FloatingUsername : MonoBehaviour
    {
        /// <summary>
        /// Set the username UI value
        /// </summary>
        public void SetUsername(string username)
        {
            this.GetComponent<TextMeshPro>().text = username;
        }
    
        /// <summary>
        /// Use LateUpdate for pointing the username object toward the player's camera after any camera movements
        /// </summary>
        void LateUpdate()
        {
            if (Camera.current != null)
            {
                transform.LookAt(Camera.current.transform);
                transform.rotation = Quaternion.LookRotation(Camera.current.transform.forward);
            }
        }
    }
}
