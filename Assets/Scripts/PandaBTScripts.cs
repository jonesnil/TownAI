using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Panda;

// The enum system here is just to make it easier for me to throw around
// actions and identify them to perform them/check their requirements.

// The action names are used to decide what specifically to do for each 
// action, and the goals are used both to find chains for the plans and
// to check if they are completed.
public enum ActionName 
{ 
    GoToBlacksmith,
    GoToChurch,
    GoToMarket,
    GoHome,
    Mine,
    HearSermon,
    GetPickaxe,
    DumpOre,
    Eat
}
public enum Goal
{
    HavePickaxe,
    GetOre,
    GetToChurch,
    GetToBlacksmith,
    GetToMarket,
    GetHome,
    BecomeMotivated,
    HaveNoOre,
    QuenchHunger,
    None
}

public class PandaBTScripts : MonoBehaviour
{
    List<Action> actions;
    List<Action> actionPlan;
    Goal curGoal;
    int actionIndex;

    NavMeshAgent agent;
    GameObject[] waypoints;
    GameObject[] farms;
    GameObject curDest;
    float hunger;
    public float morale;
    public float resource;
    bool havePickaxe;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 2;
        waypoints = GameObject.FindGameObjectsWithTag("waypoint");
        if (waypoints.Length <= 0) Debug.Log("No waypoints found!");
        hunger = 100;
        morale = 10;
        resource = 0;
        havePickaxe = false;
        actionIndex = 0;
        
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

        actions = new List<Action>();

        Action GoToBlackSmith = new Action(Goal.None, Goal.GetToBlacksmith, ActionName.GoToBlacksmith);
        actions.Add(GoToBlackSmith);

        Action GoToChurch = new Action(Goal.None, Goal.GetToChurch, ActionName.GoToChurch);
        actions.Add(GoToChurch);

        Action GoToMarket = new Action(Goal.None, Goal.GetToMarket, ActionName.GoToMarket);
        actions.Add(GoToMarket);

        Action GoHome = new Action(Goal.None, Goal.GetHome, ActionName.GoHome);
        actions.Add(GoHome);

        Action GetPickaxe = new Action(Goal.GetToBlacksmith, Goal.HavePickaxe, ActionName.GetPickaxe);
        actions.Add(GetPickaxe);

        Action DumpOre = new Action(Goal.GetToBlacksmith, Goal.HaveNoOre, ActionName.DumpOre);
        actions.Add(DumpOre);

        Action Eat = new Action(Goal.GetToMarket, Goal.QuenchHunger, ActionName.Eat);
        actions.Add(Eat);

        Action GetOre = new Action(Goal.HavePickaxe, Goal.GetOre, ActionName.Mine);
        actions.Add(GetOre);

