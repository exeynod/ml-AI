using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{

    public Signal Signal;
    public UnityEvent SignalEvent;
    public void OnSignalRaised()
    {
        SignalEvent.Invoke();
    }

    private void OnEnable()
    {
        Signal.RegisterListener(this);
    }

    private void OnDisable()
    {
        Signal.DeRegisterListener(this);
    }
}

