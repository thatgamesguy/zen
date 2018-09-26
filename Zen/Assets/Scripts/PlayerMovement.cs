using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 minMaxSpeed = new Vector2(0.5f, 2f);
    public float largestSpeedChange = 0.5f;

    private float currentSpeed = 0f;

    public void MoveTowards(Waypoint waypoint)
    {
        Vector2 heading = waypoint.point.position - transform.position;
        float distance = heading.magnitude;
        Vector2 direction = heading / distance;

        float desiredSpeed = Mathf.Clamp(distance / waypoint.secondsBetweenPoints, minMaxSpeed.x, minMaxSpeed.y);

        if(!Mathf.Approximately(desiredSpeed, currentSpeed))
        {
            currentSpeed += largestSpeedChange * Mathf.Sign((desiredSpeed - currentSpeed)) * Time.deltaTime;

            print(currentSpeed);
        }

        transform.position += (Vector3)direction * currentSpeed  * Time.deltaTime;
    }

    public void ResetSpeed()
    {
        currentSpeed = 0f;
    }
}
