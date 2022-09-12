using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace WebXR.Interactions
{
    public class WebXR_Input : MonoBehaviour
    {
        [SerializeField] WebXRController controller;

        [SerializeField] UnityEvent_bool onControllerVisible;
        [SerializeField] UnityEvent_bool onHandsActive;
        [SerializeField] UnityEvent onTriggerDown;
        [SerializeField] UnityEvent onTriggerUp;
        [SerializeField] UnityEvent onGripDown;
        [SerializeField] UnityEvent onGripUp;
        [SerializeField] UnityEvent_float onGripValue;
        [SerializeField] UnityEvent_float onTriggerValue;

        [SerializeField] UnityEvent_vector3 onStickMove;
       // [SerializeField] UnityEvent_float onStickAxisX;

        [System.Serializable] public class UnityEvent_bool : UnityEvent<bool> { }
        [System.Serializable] public class UnityEvent_float : UnityEvent<float> { }
        [System.Serializable] public class UnityEvent_vector3 : UnityEvent<Vector3> { }

        bool 
            isControllerVisible,
            isHandsActive;


        private void Awake()
        {
            if (!controller)
            {
                enabled = false;
                Debug.LogWarning($"{typeof(WebXRController).Name} is null. Pleace fill variable controller at script {typeof(WebXR_Input).Name}, on object: {name}");
                return;
            }

            OnControllerVisible(false);
            OnHandsActive(false);
        }

        private void OnEnable()
        {
            if (controller.isHandActive || controller.isControllerActive)
                OnControllerVisible(true);

            controller.OnControllerActive += OnControllerVisible;
            controller.OnHandActive += OnHandsActive;
        }

        private void OnDisable()
        {
            controller.OnControllerActive -= OnControllerVisible;
            controller.OnHandActive -= OnHandsActive;
        }

        private void Update()
        {
            if (!isControllerVisible && !isHandsActive)
                return;

            if (controller.GetButtonDown(WebXRController.ButtonTypes.Trigger))
                onTriggerDown?.Invoke();

            if (controller.GetButtonUp(WebXRController.ButtonTypes.Trigger))
                onTriggerUp?.Invoke();

            if (controller.GetButtonDown(WebXRController.ButtonTypes.Grip))
                onGripDown?.Invoke();

            if (controller.GetButtonUp(WebXRController.ButtonTypes.Grip))
                onGripUp?.Invoke();

            onGripValue?.Invoke(controller.GetAxis(WebXRController.AxisTypes.Grip));
            onTriggerValue?.Invoke(controller.GetAxis(WebXRController.AxisTypes.Trigger));


            onStickMove?.Invoke(new Vector3(controller.GetAxisIndexValue(2), 0, controller.GetAxisIndexValue(3)));
            //onStickAxisX?.Invoke(controller.GetAxisIndexValue(2));
        }

        void OnControllerVisible(bool isVisible)
        {
            isControllerVisible = isVisible;
            onControllerVisible.Invoke(isVisible);
        }

        void OnHandsActive(bool isActive)
        {
            isHandsActive = isActive;
            onHandsActive.Invoke(isActive);
        }
    }
}
