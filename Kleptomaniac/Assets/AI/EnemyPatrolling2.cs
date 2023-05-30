using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class EnemyPatrolling2 : MonoBehaviour
{
    public Transform player;
    public float playerDistance;
    public float AIMoveSpeed;
    public float damping = 6.0f;
    [SerializeField] private Color gizmoColor = Color.red;
    [SerializeField] private float awareAI = 10f;
    [SerializeField] private float VisionConeAngle = 60f;
    [SerializeField] private float VisionConeRange = 30f;

    public Transform[] navPoint;
    public UnityEngine.AI.NavMeshAgent agent;
    public int destPoint = 0;
    public Transform goal;

    private bool CanSeePlayer => IsPlayerInVisionCone() && !IsPlayerObstructed();

    void Start()
    {
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;

        agent.autoBraking = false;
    }

    void Update()
    {
        playerDistance = Vector3.Distance(player.position, transform.position);

        if (CanSeePlayer)
        {
            LookAtPlayer();
            Debug.Log("Seen");

            if (playerDistance > 2f)
                Chase();
            else
                GotoNextPoint();
        }
        else
        {
            if (agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }
    }

    void LookAtPlayer()
    {
        transform.LookAt(player);
    }

    void GotoNextPoint()
    {
        if (navPoint.Length == 0)
            return;
        agent.destination = navPoint[destPoint].position;
        destPoint = (destPoint + 1) % navPoint.Length;
    }

    void Chase()
    {
        transform.Translate(Vector3.forward * AIMoveSpeed * Time.deltaTime);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        DrawVisionCone(transform.position, transform.forward, VisionConeAngle, VisionConeRange);
    }

    private void DrawVisionCone(Vector3 center, Vector3 forward, float angle, float range)
    {
        Vector3 startPoint = Mathf.Cos(-angle * Mathf.Deg2Rad) * forward +
                             Mathf.Sin(-angle * Mathf.Deg2Rad) * transform.right;

        Handles.color = gizmoColor;
        Handles.DrawSolidArc(center, Vector3.up, startPoint, angle * 2f, range);
    }

    private bool IsPlayerInVisionCone()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(directionToPlayer, transform.forward);

        if (angleToPlayer <= VisionConeAngle)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, VisionConeRange))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsPlayerObstructed()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, directionToPlayer, out hit, playerDistance))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return false;
            }
        }

        return true;
    }
#endif // UNITY_EDITOR
}
