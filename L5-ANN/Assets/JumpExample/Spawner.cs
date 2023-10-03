using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject projectile;
    public int secondsBetweenSpawns = 3;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 1, secondsBetweenSpawns);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void
        Spawn()
    {
        Instantiate(projectile, transform.position, Quaternion.identity);
        
    }
}
