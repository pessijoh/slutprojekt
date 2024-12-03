using UnityEngine;
using System.Collections;
using System.Linq; 
using UnityEngine.AI;


public class NPCController : MonoBehaviour
{
    
    NavMeshAgent Agent;
    Animator Anim;

    public Transform[] FoodStations;
    public Transform[] CashRegisters;
    public Transform ExitPoint;
    public float WaitTimeAtStation = 2f;
    public float WaitTimeAtRegister = 1f;

    private Transform currentTarget;
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

           
            currentTarget = FindNearestTarget(CashRegisters);
            Agent.SetDestination(currentTarget.position);
        }
        else if (CashRegisters.Contains(currentTarget))
        {
            yield return new WaitForSeconds(WaitTimeAtRegister);

         
            currentTarget = ExitPoint;
            Agent.SetDestination(currentTarget.position);
        }
        else if (currentTarget == ExitPoint)
        {
          
            yield return new WaitForSeconds(0.5f); 
            Destroy(gameObject);
        }

        isWaiting = false; 
    }

    Transform FindNearestTarget(Transform[] targets)
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (var target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < minDistance)
            {
                nearest = target;
                minDistance = distance;
            }
        }

        return nearest;
    }
}
