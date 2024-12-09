using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CashRegister : MonoBehaviour
{
    public Transform[] Waypoints;
    private Dictionary<Transform, Waypoint> waypointComponents = new Dictionary<Transform, Waypoint>();

    void Start()
    {
        foreach (var waypoint in Waypoints)
        {
            var wp = waypoint.GetComponent<Waypoint>();
            if (wp != null)
            {
                waypointComponents[waypoint] = wp;
            }
            else
            {
                Debug.LogError($"Waypoint {waypoint.name} is missing a Waypoint component!");
            }
        }
    }

    public Transform AssignWaypoint(NPCController npc)
    {
        foreach (var pair in waypointComponents)
        {
            if (pair.Value.IsFree())
            {
                pair.Value.SetOccupant(npc);
                return pair.Key;
            }
        }
        return null;
    }

    public Transform GetNextWaypoint(NPCController npc, Transform currentWaypoint)
    {
        int index = System.Array.IndexOf(Waypoints, currentWaypoint);
        if (index < 0)
        {
            Debug.LogError("Current waypoint not found in register waypoints!");
            return null;
        }

        if (index < Waypoints.Length - 1)
        {
            var nextWaypoint = Waypoints[index + 1];
            if (waypointComponents[nextWaypoint].IsFree())
            {
                waypointComponents[currentWaypoint].ClearOccupant();
                waypointComponents[nextWaypoint].SetOccupant(npc);
                return nextWaypoint;
            }
        }

        if (index == Waypoints.Length - 1)
        {
            waypointComponents[currentWaypoint].ClearOccupant();
            return null;
        }

        return currentWaypoint;
    }

    public bool ContainsWaypoint(Transform waypoint)
    {
        return waypointComponents.ContainsKey(waypoint);
    }
}
