using UnityEngine;

[CreateAssetMenu(menuName = "GamificationGame/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float baseSpeed;
    public float runningSpeed;
    public float dodgeDistance;
    public float weight;
    public float defence;
    public float stamina;
}