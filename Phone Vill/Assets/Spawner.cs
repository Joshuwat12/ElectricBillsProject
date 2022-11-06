using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject item;
    public Road spawnRoad;
    public float interval;

    GameObject newItem;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", interval, interval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        newItem = Instantiate(item);
        newItem.GetComponent<Car>().startRoad = spawnRoad;
    }
}