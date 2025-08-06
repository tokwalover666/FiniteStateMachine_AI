using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIRoam : MonoBehaviour
{
    [SerializeField] NavMeshAgent AI;
    [SerializeField] float range;
    [SerializeField] Transform playerTransform;
    [SerializeField] float viewAngle = 45f;
    [SerializeField] float viewDistance = 10f;
    [SerializeField] LayerMask player;
    [SerializeField] LayerMask obstructionMask;
    [SerializeField] Transform ground;

    private bool isChasingPlayer = false;
    private bool playerInSight;
    void Start()
    {
        AI = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        DetectPlayer();

        if (playerInSight)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            ChasePlayer();
            return;
        }


        if (AI.remainingDistance <= AI.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(transform.position, range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                AI.SetDestination(point);
            }
        }
    }


    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + new Vector3(Random.Range(-range, range), 0f,Random.Range(-range, range));
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void DetectPlayer()
    {
        Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (Vector3.Distance(transform.position, playerTransform.position) <= viewDistance && angle <= viewAngle / 2)
        {
            if (!Physics.Raycast(transform.position + Vector3.up, dirToPlayer, out RaycastHit hit, viewDistance, obstructionMask))
            {
                if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer, viewDistance, player))
                {
                    playerInSight = true;
                    return;
                }
            }
        }

        playerInSight = false;
    }

    private void ChasePlayer()
    {
        if (playerInSight)
        {
            AI.SetDestination(playerTransform.position);
        }


    }

    private void OnDrawGizmosSelected()
    {
        if (playerTransform == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewDistance);
    }
    Vector3 DirFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}