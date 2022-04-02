using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

namespace BUK
{
public class FloatingUsername : MonoBehaviour
    {
        public void SetUsername(string username)
        {
            this.GetComponent<TextMeshPro>().text = username;
        }
    
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
