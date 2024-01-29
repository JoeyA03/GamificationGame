using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingSprite : MonoBehaviour
{

    public float bobbingSpeed = 1f; // Speed of the bobbing effect
    public float bobbingAmount = 0.5f; // Amount of bobbing effect
    public float midpoint = 2f; // Midpoint of the bobbing

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Compute the new Y position using a sine wave
        float newY = startPos.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
        
        // Apply the new position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
