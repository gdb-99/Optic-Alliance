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
    private Vector3 lastKnownPlayerPosition;
    private float searchTimer;
    private float searchDuration = 5f;
    [SerializeField] private float rotationSpeed = 1f; // Velocità di rotazione in gradi al secondo

    enum EnemyState
    {
        Patrolling,
        Chasing,
        Searching
    }

    private EnemyState currentState;

    void Start()
    {
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;

        agent.autoBraking = false;
        currentState = EnemyState.Patrolling;
    }

    void Update()
    {
        playerDistance = Vector3.Distance(player.position, transform.position);

        switch (currentState)
        {
            case EnemyState.Patrolling:
                if (CanSeePlayer)
                {
                    Debug.Log("Ehi ti ho visto!");
                    currentState = EnemyState.Chasing;
                    Chase();
                }
                else if (agent.remainingDistance < 0.2f)
                {
                    Debug.Log("Punto raggiunto, next");
                    GotoNextPoint();
                }
                break;

            case EnemyState.Chasing:
                if (CanSeePlayer)
                {
                    LookAtPlayer();
                    if (playerDistance > 2f)
                    {
                        Chase();
                    }
                    else
                    {
                        Debug.Log("Game Over");
                    }
                }
                else
                {
                    Debug.Log("Dove sei finito?");
                    ReturnToLastKnownPlayerPosition();
                    if (agent.remainingDistance < 0.1f)
                    {
                        //agent.enabled = false;
                        agent.angularSpeed = 0f;
                        searchTimer = 0f;
                        //StartCoroutine(AnimateEnemy());
                        currentState = EnemyState.Searching;
                    }

                    //searchTimer = 0f;
                }
                break;

            case EnemyState.Searching:
                searchTimer += Time.deltaTime;
                //agent.enabled = false;
                if (CanSeePlayer)
                {
                    // agent.enabled = true;
                    agent.angularSpeed = 120f;
                    currentState = EnemyState.Chasing;
                   // searchTimer = 0f;
                }
                if (searchTimer >= searchDuration)
                {
                    agent.angularSpeed = 120f;
                    currentState = EnemyState.Patrolling;
                    //searchTimer = 0f;
                }
                else
                {
                    //ReturnToLastKnownPlayerPosition();
                    //DA MIGLIORARE
                    LookAround();
                    Debug.Log("Girotondo");
                }
                break;
        }
    }
    IEnumerator AnimateEnemy()
    {
        float rotationAngle = 120f;
        for (int i = 0; i<3; i++)
        {
        // Calcola la velocità di rotazione in radianti al secondo
        float rotationSpeedRad = rotationSpeed * Mathf.Deg2Rad;
        //Debug.Log("Prima" + searchTimer);
        // Ruota il nemico attorno all'asse verticale (asse Y)
        transform.Rotate(Vector3.up, rotationAngle * rotationSpeedRad *Time.deltaTime, Space.Self);
        yield return new WaitForSeconds(0.1f);
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
        agent.destination = player.position;
    }

    void ReturnToLastKnownPlayerPosition()
    {
        agent.destination = lastKnownPlayerPosition;
    }

    void LookAround()
    {
        float rotationAngle = 120f;
        // Calcola la velocità di rotazione in radianti al secondo
        float rotationSpeedRad = rotationSpeed * Mathf.Deg2Rad;
            //Debug.Log("Prima" + searchTimer);
            // Ruota il nemico attorno all'asse verticale (asse Y)
            transform.Rotate(Vector3.up, rotationAngle * rotationSpeedRad * Time.deltaTime, Space.Self);
        if (searchTimer >= searchDuration)
        {
            // Arresta la rotazione
            transform.rotation = Quaternion.identity;
        }
        /*// Calcola l'angolo di rotazione desiderato per un giro completo
        float rotationAngle = 60f;
        // Calcola la velocità di rotazione in radianti al secondo
        float rotationSpeedRad = rotationSpeed * Mathf.Deg2Rad;
        //Debug.Log("Prima" + searchTimer);
        // Ruota il nemico attorno all'asse verticale (asse Y)
        transform.Rotate(Vector3.up, rotationAngle * Time.deltaTime);

        //Debug.Log("Dopo" + searchTimer);
        // Controlla se è trascorso il tempo per un giro completo
        if (searchTimer >= searchDuration)
        {
            // Arresta la rotazione
            transform.rotation = Quaternion.identity;
        } */
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
                    lastKnownPlayerPosition = player.position;
                    Debug.Log("True");
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
