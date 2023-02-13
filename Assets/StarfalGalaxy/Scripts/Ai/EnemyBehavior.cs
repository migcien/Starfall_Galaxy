using UnityEngine;
using System.Collections.Generic;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 5f;
    public float attackRange = 2f;
    public List<string> nodeTags;

    [HideInInspector]
    public GameObject targetNode;

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
            targetNode = GetClosestNode();
            return;
        }

        // Check if the AI is within attack range
        if (Vector3.Distance(transform.position, targetNode.transform.position) <= attackRange)
        {
            // Attack the target and select the next target
            targetNode = GetClosestNode();
        }
        else
        {
            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, targetNode.transform.position, speed * Time.deltaTime);
        }
    }

    private GameObject GetClosestNode()
    {
        List<GameObject> nodes = new List<GameObject>();
        foreach (string tag in nodeTags)
        {
            GameObject[] tagNodes = GameObject.FindGameObjectsWithTag(tag);
            if (tagNodes.Length > 0)
            {
                nodes.AddRange(tagNodes);
            }
        }

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
        return closest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PathfindingNode"))
        {
            if (nodeTags.Contains(other.tag))
            {
                targetNode = other.gameObject;
            }
        }
    }
}
