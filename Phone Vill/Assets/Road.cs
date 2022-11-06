using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public Car.Direction turnType = Car.Direction.Straight;
    public Road straightRoad, leftLane, rightLane, leftFork, rightFork;
    public bool isEnd = false;

    Vector3 endpoint;

    // Start is called before the first frame update
    void Start()
    {
        endpoint = straightRoad.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        endpoint = straightRoad.transform.position;
    }

    public float GetLength()
    {
        float d = Vector3.Distance(transform.position, endpoint);
        if (turnType == Car.Direction.Straight)
        {
            return d;
        }
        float r = d / Mathf.Sqrt(2);
        return Mathf.PI * r / 2;
    }
    
    public float DistanceToPortion(float distance)
    {
        return distance / GetLength();
    }

    public float PortionToDistance(float portion)
    {
        return portion * GetLength();
    }

    public Vector3 PortionToPos(float portion)
    {
        Vector3 straight = Vector3.Lerp(transform.position, endpoint, portion);
        Vector3 delta = endpoint - transform.position;
        float downCurve = Mathf.Sin(portion * Mathf.PI / 2);
        float upCurve = 1 - Mathf.Cos(portion * Mathf.PI / 2);
        float x = 0;
        float z = 0;

        switch (turnType)
        {
            case Car.Direction.Straight:
                return straight;
            case Car.Direction.Left:
                switch (Mathf.Sign(delta.x * delta.z))
                {
                    case 1:
                        x = downCurve;
                        z = upCurve;
                        break;
                    case -1:
                        x = upCurve;
                        z = downCurve;
                        break;
                    default:
                        return straight;
                }
                break;
            case Car.Direction.Right:
                switch (Mathf.Sign(delta.x * delta.z))
                {
                    case -1:
                        x = downCurve;
                        z = upCurve;
                        break;
                    case 1:
                        x = upCurve;
                        z = downCurve;
                        break;
                    default:
                        return straight;
                }
                break;
        }

        float fx = endpoint.x * x + transform.position.x * (1 - x);
        float fz = endpoint.z * z + transform.position.z * (1 - z);

        return new Vector3(fx, straight.y, fz);
    }

    public Vector3 DistanceToPos(float distance)
    {
        if (distance >= GetLength())
        {
            return straightRoad.DistanceToPos(distance - GetLength());
        }
        return PortionToPos(DistanceToPortion(distance));
    }

    void OnDrawGizmos()
    {
        // Gizmos.DrawLine(transform.position, endpoint);
    }
}