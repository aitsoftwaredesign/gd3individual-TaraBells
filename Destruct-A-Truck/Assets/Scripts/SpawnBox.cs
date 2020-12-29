using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBox : MonoBehaviour
{
    private float nextSpawnTime;
    public GameObject[] spawnPoints;
 //   private int spIndex = 0;

    public GameObject[] pattern;
    //private int patternIndex = 0;


    //I used a serialized field because I wanted to have this because with it being serialized i can view it in the inspector and place this at any time
    [SerializeField]
    private GameObject boxPrefab;
    [SerializeField]
    private float spawnDelay = 5;

    public GameObject box;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    private void Update()
    {
        if (ShouldSpawn())
        {
            SpawnBoxes();
            
        }

    }

    private void SpawnBoxes()
    {
        
            nextSpawnTime = Time.time + spawnDelay;
    

        Instantiate(boxPrefab, transform.position, transform.rotation);
        
     }

    private bool ShouldSpawn()
    {
        return Time.time > nextSpawnTime;
    }

 /*  for (int i = 0; i<wheel.Length; i++)
        {
            Vector3 wheelPosWs = transform.TransformPoint(wheelPos[i]);
    wheel[i] = Instantiate(wheelPrefab,
        wheelPosWs + (-transform.up* suspensionHeight),
                Quaternion.identity);
        }*/
}
