using System.Collections;
using UnityEngine;

public class FollowShip : MonoBehaviour
{
    public GameObject player;  // Reference to the player game object/prefab to follow
    public float followDistance = 3.0f;  // Distance from player game object/prefab to follow at
    public float followHeight = 2.0f;  // Height from player game object/prefab to follow at
    public float rotationSpeed = 5.0f;  // Speed at which the ship will rotate towards the player game object/prefab
    public float minDistance = 1.0f;  // Minimum distance to keep from player game object/prefab
    public float raycastRange = 5.0f;  // Range of the raycast used to detect obstacles
    public float avoidanceRange = 10.0f;  // Avoidance range to keep from obstacles

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        // Get player position
        Vector3 playerPos = player.transform.position;
        // Calculate follow position based on player position, follow distance and follow height
        Vector3 followPos = new Vector3(playerPos.x, playerPos.y + followHeight, playerPos.z - followDistance);
        float distance = Vector3.Distance(followPos, transform.position);

        // Check if the distance between the follow ship and the player game object/prefab is less than the minimum distance
        if (distance < minDistance)
        {
            // If so, adjust the follow position to be the minimum distance away from the player game object/prefab
            followPos = transform.position + (followPos - transform.position).normalized * minDistance;
        }

        // Get direction from follow ship to follow position
        Vector3 direction = followPos - transform.position;
        // Raycast to detect obstacles
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, raycastRange))
        {
            // Check if a collider was hit
            if (hit.collider != null)
            {
                // If so, adjust the follow position to be the avoidance range away from the obstacle
                followPos = hit.point - direction.normalized * (minDistance + avoidanceRange);
            }
        }

        // Update the position of the follow ship to match the follow position
        transform.position = Vector3.Lerp(transform.position, followPos, Time.deltaTime);

        // Get the forward direction of the player game object/prefab
        Vector3 targetDirection = player.transform.forward;
        // Get the rotation towards the player game object/prefab's forward direction
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Update the rotation of the follow ship towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
