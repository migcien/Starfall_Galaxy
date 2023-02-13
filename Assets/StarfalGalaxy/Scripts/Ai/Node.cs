using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    public string tag; // the tag of the node, used for filtering
    public List<Node> neighbors = new List<Node>(); // the neighboring nodes
    public Node parent; // the parent node, used for retracing the path
    public float gCost; // the cost of getting to this node from the start node
    public float hCost; // the heuristic cost of getting from this node to the end node
    public float fCost { get { return gCost + hCost; } } // the total cost of getting from the start node to the end node through this node

    private void Start()
    {
        // populate the list of neighbors based on the objects in the scene
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj != gameObject && Vector3.Distance(obj.transform.position, transform.position) < 10f)
            {
                Node neighborNode = obj.GetComponent<Node>();
                if (neighborNode != null)
                {
                    neighbors.Add(neighborNode);
                }
            }
        }
    }
}
