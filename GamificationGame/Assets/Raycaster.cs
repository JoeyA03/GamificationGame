using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public GameObject player;
    private List<GameObject> stuffWeHit;

    void Start()
    {
        stuffWeHit = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Get the direction towards the player
            Vector3 direction = player.transform.position - transform.position;

            // Perform a raycast towards the player
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit))
            {
                // Check if the raycast hits the player
                if (hit.collider.gameObject == player)
                {
                    /*if(stuffWeHit.Count > 0)
                    {
                        for(int i = 0; i <= stuffWeHit.Count; i++)
                        {
                            stuffWeHit[i].SetActive(true);
                        }
                    }*/
                }
                if(hit.collider.CompareTag("Wall"))
                {
                    stuffWeHit.Add(hit.collider.gameObject);
                    hit.collider.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogWarning("Player reference is null.");
        }
    }
}
