using UnityEngine;
using System.Collections.Generic;

public class PathfindingNode : MonoBehaviour
{
    public List<string> validTags;

    private void Start()
    {
        // This script only provides positional information to the EnemyBehavior script
    }

    private void Update()
    {
        // This script only provides positional information to the EnemyBehavior script
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (validTags.Contains(other.GetComponent<EnemyBehavior>().targetNode.tag))
            {
                other.GetComponent<EnemyBehavior>().targetNode = gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // do nothing
        }
    }
}
