using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Road startRoad;

    float distance = 0;
    Road currentRoad;

    // Start is called before the first frame update
    void Start()
    {
        currentRoad = startRoad;
    }

    // Update is called once per frame
    void Update()
    {
        Drive(Time.deltaTime);
        transform.position = currentRoad.PortionToPos(currentRoad.DistanceToPortion(distance));
    }

    public void Drive(float d)
    {
        distance += d;
        while (distance >= currentRoad.GetLength())
        {
            distance -= currentRoad.GetLength();
            currentRoad = currentRoad.straightRoad.GetComponent<Road>();
            // print("Road changed");
        }
    }
}