using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Car))]
public class NPC : MonoBehaviour
{
    bool isDistracted = false;
    Car car;

    // Start is called before the first frame update
    void Start()
    {
        car = GetComponent<Car>();
        car.speed = Random.RandomRange(200f, 300f);

        Invoke("Distract", Random.RandomRange(15f, 45f));
        Invoke("NextMove", Random.RandomRange(10f, 20f));
    }

    // Update is called once per frame
    void Update()
    {
        car.Drive();
    }

    void Distract()
    {
        isDistracted = true;
        Invoke("StopDistract", Random.RandomRange(5f, 15f));
    }

    void StopDistract()
    {
        isDistracted = false;
        Invoke("Distract", Random.RandomRange(15f, 45f));
    }

    void NextMove()
    {
        if (isDistracted || Random.value <= 0.2f)
        {
            car.Signal(Car.Direction.Straight);
        }
        else if (Random.value <= 0.5f)
        {
            if (car.currentRoad.leftLane) { car.ChangeLane(Car.Direction.Left); }
            else { car.Signal(Car.Direction.Left); }
        }
        else if (car.currentRoad.rightLane) { car.ChangeLane(Car.Direction.Right); }
        else { car.Signal(Car.Direction.Right); }

        Invoke("NextMove", Random.RandomRange(10f, 20f));
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Car>())
        {
            Destroy(gameObject);
            if (collider.GetComponent<PlayerCar>())
            {
                collider.GetComponent<PlayerCar>().Failed();
            }
            else
            {
                Destroy(collider.gameObject);
            }
        }
    }
}