using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Socket_slotedVisulizer : MonoBehaviour
{
    [SerializeField] Socket_sloted socket;
    [SerializeField] Data[] data;

    [System.Serializable] public class UnityEvent_bool : UnityEvent<bool> { }

    [System.Serializable]
    public class Data
    {
        [SerializeField] UnityEvent onOccupated;
        [SerializeField] UnityEvent onEmpty;
       // [SerializeField] UnityEvent onStartWork;
       // [SerializeField] UnityEvent onStopWork;
       // bool isOccupated;

        public void SetEnable(bool isOccupated)
        {
          //  this.isOccupated = isOccupated;
            if (isOccupated)
                onOccupated?.Invoke();
            else
                onEmpty?.Invoke();
        }

      //  public void SetWork(bool isWork)
      //  {
      //      if (isWork && !isOccupated)
      //          onStartWork?.Invoke();
      //      else
      //          onStopWork?.Invoke();
      //  }
    }


    void Awake()
    {
        if (socket)
            socket.onSlotsChanged += OnSlotsStateChanged;
    }

  // public void OnStartWork(bool isWork)
  // {
  //     foreach (var item in data)
  //         item.SetWork(isWork);
  // }

    void OnSlotsStateChanged(bool[] states)
    {
        for (int i = 0; i < states.Length && i < data.Length; i++)
            data[i].SetEnable(states[i]);
    }
}
