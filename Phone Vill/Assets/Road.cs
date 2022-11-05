using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public enum RoadType
    {
        Straight,
        LeftTurn,
        RightTurn
    }

    public RoadType turnType = RoadType.Straight;
    public Transform straightRoad, leftLane, rightLane, leftFork, rightFork;

    Vector3 endpoint;

    // Start is called before the first frame update
    void Start()
    {
        endpoint = straightRoad.position;
    }

    // Update is called once per frame
    void Update()
    {
        endpoint = straightRoad.position;
    }

    public float GetLength()
    {
        float d = Vector3.Distance(transform.position, endpoint);
        if (turnType == RoadType.Straight)
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
        float midY = transform.position.y + endpoint.y / 2;
        float downCurve = Mathf.Sin(portion * Mathf.PI / 2);
        float upCurve = 1 - Mathf.Cos(portion * Mathf.PI / 2);
        float x = 0;
        float z = 0;

        switch (turnType)
        {
            case RoadType.Straight:
                return straight;
            case RoadType.LeftTurn:
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
            case RoadType.RightTurn:
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

        print("Delta: " + delta);
        return new Vector3(fx, midY, fz);
    }
}