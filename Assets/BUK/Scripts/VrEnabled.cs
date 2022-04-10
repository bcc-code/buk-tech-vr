using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;

namespace Buk
{
    public class VrEnabled : MonoBehaviour
    {
        public bool isVREnabled;
    
        void Start()
        {
            if (isVREnabled)
                StartCoroutine(StartXRCoroutine());
            else
                StopXR();
        }
    
        public IEnumerator StartXRCoroutine()
        {
            Debug.Log("Initializing XR...");
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
    
            if (XRGeneralSettings.Instance.Manager.activeLoader != null)
            {
                Debug.Log("Starting XR...");
                XRGeneralSettings.Instance.Manager.StartSubsystems();
            }
        }
    
        void StopXR()
        {
            if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
            {
                Debug.Log("Stopping XR...");
        
                XRGeneralSettings.Instance.Manager.StopSubsystems();
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();

                Camera.main.ResetAspect();
                Debug.Log("XR stopped completely.");
            }
        }
    }
}
