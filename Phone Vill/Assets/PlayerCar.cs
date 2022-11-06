using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Car))]
public class PlayerCar : MonoBehaviour
{
    Car car;

    // Start is called before the first frame update
    void Start()
    {
        car = GetComponent<Car>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (car.currentRoad.leftLane) { car.ChangeLane(Car.Direction.Left); }
            else { car.Signal(Car.Direction.Left); }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (car.currentRoad.rightLane) { car.ChangeLane(Car.Direction.Right); }
            else { car.Signal(Car.Direction.Right); }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            car.Signal(Car.Direction.Straight);
        }

        car.isBraking = Input.GetKey(KeyCode.DownArrow);
        car.Drive();
    }
}