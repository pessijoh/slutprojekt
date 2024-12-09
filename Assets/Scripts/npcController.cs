using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    NavMeshAgent Agent;
    Animator Anim;

    public Transform[] FoodStations;
    public CashRegister[] CashRegisters;
    public Transform[] ExitPoints;
    public float WaitTimeAtStation = 2f;
    public float WaitTimeAtWaypoint = 1f;
    public float WaitTimeAtRegister = 1f;

    private Transform currentTarget;
    private Transform currentWaypoint;
    private bool isWaiting = false;

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();

        currentTarget = FoodStations[Random.Range(0, FoodStations.Length)];
        Agent.SetDestination(currentTarget.position);
    }

    void Update()
    {
        if (Agent.remainingDistance > Agent.stoppingDistance)
        {
            Anim.SetBool("Walking", true);
        }
        else
        {
            Anim.SetBool("Walking", false);

            if (!isWaiting) 
            {
                StartCoroutine(HandleCurrentStation());
            }
        }
    }

    IEnumerator HandleCurrentStation()
    {
        isWaiting = true;

        if (FoodStations.Contains(currentTarget))
        {
            yield return new WaitForSeconds(WaitTimeAtStation);

            var nearestRegister = FindNearestRegister(CashRegisters);
            if (nearestRegister == null)
            {
                yield break;
            }

            currentWaypoint = nearestRegister.AssignWaypoint(this);
            if (currentWaypoint == null)
            {
                yield break;
            }

            currentTarget = currentWaypoint;
            Agent.SetDestination(currentTarget.position);
        }
        else if (CashRegisters.Any(r => r.ContainsWaypoint(currentTarget)))
        {
            var register = CashRegisters.First(r => r.ContainsWaypoint(currentTarget));
            if (register == null)
            {
                yield break;
            }

            Transform nextWaypoint = null;

            while (true)
            {
                nextWaypoint = register.GetNextWaypoint(this, currentWaypoint);

                if (nextWaypoint == null || nextWaypoint != currentWaypoint)
                {
                    break;
                }

                yield return new WaitForSeconds(WaitTimeAtWaypoint);
            }

            if (nextWaypoint != null)
            {
                currentWaypoint = nextWaypoint;
                currentTarget = currentWaypoint;
                Agent.SetDestination(currentTarget.position);
            }
            else
            {
                yield return new WaitForSeconds(WaitTimeAtRegister);

                Transform closestExit = GetClosestExitPoint();
                if (closestExit != null)
                {
                    currentTarget = closestExit;
                    Agent.SetDestination(currentTarget.position);
                }
                else
                {
                    yield break;
                }
            }
        }
        else if (ExitPoints.Contains(currentTarget))
        {
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }
        else
        {
            yield break;
        }

        isWaiting = false;
    }

    CashRegister FindNearestRegister(CashRegister[] registers)
    {
        return registers
            .OrderBy(r => Vector3.Distance(transform.position, r.transform.position))
            .FirstOrDefault();
    }

    Transform GetClosestExitPoint()
    {
        return ExitPoints
            .OrderBy(exit => Vector3.Distance(transform.position, exit.position))
            .FirstOrDefault();
    }
}
