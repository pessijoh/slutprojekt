using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Linq;


public class CharacterBehavior : MonoBehaviour
{
    public Transform[] FoodStations; 
    public Transform[] CashRegisters; 
    public float WaitTimeAtStation = 2f; 
    public float WaitTimeAtRegister = 1f; 
    private NavMeshAgent agent;
    private Animator animator;
    private Transform currentTarget;
    private float startTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        startTime = Time.time;

       
        SelectRandomFoodStation();
    }

    void Update()
    {
        
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }

     
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(HandleStation());
        }
    }

    void SelectRandomFoodStation()
    {
        currentTarget = FoodStations[Random.Range(0, FoodStations.Length)];
        agent.SetDestination(currentTarget.position);
    }

    IEnumerator HandleStation()
    {
        yield return new WaitForSeconds(WaitTimeAtStation);

        
        if (FoodStations.Contains(currentTarget))
        {
            currentTarget = FindNearestRegister();
            agent.SetDestination(currentTarget.position);
        }
        else if (CashRegisters.Contains(currentTarget))
        {
           
            float totalTime = Time.time - startTime;
            Debug.Log($"Character finished in {totalTime:F2} seconds.");
            Destroy(gameObject); 
        }
    }

    Transform FindNearestRegister()
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (var register in CashRegisters)
        {
            float distance = Vector3.Distance(transform.position, register.position);
            if (distance < minDistance)
            {
                nearest = register;
                minDistance = distance;
            }
        }
        return nearest;
    }
}
