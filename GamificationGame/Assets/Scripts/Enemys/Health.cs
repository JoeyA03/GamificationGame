using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float currentHealth;
    public float maxHealth;

    public bool isHit;
    public float hitStun;
    private float hitStunCountdown;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        HitStunCounter();
    }

    private void HitStunCounter()
    {
        if (isHit)
        {
            hitStunCountdown += Time.deltaTime;
            if (hitStunCountdown >= hitStun)
            {
                isHit = false;
                hitStunCountdown = 0;
            }
        }
    }

    public void takeDamage(float d)
    {
        currentHealth -= d;
        isHit = true;

        DeathCheck();
    }

    private void DeathCheck()
    {
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }



}
