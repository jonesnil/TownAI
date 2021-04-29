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
    GameObject[] farms;
    GameObject curDest;
    float hunger;
    float morale;
    public float resource;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 2;
        waypoints = GameObject.FindGameObjectsWithTag("waypoint");
        if (waypoints.Length <= 0) Debug.Log("No waypoints found!");
        hunger = 100;
        morale = 10;
        resource = 0;

        farms = new GameObject[3];
        int index = 0;
        foreach (GameObject waypoint in waypoints)
        {
            if (waypoint.name == "FarmfieldWaypoint1" || 
                waypoint.name == "FarmfieldWaypoint2" ||
                waypoint.name == "FarmfieldWaypoint3")
            {
                farms[index] = waypoint;
                index += 1;
            }
        }
    }

    private void Update()
    {

        hunger -= Time.deltaTime * 2;
        morale -= Time.deltaTime;
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

    [Task]
    public void SetDestinationChurch()
    {
        Vector3 dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        if (waypoints.Length > 0)
        {
            foreach (GameObject waypoint in waypoints) 
            {
                if (waypoint.name == "ChurchWaypoint") 
                {
                    curDest = waypoint;
                    agent.transform.LookAt(curDest.transform);
                    dest = curDest.transform.position;
                    break;
                }
            }
        }

        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    [Task]
    public void SetDestinationMarket()
    {
        Vector3 dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        if (waypoints.Length > 0)
        {
            foreach (GameObject waypoint in waypoints)
            {
                if (waypoint.name == "MarketWaypoint1")
                {
                    curDest = waypoint;
                    agent.transform.LookAt(curDest.transform);
                    dest = curDest.transform.position;
                    break;
                }
            }
        }

        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    [Task]
    public void SetDestinationPriestHome()
    {
        Vector3 dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        if (waypoints.Length > 0)
        {
            foreach (GameObject waypoint in waypoints)
            {
                if (waypoint.name == "House1Waypoint")
                {
                    curDest = waypoint;
                    agent.transform.LookAt(curDest.transform);
                    dest = curDest.transform.position;
                    break;
                }
            }
        }

        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    [Task]
    public void SetDestinationMinerHome()
    {
        Vector3 dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        if (waypoints.Length > 0)
        {
            foreach (GameObject waypoint in waypoints)
            {
                if (waypoint.name == "House2Waypoint")
                {
                    curDest = waypoint;
                    agent.transform.LookAt(curDest.transform);
                    dest = curDest.transform.position;
                    break;
                }
            }
        }

        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    [Task]
    public void SetDestinationFarmerHome()
    {
        Vector3 dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        if (waypoints.Length > 0)
        {
            foreach (GameObject waypoint in waypoints)
            {
                if (waypoint.name == "House3Waypoint")
                {
                    curDest = waypoint;
                    agent.transform.LookAt(curDest.transform);
                    dest = curDest.transform.position;
                    break;
                }
            }
        }

        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    [Task]
    public void SetDestinationMine()
    {
        Vector3 dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        if (waypoints.Length > 0)
        {
            foreach (GameObject waypoint in waypoints)
            {
                if (waypoint.name == "MineEntranceWaypoint")
                {
                    curDest = waypoint;
                    agent.transform.LookAt(curDest.transform);
                    dest = curDest.transform.position;
                    break;
                }
            }
        }

        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    [Task]
    public void SetDestinationFarm()
    {
        Vector3 dest;
        if (waypoints.Length > 0)
        {
            curDest = farms[Random.Range(0, farms.Length)];
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
    public void SetDestinationMill()
    {
        Vector3 dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        if (waypoints.Length > 0)
        {
            foreach (GameObject waypoint in waypoints)
            {
                if (waypoint.name == "MillWaypoint")
                {
                    curDest = waypoint;
                    agent.transform.LookAt(curDest.transform);
                    dest = curDest.transform.position;
                    break;
                }
            }
        }

        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    [Task]
    public void SetDestinationSmith()
    {
        Vector3 dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        if (waypoints.Length > 0)
        {
            foreach (GameObject waypoint in waypoints)
            {
                if (waypoint.name == "BlacksmithWaypoint")
                {
                    curDest = waypoint;
                    agent.transform.LookAt(curDest.transform);
                    dest = curDest.transform.position;
                    break;
                }
            }
        }

        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    [Task]
    public void Hungry() 
    {
        if (hunger > 30)
        {
            Task.current.Fail();
        }
        else 
        {
            Task.current.Succeed();
        }
    }

    [Task]
    public void Demotivated()
    {
        if (morale > 30)
        {
            Task.current.Fail();
        }
        else
        {
            Task.current.Succeed();
        }
    }

    [Task]
    public void EncumberedWithResource()
    {
        if (resource > 100)
        {
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
    }

    [Task]
    public void Eat()
    {
        hunger = 100;
        Task.current.Succeed();
    }

    [Task]
    public void Hear()
    {
        
        morale += Time.deltaTime * 10;
        
        if(morale >= 100)
            Task.current.Succeed();
    }

    [Task]
    public void DumpResource()
    {

        resource = 0;
        Task.current.Succeed();
    }

    [Task]
    public void GainResource()
    {
        resource += Time.deltaTime * 20;
        Task.current.Succeed();
    }



    [Task]
    public void IsNightTime()
    {
        if (LightingManager.TimeOfDay >= 18f || LightingManager.TimeOfDay <= 6f)
            Task.current.Succeed();
        else
            Task.current.Fail();
    }



}

