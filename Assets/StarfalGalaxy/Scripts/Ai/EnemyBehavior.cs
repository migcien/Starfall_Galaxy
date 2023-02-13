using UnityEngine;
using System.Collections.Generic;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 5f;                        // The speed of the enemy
    public float attackRange = 2f;                  // The range at which the enemy will attack its target
    public List<string> nodeTags;                   // The list of tags to use when finding pathfinding nodes

    [HideInInspector]
    public GameObject targetNode;                   // The current target pathfinding node

    private void Start()
    {
        // Set the initial target node
        targetNode = GetClosestNode();
    }

    private void Update()
    {
        // Check if the target node is null
        if (targetNode == null)
        {
            // If the target node is null, find the closest node and set it as the target
            targetNode = GetClosestNode();
            return;
        }

        // Check if the AI is within attack range
        if (Vector3.Distance(transform.position, targetNode.transform.position) <= attackRange)
        {
            // If the AI is within attack range, attack the target node and select the next target
            targetNode = GetClosestNode();
        }
        else
        {
            // If the AI is not within attack range, move towards the target node
            transform.position = Vector3.MoveTowards(transform.position, targetNode.transform.position, speed * Time.deltaTime);
        }

        // Rotate towards the target node
        transform.LookAt(targetNode.transform.position);
    }

    private GameObject GetClosestNode()
    {
        // Create a list of pathfinding nodes with the appropriate tags
        List<GameObject> nodes = new List<GameObject>();
        foreach (string tag in nodeTags)
        {
            GameObject[] tagNodes = GameObject.FindGameObjectsWithTag(tag);
            if (tagNodes.Length > 0)
            {
                nodes.AddRange(tagNodes);
            }
        }

        // Find the closest pathfinding node to the AI
        GameObject closest = null;
        float closestDist = Mathf.Infinity;
        foreach (GameObject node in nodes)
        {
            float dist = Vector3.Distance(transform.position, node.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = node;
            }
        }

        // Return the closest pathfinding node
        return closest;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the collider is a pathfinding node and has a tag in the list of node tags, set it as the target node
        if (other.CompareTag("PathfindingNode"))
        {
            if (nodeTags.Contains(other.tag))
            {
                targetNode = other.gameObject;
            }
        }
    }
}
