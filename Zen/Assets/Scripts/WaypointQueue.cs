using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint
{
    public Transform point;
    public float secondsBetweenPoints;

    public Waypoint(Transform point, float secondsBetweenPoints)
    {
        this.point = point;
        this.secondsBetweenPoints = secondsBetweenPoints;
    }
}

public class WaypointQueue : MonoBehaviour
{
    public float distanceToWaypointBeforeReached = 0.2f;
    public PlayerMovement movement;
    public Transform waypointParent;

    public bool ProcessingRequests { get { return waypoints.Count > 0; }}
    
    private Queue<Waypoint> waypoints = new Queue<Waypoint>();

    private Waypoint currentWaypoint;

    public void AddWaypoints(List<Waypoint> waypoints)
    {
        if (waypoints.Count > 0)
        {
            foreach (var w in waypoints)
            {
                this.waypoints.Enqueue(w);
                w.point.transform.SetParent(waypointParent, true);
            }

            if (currentWaypoint == null)
            {
                currentWaypoint = this.waypoints.Dequeue();
            }
        }
    }

    private void Update()
    {
        if (currentWaypoint != null)
        {
            movement.MoveTowards(currentWaypoint);

            if(Vector2.Distance(transform.position, currentWaypoint.point.position) <= distanceToWaypointBeforeReached)
            {
                Destroy(currentWaypoint.point.gameObject);

                if (waypoints.Count == 0)
                {
                    currentWaypoint = null;
                    movement.ResetSpeed();
                }
                else
                {
                    currentWaypoint = waypoints.Dequeue();
                }
            }
        }
    }
}
