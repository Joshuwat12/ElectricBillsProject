using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public const float MphToUps = 251f / 600f;
    public enum Direction
    {
        Straight,
        Left,
        Right
    }

    public Road startRoad;
    [HideInInspector] public Road currentRoad;
    [Tooltip("The speed in mph")] public float speed = 40;
    [Tooltip("The number of seconds it takes to change lanes")] public float changeSpeed = 1;
    [Tooltip("The transform to use for orienting the car")] public Transform pointer;
    [HideInInspector] public bool isBraking = false;

    Direction changeDir = Direction.Straight;
    Direction signalDir = Direction.Straight;
    float distance = 0;
    float change = 0;


    // Start is called before the first frame update
    void Start()
    {
        currentRoad = startRoad;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drive()
    {
        if (!isBraking)
        {
            distance += speed * MphToUps * Time.deltaTime;
            while (distance >= currentRoad.GetLength())
            {
                distance -= currentRoad.GetLength();
                if (signalDir == Direction.Left && currentRoad.leftFork)
                { 
                    currentRoad = currentRoad.leftFork;
                    signalDir = Direction.Straight;
                }
                else if (signalDir == Direction.Right && currentRoad.rightFork)
                {
                    currentRoad = currentRoad.rightFork;
                    signalDir = Direction.Straight;
                }
                else { currentRoad = currentRoad.straightRoad; }
            }

            Vector3 posOnRoad = currentRoad.DistanceToPos(distance);
            if (changeDir != Direction.Straight)
            {
                change += Time.deltaTime / changeSpeed;
                if (change >= 1)
                {
                    if (changeDir == Direction.Left) { currentRoad = currentRoad.leftLane; }
                    else { currentRoad = currentRoad.rightLane; }
                    changeDir = Direction.Straight;
                    change = 0;
                    transform.position = currentRoad.DistanceToPos(distance);
                }
                else
                {
                    float curve = (1 - Mathf.Cos(change * Mathf.PI)) / 2;
                    if (changeDir == Direction.Left)
                    {
                        transform.position = Vector3.Lerp(posOnRoad, currentRoad.leftLane.DistanceToPos(distance), curve);
                    }
                    else
                    {
                        transform.position = Vector3.Lerp(posOnRoad, currentRoad.rightLane.DistanceToPos(distance), curve);
                    }
                }
            }
            else
            {
                transform.position = posOnRoad;
            }

            if (changeDir == Direction.Straight)
            {
                pointer.position = currentRoad.DistanceToPos(distance + 0.1f);
                transform.LookAt(pointer);
            }
        }
    }

    public void ChangeLane(Direction dir)
    {
        if (changeDir == Direction.Straight && ((dir == Direction.Left && currentRoad.leftLane) || (dir == Direction.Right && currentRoad.rightLane)))
        {
            changeDir = dir;
            signalDir = Direction.Straight;
        }
    }

    public void Signal(Direction dir)
    {
        signalDir = dir;
    }

    public float DistanceToCar(Car other, Road check = null, float d = 0)
    {
        if (!check) { check = currentRoad; }
        if (check == other.currentRoad && distance <= other.distance)
        {
            return other.distance - distance;
        }

        d -= distance;
        List<Road> roadsChecked = new List<Road>();
        while (!roadsChecked.Contains(check))
        {
            d += check.GetLength();
            roadsChecked.Add(check);
            if (signalDir == Direction.Left && check.leftFork) { check = check.leftFork; }
            else if (signalDir == Direction.Right && check.rightFork) { check = check.rightFork; }
            else { check = check.straightRoad; }

            if (check == other.currentRoad)
            {
                return d + other.distance;
            }
        }
        return -1;
    }

    public Car NearestCar(Road check = null, float startDistance = 0)
    {
        Car nearestC = null;
        float nearestD = 999999;
        float d;

        foreach(Car c in FindObjectsOfType<Car>())
        {
            d = DistanceToCar(c, check, startDistance);
            if (d >= 0 && d < nearestD && c != this)
            {
                nearestC = c;
                nearestD = d;
            }
        }

        return nearestC;
    }

    public float DistToNearestCar(Road check = null, float d = 0)
    {
        return DistanceToCar(NearestCar(check, d), check, d);
    }
}