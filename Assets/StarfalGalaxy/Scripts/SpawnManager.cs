using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public int density = 8;
    public Vector3 minExtents = new Vector3(-5f, 0f, -5f);
    public Vector3 maxExtents = new Vector3(5f, 0f, 5f);
    public bool snapToGrid = true;
    public float gridSize = 1f;
    public int[] probabilities;
    public Vector3 scale = Vector3.one;
    public float poolSize = 10f;
    //public CinemachineVirtualCamera mainCamera;
    public Camera mainCamera;


    private List<GameObject> pool = new List<GameObject>();
    private List<Vector3> occupiedPositions = new List<Vector3>();
    public float minDistance = 0.5f;
    public float maxDistance = 2f;

    private void Start()
    {
        //mainCamera = GetComponent<CinemachineVirtualCamera>();
        //mainCamera = GetComponent<CinemachineVirtualCamera>().GetComponent<Camera>();
        mainCamera = Camera.main;


        // Initialize object pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obj.transform.parent = transform;
            obj.SetActive(false);
            obj.tag = "SpawnedObject";
            pool.Add(obj);
        }

        // Spawn objects
        for (int i = 0; i < density; i++)
        {
            SpawnObject();
        }
    }

    private void Update()
    {
        // Check if any spawned objects are outside camera view and return them to the pool
        for (int i = occupiedPositions.Count - 1; i >= 0; i--)
        {
            Vector3 pos = occupiedPositions[i];
            GameObject obj = FindOccupiedObject(pos);

            if (obj == null)
            {
                // Object was returned to pool
                occupiedPositions.RemoveAt(i);
                continue;
            }

            if (!IsInCameraView(obj))
            {
                obj.SetActive(false);
                occupiedPositions.RemoveAt(i);
            }
        }

        // Spawn new objects if necessary
        if (occupiedPositions.Count < density)
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        CheckOccupiedPositions();
        GameObject prefab = GetPooledObject();
        if (prefab != null)
        {
            int prefabIndex = GetRandomPrefabIndex();
            Vector3 position = GetRandomPosition();
            while (!CanSpawnAtPosition(position))
            {
                position = GetRandomPosition();
            }
            prefab.transform.position = position;
            prefab.transform.localScale = scale;
            if (snapToGrid) prefab.transform.position = SnapToGrid(prefab.transform.position);
            prefab.SetActive(true);
            occupiedPositions.Add(position);
        }
    }

    private bool CanSpawnAtPosition(Vector3 position)
    {
        if (occupiedPositions.Count == 0)
        {
            return true;
        }
        foreach (Vector3 occupiedPosition in occupiedPositions)
        {
            float distance = Vector3.Distance(position, occupiedPosition);
            if (distance < minDistance || distance > maxDistance)
            {
                continue;
            }
            return false;
        }
        return true;
    }

    private void CheckOccupiedPositions()
    {
        for (int i = occupiedPositions.Count - 1; i >= 0; i--)
        {
            Vector3 pos = occupiedPositions[i];
            GameObject obj = FindOccupiedObject(pos);

            if (obj == null)
            {
                // Object was returned to pool
                occupiedPositions.RemoveAt(i);
            }
            else
            {
                // Check if object is too far from its occupied position
                float distance = Vector3.Distance(obj.transform.position, pos);
                if (distance > maxDistance)
                {
                    obj.SetActive(false);
                    occupiedPositions.RemoveAt(i);
                }
            }
        }
    }

    private GameObject FindOccupiedObject(Vector3 position)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("SpawnedObject");
        foreach (GameObject obj in objectsWithTag)
        {
            if (obj.activeSelf && Vector3.Distance(obj.transform.position, position) < 0.1f)
            {
                return obj;
            }
        }
        return null;
    }

    private GameObject GetPooledObject()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }

        // If no objects are available in the pool, create a new one and add it to the pool
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        obj.transform.parent = transform;
        obj.SetActive(false);
        obj.tag = "SpawnedObject";
        pool.Add(obj);
        return obj;
    }

    private int GetRandomPrefabIndex()
    {
        int total = 0;
        foreach (int prob in probabilities)
        {
            total += prob;
        }
        int randomNum = Random.Range(0, total);
        int currentIndex = 0;
        for (int i = 0; i < probabilities.Length; i++)
        {
            if (randomNum >= currentIndex && randomNum < currentIndex + probabilities[i])
            {
                return i;
            }
            currentIndex += probabilities[i];
        }
        return 0;
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(minExtents.x, maxExtents.x), 0f, Random.Range(minExtents.z, maxExtents.z));
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x / gridSize) * gridSize, position.y, Mathf.Round(position.z / gridSize) * gridSize);
    }

    private bool IsInCameraView(GameObject obj)
    {
        if (mainCamera == null)
        {
            return true;
        }

        Collider objCollider = obj.GetComponent<Collider>();
        if (objCollider == null)
        {
            return true;
        }

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        return GeometryUtility.TestPlanesAABB(planes, objCollider.bounds);
    }
}
