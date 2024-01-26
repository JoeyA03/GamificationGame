using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanisterPickup : MonoBehaviour
{
    public int canisterWorth = 1;
   private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (other != null )
            {
                // Attempt to get the script component from the collider's GameObject
                Player script = other.GetComponent<Player>();

                    // Check if the script component was found
                if (script != null)
                {
                    script.UpdateCanister(canisterWorth);
                    Destroy(gameObject);
                }
            }
            else
            {
                    // Handle the case where the collider is null
                Debug.LogWarning("Collider is null.");
            }
        }
        
    }
}
