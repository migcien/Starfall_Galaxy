using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShip : MonoBehaviour
{
    public GameObject player;
    public float followDistance = 3.0f;
    public float followHeight = 2.0f;
    public float rotationSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 followPos = new Vector3(playerPos.x, playerPos.y + followHeight, playerPos.z - followDistance);
        transform.position = Vector3.Lerp(transform.position, followPos, Time.deltaTime);

        Vector3 targetDirection = player.transform.forward;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

}