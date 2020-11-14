using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBreak : MonoBehaviour
{
    public GameObject breakItem;
   
    public int itemCount;

    public GameObject[] spawnPoints;
    private int spIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    IEnumerator spDrop()
    {
        for (int i = 0; spawnPoints[i]; i++)
        {
      
            Instantiate(breakItem, new Vector3(0,0,0),Quaternion.identity);

            yield return new WaitForSeconds(1);
            itemCount += 1;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
