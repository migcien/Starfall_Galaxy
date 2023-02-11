using UnityEngine;

public class MasterShipController : MonoBehaviour
{
    public float forwardSpeed = 10f; // speed of forward movement
    public float horizontalSpeed = 5f; // speed of strafing movement
    public float mouseSensitivity = 10f; // sensitivity of mouse rotation
    public float orbitSpeed = 2f; // speed of orbiting movement

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

        // calculate the direction to the mouse position in world space
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, transform.position.z - Camera.main.transform.position.z));
        Vector3 directionToMouse = mousePosition - transform.position;

        // calculate the final direction
        Vector3 forwardDirection = -transform.right * forwardSpeed * vertical;
        Vector3 orbitDirection = Quaternion.AngleAxis(90, transform.up) * directionToMouse.normalized * orbitSpeed * horizontal;
        Vector3 strafeDirection = transform.forward * horizontalSpeed * horizontal;
        Vector3 direction = forwardDirection + orbitDirection + strafeDirection;

        // set the velocity of the rigidbody
        rigidBody.velocity = direction;
    }
}
