using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseTrigger : MonoBehaviour
{   
    public GameObject attachedSegment;
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("left");
            attachedSegment.SetActive(true);
        }
    }
}
