using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New params", menuName = "KnockParametrs")]
public class DefaultKnock : ScriptableObject
{
    public float Damage;
    public float WaitTime;
    public float KnockTime;
    public float Thrust;
}
