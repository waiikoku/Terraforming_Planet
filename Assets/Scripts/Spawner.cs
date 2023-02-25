using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] ObjectToSpawn;


    public int SpawnAmount;
    public bool RandomPosition = true;
    public bool CenterOfArea = true;
    public Vector3 MinArea;
    public Vector3 MaxArea;

    public bool AvoidCollapse = true;
    public List<Vector3> ArrayPos;
    private void Start()
    {
        for (int i = 0; i < SpawnAmount; i++)
        {
            if (ObjectToSpawn.Length == 2)
            {
                float randomRate = Random.Range(0.0f, 100.0f);
                int choose;
                if(randomRate < 50f)
                {
                    choose = 1;
                }
                else
                {
                    choose = 0;
                }
                GameObject RandomObject = Instantiate(ObjectToSpawn[choose], transform);
                RandomObject.transform.position = RandomPos(MinArea, MaxArea, true);
            }           
        }
    }

    Vector3 RandomPos(Vector3 min,Vector3 max,bool Instant)
    {
        Vector3 result = new Vector3();
        if (Instant)
        {
            if (CenterOfArea)
            {
                result = new Vector3(Random.Range(min.x, max.x) + transform.position.x, Random.Range(min.y, max.y) + transform.position.y, Random.Range(min.z, max.z) + transform.position.z);
            }
            else
            {
                result = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
            }
            if (AvoidCollapse)
            {
                if (ArrayPos.Contains(result))
                {
                    return RandomPos(min, max, Instant);
                }
                else
                {
                    ArrayPos.Add(result);
                }
            }
        }
        return result;
    }
}
