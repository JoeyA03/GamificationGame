using UnityEngine;

[CreateAssetMenu(menuName = "GamificationGame/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    //Knowns 

    public float baseSpeed;
    public float runningSpeed;
    public float dodgeDistance;
    public float weight;
    public float defence;
    public float stamina;
    public float maxHealth;
    //public float health;

    [Space(10)]
    //Unknowns
    public float dodgeSpeed;
    public float dodgeDuration;
    public float dodgeCooldown;
    public float dodgeStaminaCost;
    public float meleeSpeed;
    public float meleeStaminaCost;
}