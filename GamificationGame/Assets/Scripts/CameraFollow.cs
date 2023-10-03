using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 5.0f;
    public float delayInSeconds = 1.0f;

    private Vector3 offset;

    private bool isFollowing = false;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void Update()
    {
        // Calculate the target position based on the player's position and the offset.
        Vector3 targetPosition = player.position + offset;

        // If the player moves, start or reset the delay timer.
        if (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            if (!isFollowing)
            {
                isFollowing = true;
                StartCoroutine(DelayedFollow(targetPosition));
            }
        }
    }

    
    IEnumerator DelayedFollow(Vector3 targetPosition)
    {
        yield return new WaitForSeconds(delayInSeconds);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            // Use Lerp to smoothly interpolate between the current position and the target position.
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            targetPosition = player.position + offset;
            yield return null;
        }

        isFollowing = false; // Reset the flag when the camera has caught up.
    }
}
