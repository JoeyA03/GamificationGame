using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CanisterComponent", menuName = "GamificationGame/CanisterComponent", order = 0)]
public class CanisterComponent : ScriptableObject 
{
    public float fuelAmount;
    public float overheatAmount;
    public float overheatDecreaseRate;
    public float overheatThreshold;
    public float coolingDecrease;
    public float coolingIncrease;
    public float weight;
    public float maxFuelTax;

    
}
