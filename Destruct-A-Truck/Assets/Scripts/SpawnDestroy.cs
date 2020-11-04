using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDestroy : MonoBehaviour
{
    public Transform[] spawnPlace;
    public GameObject boxPrefab;
    
  
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Spawn()
    {
        boxPrefab = Instantiate(boxPrefab, spawnPlace[0].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
    }
    
}
