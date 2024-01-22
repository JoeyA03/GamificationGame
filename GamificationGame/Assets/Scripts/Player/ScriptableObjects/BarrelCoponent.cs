using UnityEngine;

[CreateAssetMenu(fileName = "BarrelCoponent", menuName = "GamificationGame/BarrelCoponent", order = 0)]
public class BarrelCoponent : ScriptableObject 
{
    public enum barrelType
    {
        Straight,
        Spread,
        Special,
        None
    };

    public barrelType flameShape;
    public float tickDamageSpeed;
    public float fuelConsumption;
    public float weight;


    
}

