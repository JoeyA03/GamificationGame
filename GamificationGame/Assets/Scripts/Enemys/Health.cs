using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float currentHealth;
    public float maxHealth;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void takeDamage(float d)
    {
        currentHealth -= d;
        DeathCheck();
    }

    private void DeathCheck()
    {
        if (currentHealth <= 0)
        {
            Destroy(this);
        }
    }



}
