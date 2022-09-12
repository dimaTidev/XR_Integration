//Source - https://gist.github.com/demonixis/fc2f9154cd9d87e5f1c6a7a1de2dbb70

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class XR_Detector : MonoBehaviour
{
    [SerializeField] GameObject controller_vr;
    [SerializeField] GameObject controller_pc;

    void Start()
    {
        bool isVR = CheckVRAvaliability();
        controller_vr.SetActive(isVR);
        controller_pc.SetActive(!isVR);
    }


    public bool CheckVRAvaliability()
    {
        var xrSettings = XRGeneralSettings.Instance;
        if (xrSettings == null)
        {
            Debug.Log($"XRGeneralSettings is null.");
            return false;
        }

        var xrManager = xrSettings.Manager;
        if (xrManager == null)
        {
            Debug.Log($"XRManagerSettings is null.");
            return false;
        }

        var xrLoader = xrManager.activeLoader;
        if (xrLoader == null)
        {
            Debug.Log($"XRLoader is null.");
            return false;
        }

        return true;

       // Debug.Log($"Loaded XR Device: {xrLoader.name}");
       //
       // var xrDisplay = xrLoader.GetLoadedSubsystem<XRDisplaySubsystem>();
       // Debug.Log($"XRDisplay: {xrDisplay != null}");
       //
       // if (xrDisplay != null)
       // {
       //     if (xrDisplay.TryGetDisplayRefreshRate(out float refreshRate))
       //     {
       //         Debug.Log($"Refresh Rate: {refreshRate}hz");
       //     }
       // }
       //
       // var xrInput = xrLoader.GetLoadedSubsystem<XRInputSubsystem>();
       // Debug.Log($"XRInput: {xrInput != null}");
       //
       // if (xrInput != null)
       // {
       //     xrInput.TrySetTrackingOriginMode(TrackingOriginModeFlags.Device);
       //     xrInput.TryRecenter();
       // }
       //
       // var xrMesh = xrLoader.GetLoadedSubsystem<XRMeshSubsystem>();
       // Debug.Log($"XRMesh: {xrMesh != null}");
    }
}
