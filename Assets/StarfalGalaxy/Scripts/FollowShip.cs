using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShip : MonoBehaviour
{
    public GameObject player;
    public float followDistance = 3.0f;
    public float followHeight = 2.0f;
    public float rotationSpeed = 5.0f;
    public float minDistance = 1.0f;
    public float raycastRange = 5.0f;
    public float avoidanceRange = 10.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 followPos = new Vector3(playerPos.x, playerPos.y + followHeight, playerPos.z - followDistance);
        float distance = Vector3.Distance(followPos, transform.position);

        if (distance < minDistance)
        {
            followPos = transform.position + (followPos - transform.position).normalized * minDistance;
        }

        Vector3 direction = followPos - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, raycastRange))
        {
            if (hit.collider != null)
            {
                followPos = hit.point - direction.normalized * (minDistance + avoidanceRange);
            }
        }

        transform.position = Vector3.Lerp(transform.position, followPos, Time.deltaTime);

        Vector3 targetDirection = player.transform.forward;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}