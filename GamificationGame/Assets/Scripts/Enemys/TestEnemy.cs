using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public Health enemyHealth;
    [SerializeField]
    private GameObject playerObject;
    public float attackRange;
    public float attackSpeed;


    private void Awake()
    {
        playerObject = GameObject.Find("Player");   // FIND A BETTER WAY TO DO THIS RATHER/CHANGE TO FIND TAG???
    }

    private void Update()
    {
        //this.transform.position += this.transform.position - playerObject.transform.position;
    }

    public void followPlayer() 
    {

    
    }

    public void Attack() 
    {
        float distanceBtwPlayer = Vector3.Distance(this.transform.position, playerObject.transform.position);
        if(distanceBtwPlayer <= attackRange) 
        {
            ///set attack animations
            ///set attack speed
            ///
        }
    }


}
