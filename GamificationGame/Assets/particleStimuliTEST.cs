using System.Collections.Generic;
using UnityEngine;

public class particleStimuliTEST : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    public float damage;
    public float invincibilitySpeed;

    [SerializeField]
    private List<GameObject> objcetsHit = new List<GameObject>();


    // these lists are used to contain the particles which match
    // the trigger conditions each frame.
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    //List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

    private void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {

        if (!objcetsHit.Contains(other)) 
        {
            objcetsHit.Add(other);
            if (other.TryGetComponent<Health>(out Health enm)) 
            {
                Debug.Log("TAKINGDAMGAGE");
                //enm.takeDamage(damage);
            }

        }
    }

    //private void OnParticleTrigger()
    //{
    //    int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    //    // iterate through the particles which entered the trigger and make them red
    //    for (int i = 0; i < numEnter; i++)
    //    {
    //        ParticleSystem.Particle p = enter[i];
    //        Debug.Log("particleHit");
    //        enter[i] = p;
    //    }

    //    ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    //}
}
