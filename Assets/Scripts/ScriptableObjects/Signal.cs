﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Signal : ScriptableObject
{
    public List<SignalListener> Listeners = new List<SignalListener>();

    public void Raise()
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            Listeners[i].OnSignalRaised();
        }
    }
    public void RegisterListener(SignalListener Listener)
    {
        Listeners.Add(Listener);
    }

    public void DeRegisterListener(SignalListener Listener)
    {
        Listeners.Remove(Listener);
    }
}
