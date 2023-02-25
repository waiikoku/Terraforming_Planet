using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SphereGrid : MonoBehaviour
{
    public bool spawnVoxelSphere = false;
    public bool stopCoroutine = false;
    public bool DeleteAll = false;
    public float secPerSpawn = 1f;
    public GameObject prefab;
    public Transform target;
    public List<GameObject> container;
    public int currentIndex = 0;
    public int currentSize = 1;
    public float gizmosSize = 1f;
    public int radius = 8;
    public int height = 8;
    public bool isGizmos;

    private void Update()
    {
        if (spawnVoxelSphere)
        {
            spawnVoxelSphere = false;
            StartCoroutine("SpawnSlowly");
        }
        if (stopCoroutine)
        {
            stopCoroutine = false;
            StopAllCoroutines();
        }
        if (DeleteAll)
        {
            DeleteAll = false;
            for (int i = 0; i < container.Count; i++)
            {
                DestroyImmediate(container[i]);
            }
            container.Clear();
        }
    }

    IEnumerator SpawnSlowly()
    {
        bool isFinished = false;
        while (!isFinished)
        {
            for (int i = -radius; i < radius; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    for (int k = -radius; k < radius; k++)
                    {
                        Vector3 pos = new Vector3(i, j, k);
                        float distance = ((pos + target.position) - target.position).sqrMagnitude;
                        if (distance < radius)
                        {
                            GameObject voxel = Instantiate(prefab, target);
                            voxel.transform.localPosition = pos + target.position;
                            if (!container.Contains(voxel))
                            {
                                container.Add(voxel);
                            }
                            voxel.name = "Voxel No." + currentIndex;
                            currentIndex++;
                            yield return null;
                        }

                    }
                }
            }
            isFinished = true;
        }
    }
    private void OnDrawGizmos()
    {
        if (isGizmos)
        {
            Gizmos.color = Color.blue;
            for (int i = -radius; i < radius; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    for (int k = -radius; k < radius; k++)
                    {
                        Vector3 pos = new Vector3(i, j, k);
                        pos += transform.position;
                        float distance = (pos - transform.position).sqrMagnitude;
                        if (distance < radius)
                        {
                            Gizmos.DrawCube(new Vector3(i, j, k), Vector3.one * gizmosSize);
                        }
                    }
                }
            }
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
