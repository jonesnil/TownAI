using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Panda;

public class PandaBTScripts : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject[] waypoints;
    GameObject curDest;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 30;
        waypoints = GameObject.FindGameObjectsWithTag("waypoint");
        if (waypoints.Length <= 0) Debug.Log("No waypoints found!");
    }


    [Task]
    public void SetNewRandomDestination()
    {
        Vector3 dest; 
        if (waypoints.Length > 0)
        {
            curDest = waypoints[Random.Range(0, waypoints.Length - 1)];
            agent.transform.LookAt(curDest.transform);
            dest = curDest.transform.position;
        }
        else
        {
            dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        }

        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    [Task]
    public void MoveToNewDestination()
    {
        if (Task.isInspected) // showing in the inspector 
        {
            Task.current.debugInfo = string.Format(curDest.ToString() + " t= {0:0.00}", Time.time);
        }
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Task.current.Succeed();
        }
    }
}

