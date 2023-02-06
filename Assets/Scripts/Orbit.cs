using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float rotationSpeed;
    public float spinningSpeed;
    public Vector3 spinningDirection = Vector3.up;
    public GameObject pivotObject;
    public float gravitationalConstant = 6.67430f * Mathf.Pow(10, -11f);
    public bool orbitX = true;
    public bool orbitY = true;
    public bool orbitZ = false;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, pivotObject.transform.position);
        Vector3 direction = (pivotObject.transform.position - transform.position).normalized;
        float forceMagnitude = gravitationalConstant * (rb.mass * pivotObject.GetComponent<Rigidbody>().mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction * forceMagnitude;

        rb.AddForce(force);
        Vector3 rotationAxis = new Vector3(orbitX ? 1 : 0, orbitY ? 1 : 0, orbitZ ? 1 : 0);
        transform.RotateAround(pivotObject.transform.position, rotationAxis, rotationSpeed * Time.deltaTime);
        transform.Rotate(spinningDirection * spinningSpeed * Time.deltaTime);
    }
}
