using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WebXR.Interactions
{
    public class WebXR_Haptics : MonoBehaviour
    {
        [SerializeField] WebXRController controller;
        [SerializeField] float intensity = 0.5f;
        [SerializeField] int durationMilliseconds = 250;

        public void Send_Haptic() => controller?.Pulse(intensity, durationMilliseconds);
    }
}
