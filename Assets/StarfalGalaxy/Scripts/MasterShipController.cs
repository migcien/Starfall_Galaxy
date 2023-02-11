using UnityEngine;

public class MasterShipController : MonoBehaviour
{
    public float forwardSpeed = 10f; // speed of forward movement
    public float horizontalSpeed = 5f; // speed of strafing movement
    public float mouseSensitivity = 10f; // sensitivity of mouse rotation

    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float horizontal = 0f; // horizontal input
        float vertical = 0f; // vertical input

        // get input for forward/backward movement
        if (Input.GetKey(KeyCode.W))
        {
            vertical = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            vertical = -1f;
        }

        // get input for strafing movement
        if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1f;
        }

        float mouseX = Input.GetAxis("Mouse X");
        float yRotation = transform.eulerAngles.y + mouseX * mouseSensitivity;
        transform.eulerAngles = new Vector3(0, yRotation, 0);

        // calculate the final direction
        vertical *= forwardSpeed;
        Vector3 direction = new Vector3(-vertical, 0, horizontal * horizontalSpeed);

        // set the velocity of the rigidbody
        rigidBody.velocity = direction;
    }
}
