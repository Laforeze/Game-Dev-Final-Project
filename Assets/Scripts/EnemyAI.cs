using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float waitTime = 2f;
    public float detectionRadius = 10f;
    public float investigateTime = 10f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireCooldown = 1f;
  

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private bool isInvestigating = false;
    private bool isAttacking = false;
    private Vector3 lastKnownPosition;
    private float investigationTimer = 0f;
    private float fireTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (isAttacking)
        {
            Attack();
        }
        else if (isInvestigating)
        {
            InvestigateUpdate();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }

        // Always face next waypoint
        Vector3 direction = agent.steeringTarget - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public void Investigate(Vector3 position)
    {
        Debug.Log("Investigating");
        isInvestigating = true;
        isAttacking = false;
        lastKnownPosition = position;
        investigationTimer = 0f;
        agent.SetDestination(lastKnownPosition);
    }

    void InvestigateUpdate()
{
    investigationTimer += Time.deltaTime;
    PlayerHUD playerHUD = Object.FindFirstObjectByType<PlayerHUD>();
    if (playerHUD != null && playerHUD.CurrentVisibility >= 100f)
    {
        Vector3 direction = (playerHUD.transform.position - transform.position).normalized;
        Ray ray = new Ray(firePoint.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, detectionRadius))
        {
            if (hit.transform.CompareTag("Player"))
            {
                isAttacking = true;
                isInvestigating = false;
                agent.ResetPath(); // Stop moving while attacking
                return;
            }
        }
    }

    if (investigationTimer >= investigateTime)
    {
        isInvestigating = false;
        GoToClosestWaypoint();
    }
}

void Attack()
{
    PlayerHUD playerHUD = Object.FindFirstObjectByType<PlayerHUD>();
    if (playerHUD == null)
    {
        isAttacking = false;
        GoToClosestWaypoint();
        return;
    }

    Vector3 direction = (playerHUD.transform.position - transform.position).normalized;
    Ray ray = new Ray(firePoint.position, direction);
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, detectionRadius))
    {
        if (hit.transform.CompareTag("Player"))
        {
            // Face the player
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            if (fireTimer >= fireCooldown)
            {
                ShootAtPlayer(playerHUD.transform);
                fireTimer = 0f;
            }
            return;
        }
    }

    // Lost line of sight
    isAttacking = false;
    GoToClosestWaypoint();
}


    void GoToClosestWaypoint()
    {
        float shortest = Mathf.Infinity;
        int closestIndex = 0;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, waypoints[i].position);
            if (distance < shortest)
            {
                shortest = distance;
                closestIndex = i;
            }
        }

        currentWaypointIndex = closestIndex;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void ShootAtPlayer(Transform player)
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.transform.forward = (player.position - firePoint.position).normalized;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = bullet.transform.forward * 10f;
            }
        }
    }
}
