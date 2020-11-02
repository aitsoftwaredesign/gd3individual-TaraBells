using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBox : MonoBehaviour
{
    private float nextSpawnTime;
    public GameObject[] spawnPoints;
    private int spIndex = 0;

    public GameObject[] pattern;
    private int patternIndex = 0;


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
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            nextSpawnTime = Time.time + spawnDelay;
        }

        Instantiate(boxPrefab, transform.position, transform.rotation);
        }

    private bool ShouldSpawn()
    {
        return Time.time > nextSpawnTime;
    }

   
}
