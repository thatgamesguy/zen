using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerDrag : MonoBehaviour
{
    public float catchmentRadius = 5;
    public float distanceBetweenWaypointCreation = 1f;
    public WaypointQueue queue;
    public GameObject waypointPrefab;
    public int maxWaypoints = 10;
    private bool caught = false;
    private List<Waypoint> waypoints = new List<Waypoint>();
    private float waypointTimer = 0f;

    private void Update()
    {
        if (queue.ProcessingRequests) { return; }

        if (caught && Input.GetMouseButtonUp(0))
        {
            ProcessRelease();
        }
        if (caught && Input.GetMouseButton(0))
        {
            ProcessDrag();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            CheckForCatch();
        }
    }

    private void CheckForCatch()
    {
        Vector2 clickPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(clickPosition, transform.position) < catchmentRadius)
        {
            caught = true;
        }
    }

    private void ProcessDrag()
    {
        waypointTimer += Time.deltaTime;

        Vector2 currentWaypointPos = (waypoints.Count > 0) ? waypoints[waypoints.Count - 1].point.position : transform.position;
        Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 heading = mousePosition - currentWaypointPos;
        float distance = heading.magnitude;

        if (distance >= distanceBetweenWaypointCreation)
        {
            Vector2 direction = heading / distance;
            Vector2 waypointPos = currentWaypointPos + (direction * distanceBetweenWaypointCreation);

            Transform waypointTransform = Instantiate(waypointPrefab).transform;
            waypointTransform.SetParent(transform, true);
            waypointTransform.position = waypointPos;
            waypoints.Add(new Waypoint(waypointTransform, waypointTimer));
            waypointTimer = 0f;

            if (waypoints.Count >= maxWaypoints)
            {
                ProcessRelease();
            }
        }
    }

    private void ProcessRelease()
    {
        caught = false;
        waypointTimer = 0f;
        queue.AddWaypoints(waypoints);
        waypoints.Clear();
    }
}