        Action HearSermon = new Action(Goal.GetToChurch, Goal.BecomeMotivated, ActionName.HearSermon);
        actions.Add(HearSermon);
    }

    private void Update()
    {

        hunger -= Time.deltaTime * 2;
        morale -= Time.deltaTime;

        // If we have an action plan, and the action being asked of us isn't already completed, 
        // perform the action we're on.

        if (actionPlan != null && actionIndex < actionPlan.Count && !CheckRequirements(curGoal))
        {
            PerformAction(actionPlan[actionIndex].name);
        }
        else if (actionPlan != null && actionIndex >= actionPlan.Count) 
        {
            // If we've gone over every action in our action plan, we should set our
            // current goal to none so later we can be given the same goal and still
            // do it.

            curGoal = Goal.None;
        }
            
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

    //
    // All the tasks below here are to give Panda BT access to the GOAP system.  
    // When called they create a plan to meet whatever goal the behavior tree
    // wants met.
    //

    [Task]
    public void MakePlanMine() 
    {
        SetPlan(Goal.GetOre);
        Task.current.Succeed();
    }

    [Task]
    public void MakePlanDumpResource()
    {
        SetPlan(Goal.HaveNoOre);
        Task.current.Succeed();
    }

    [Task]
    public void MakePlanQuenchHunger()
    {
        SetPlan(Goal.QuenchHunger);
        Task.current.Succeed();
    }

    [Task]
    public void MakePlanGoHome()
    {
        SetPlan(Goal.GetHome);
        Task.current.Succeed();
    }

    [Task]
    public void MakePlanBecomeMotivated()
    {
        SetPlan(Goal.BecomeMotivated);
        if (CheckRequirements(Goal.BecomeMotivated))
            Task.current.Succeed();
    }

    // This is used by all the make plan tasks that are made
    // for the behavior tree.
    void SetPlan(Goal goal) 
    {
        // If our current goal is the same as the one we're being
        // asked to do, we don't make a new plan since the plan we have
        // already meets the goal.

        if (curGoal != goal)
        {
            this.actionPlan = MakePlan(goal);
            this.actionIndex = 0;
            this.curGoal = goal;
        }
    }

    // This is a recursive function that returns a full plan given a goal. If
    // it finds a way to fulfill it's goal that has a requirement it recursively
    // figures out what actions are required to fill that requirement, and finally
    // spits out the full plan.
    List<Action> MakePlan(Goal goal) 
    {
        List<Action> output = new List<Action>();

        foreach (Action action in actions) 
        {
            if (action.outcome == goal) 
            {
                bool readyToDoAction = CheckRequirements(action.requirement);

                if (readyToDoAction)
                {
                    output.Add(action);
                    return output;
                }

                else
                {
                    List<Action> priorPlan = MakePlan(action.requirement);
                    priorPlan.Add(action);
                    return priorPlan;
                }
            }
        }

        return output;

    }

    // This takes a requirement and returns true if it is met, and false
    // if it's not. It uses the enum system to know what requirements to 
    // accept. If none is passed here it always returns true, because if
    // there is no requirement it is always satisfied.
    bool CheckRequirements(Goal requirement) 
    {
        bool output = false;
        Vector3 dest;

        switch (requirement) 
        {
            case Goal.GetOre:
                if (this.resource >= 100)
                    output = true;
                break;

            case Goal.HavePickaxe:
                if (this.havePickaxe)
                    output = true;
                break;

            case Goal.GetToChurch:

                dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                if (waypoints.Length > 0)
                {
                    foreach (GameObject waypoint in waypoints)
                    {
                        if (waypoint.name == "ChurchWaypoint")
                        {
                            curDest = waypoint;
                            dest = curDest.transform.position;
                            break;
                        }
                    }
                }

                if (Vector3.Distance(this.transform.position, dest) <= agent.stoppingDistance)
                    output = true;

                break;

            case Goal.GetToBlacksmith:
                dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                if (waypoints.Length > 0)
                {
                    foreach (GameObject waypoint in waypoints)
                    {
                        if (waypoint.name == "BlacksmithWaypoint")
                        {
                            curDest = waypoint;
                            dest = curDest.transform.position;
                            break;
                        }
                    }
                }

                if (Vector3.Distance(this.transform.position, dest) <= agent.stoppingDistance)
                    output = true;

                break;


            case Goal.GetToMarket:
                dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                if (waypoints.Length > 0)
                {
                    foreach (GameObject waypoint in waypoints)
                    {
                        if (waypoint.name == "MarketWaypoint1")
                        {
                            curDest = waypoint;
                            dest = curDest.transform.position;
                            break;
                        }
                    }
                }

                if (Vector3.Distance(this.transform.position, dest) <= agent.stoppingDistance)
                    output = true;

                break;

            case Goal.GetHome:
                dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                if (waypoints.Length > 0)
                {
                    foreach (GameObject waypoint in waypoints)
                    {
                        if (waypoint.name == "House2Waypoint")
                        {
                            curDest = waypoint;
                            dest = curDest.transform.position;
                            break;
                        }
                    }
                }

                if (Vector3.Distance(this.transform.position, dest) <= agent.stoppingDistance)
                    output = true;

                break;

            case Goal.BecomeMotivated:
                if (this.morale >= 100)
                    output = true;
                break;

            case Goal.HaveNoOre:
                if (this.resource == 0)
                    output = true;
                break;

            case Goal.QuenchHunger:
                if (this.hunger == 0)
                    output = true;
                break;

            case Goal.None:
                output = true;
                break;
        }

        return output;
    }

    // This does what it says on the tin. It takes an action, figures out what it
    // is by the enum action name, and then does something to make the action happen.
    void PerformAction(ActionName action)
    {
        Vector3 dest;

        switch (action)
        {
            case ActionName.GoToBlacksmith:
                dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
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

                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    actionIndex += 1;
                }

                break;

            case ActionName.Mine:

                dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
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

                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    resource += Time.deltaTime * 20;
                }

                if (resource >= 100) 
                {
                    actionIndex += 1;
                }

                break;

            case ActionName.GoToChurch:
                dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
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

                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    actionIndex += 1;
                }

                break;

            case ActionName.GoToMarket:
                dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
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

                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    actionIndex += 1;
                }

                break;

            case ActionName.GoHome:
                dest = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
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

                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    actionIndex += 1;
                }

                break;

            case ActionName.HearSermon:
                morale += Time.deltaTime * 10;

                if (morale >= 100) 
                {
                    actionIndex += 1;
                }
                break;

            case ActionName.GetPickaxe:
                havePickaxe = true;
                actionIndex += 1;
                break;

            case ActionName.DumpOre:
                resource = 0;
                actionIndex += 1;
                break;

            case ActionName.Eat:
                hunger = 100;
                actionIndex += 1;
                break;

        }

        return;
    }

    // Here are the containers for my actions, that are used in the GOAP algorithm
    // to make plans. Each has one requirement goal that must be completed (which
    // is set to Goal.None if it has no requirement) and an outcome that the action
    // will cause, as well as a name so I can plug it in to figure out what to do for
    // the action later.
    public class Action 
    {
        public Goal requirement;
        public Goal outcome;
        public ActionName name;

        public Action(Goal req, Goal res, ActionName title) 
        {
            this.requirement = req;
            this.outcome = res;
            this.name = title;
        }
    }

}

