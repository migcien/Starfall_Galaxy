using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public float moveSpeed = 10;
    public float rollPower = 60;
    public float yawPower = 60;
    public float pitchPower = 60;
    Rigidbody rbd;

    void Awake()
    {
        rbd = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Turn();
        MoveForward();
    }

    void Turn()
    {
        float roll;
        float pitch;
        float yaw;

        roll = Input.GetAxis("Roll") * rollPower * Time.deltaTime;
        pitch = Input.GetAxis("Pitch") * pitchPower * Time.deltaTime;
        yaw = Input.GetAxis("Yaw") * yawPower * Time.deltaTime;
        transform.Rotate(pitch, yaw, roll);
    }

    void MoveForward()
    {
        //transform.position += transform.forward * moveSpeed * Input.GetAxis("Roll") * Time.deltaTime;
        transform.position += new Vector3(0.0f, 1.0f, 0.0f) * moveSpeed * Input.GetAxis("Thrust") * Time.deltaTime;
        transform.position += transform.forward * moveSpeed * Input.GetAxis("Yaw") * Time.deltaTime;
    }
}
