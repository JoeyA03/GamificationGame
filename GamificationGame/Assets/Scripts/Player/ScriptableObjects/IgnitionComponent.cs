using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GamificationGame/IgnitionComponent")]
public class IgnitionComponent : ScriptableObject
{
    public float damage;
    public float overheatRate;
    public float weight;
    public float fuelConsumption;
}

