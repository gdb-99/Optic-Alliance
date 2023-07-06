using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class EnemyPatrolling2 : MonoBehaviour
{

    public event EventHandler OnDistractionReached;

    public List<Detection> detectionCameras = new List<Detection>();
    [SerializeField] private bool isActive;
    [SerializeField] private Transform vision;
    public Transform player;
    public float playerDistance;
    public float AIMoveSpeed;
    public float damping = 6.0f;
    [SerializeField] private Color gizmoColor = Color.red;
    // [SerializeField] private float awareAI = 10f;
    [SerializeField] private float VisionConeAngle = 60f;
    [SerializeField] private float VisionConeRange = 30f;

    public Transform[] navPoint;
    public Transform[] escapeNavPoint;
    public UnityEngine.AI.NavMeshAgent agent;
    public int destPoint = 0;
    public Transform goal;
    private GameManager gameManager;
    private bool CanSeePlayer => IsPlayerInVisionCone() && !IsPlayerObstructed();
    private Vector3 lastKnownPlayerPosition;
    private float searchTimer;
    [SerializeField] private float searchDuration = 5f;
    [SerializeField] private float rotateSpeed = 60f;

    Animator policeAnimator;
    private GameObject exclamationObject;
    private GameObject questionObject;
    private AudioSource barkAudioSource;
    private AudioSource sniffAudioSource;

    /* public enum GamePhase
    {
        Theft,
        Escape
    } */

    //public GamePhase currentPhase;


    enum EnemyState
    {
        Patrolling,
        Chasing,
        Searching,
        Distracted,
        Sleeping
        // Warned
    }

    private EnemyState currentState;

    void Start()
    {
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;

        GameManager.Instance.OnGamePhaseChanged += OnGamePhaseChanged;

        agent.autoBraking = false;
        currentState = EnemyState.Patrolling;

        GameObject[] detectionObjects = GameObject.FindGameObjectsWithTag("DetectionCamera");

        foreach (GameObject detectionObject in detectionObjects)
        {
            Detection detectionScript = detectionObject.GetComponentInChildren<Detection>();

            if (detectionScript != null)
            {
                //detectionScript.OnPlayerDetected += OnPlayerDetectedHandler;
                detectionScript.OnPlayerDetected += DetectionScript_OnPlayerDetected;
                // Controlla se questa guardia deve reagire a questa telecamera
                if (detectionScript.GetReactingGuards().Contains(this))
                {
                    detectionCameras.Add(detectionScript);
                }
            }
        }

        policeAnimator = GetComponent<Animator>();
        exclamationObject = transform.Find("Status/Exclamation").gameObject;
        questionObject = transform.Find("Status/Question").gameObject;
        barkAudioSource = transform.Find("Audio/Bark").gameObject.GetComponent<AudioSource>();
        sniffAudioSource = transform.Find("Audio/Sniff").gameObject.GetComponent<AudioSource>();
        // exclamationObject.SetActive(false);
    }

    private void DetectionScript_OnPlayerDetected(object sender, Detection.OnSwitchItemEventArgs aaa)
    {
        // Controllo se la telecamera corrente è nella lista detectionCameras
        if (detectionCameras.Contains(aaa.camera)) //
        {
            currentState = EnemyState.Chasing;
            lastKnownPlayerPosition = aaa.playerTransform.position;
            // Esegui le azioni specifiche per la guardia quando il giocatore viene rilevato da questa telecamera
        }
    }

    void Update()
    {
        playerDistance = Vector3.Distance(player.position, transform.position);

        switch (currentState)
        {
            case EnemyState.Patrolling:
                questionObject.SetActive(false);
                if (CanSeePlayer)
                {
                    exclamationObject.SetActive(true);
                    barkAudioSource.Play();
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
                questionObject.SetActive(false);
                if (CanSeePlayer)
                {
                    exclamationObject.SetActive(true);
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
                        agent.angularSpeed = 0f;
                        searchTimer = 0f;
                        policeAnimator.SetBool("isStop", true);
                        sniffAudioSource.Play();
                        exclamationObject.SetActive(false);
                        questionObject.SetActive(true);
                        currentState = EnemyState.Searching;
                    }
                }
                break;

            case EnemyState.Searching:

                searchTimer += Time.deltaTime;
                if (CanSeePlayer)
                {
                    barkAudioSource.Play();
                    agent.angularSpeed = 120f;
                    policeAnimator.SetBool("isStop", false);
                    currentState = EnemyState.Chasing;
                }
                if (searchTimer >= searchDuration)
                {
                    agent.angularSpeed = 120f;
                    policeAnimator.SetBool("isStop", false);
                    currentState = EnemyState.Patrolling;
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, Mathf.PingPong(Time.time * rotateSpeed, 90) - 30, 0);
                    Debug.Log("Forse è qui intorno...");
                }
                break;

            case EnemyState.Distracted:
                if (CanSeePlayer) {
                    Debug.Log("Ehi ti ho visto!");
                    currentState = EnemyState.Chasing;
                    Chase();
                } else if (agent.remainingDistance < 0.2f) {
                    OnDistractionReached?.Invoke(this, EventArgs.Empty);
                    agent.angularSpeed = 0f;
                    searchTimer = 0f;
                    policeAnimator.SetBool("isStop", true);
                    sniffAudioSource.Play();
                    exclamationObject.SetActive(false);
                    questionObject.SetActive(true);
                    currentState = EnemyState.Searching;
                }
                break;

            case EnemyState.Sleeping:
                //DO NOTHING WHILE COROUTINE IS RUNNING
                break;
        }
    }

    void LookAtPlayer()
    {
        transform.LookAt(player);
    }

    void GotoNextPoint()
    {
        if (navPoint.Length == 0)
        {
            return;
        }

        if (agent.remainingDistance < 0.2f)
        {
            UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
            agent.CalculatePath(navPoint[destPoint].position, path);
            agent.path = path;
            destPoint = (destPoint + 1) % navPoint.Length;
        }
        /*if (GameManager.Instance.currentPhase == GameManager.GamePhase.Theft) //(currentPhase == GamePhase.Theft)
        {
            if (navPoint.Length == 0)
            {
                return;
            }

            if (agent.remainingDistance < 0.2f)
            {
                UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
                agent.CalculatePath(navPoint[destPoint].position, path);
                agent.path = path;
                destPoint = (destPoint + 1) % navPoint.Length;
            }
        }
        else if (GameManager.Instance.currentPhase == GameManager.GamePhase.Escape)
        {
            if (escapeNavPoint.Length == 0)
            {
                return;
            }

            if (agent.remainingDistance < 0.2f)
            {
                UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
                agent.CalculatePath(escapeNavPoint[destPoint].position, path);
                agent.path = path;
                destPoint = (destPoint + 1) % escapeNavPoint.Length;
            }
        } */
    }

    void Chase()
    {
        agent.destination = player.position;
    }

    void ReturnToLastKnownPlayerPosition()
    {
        agent.destination = lastKnownPlayerPosition;
    }

    public void Distract(Vector3 distractionPosition) {
        Debug.Log("GUARD IS GETTING DISTRACTED");
        if (currentState != EnemyState.Chasing && currentState != EnemyState.Distracted) {
            currentState = EnemyState.Distracted;
            //lastKnownPlayerPosition = distractionPosition;
            agent.destination = distractionPosition;
        }
    }

    public void Sleep() {
        if (currentState != EnemyState.Sleeping) {
            currentState = EnemyState.Sleeping;
            agent.isStopped = true;
            agent.destination = transform.position;
            StartCoroutine(SleepCoroutine());
            agent.isStopped = false;
        }
    }

    IEnumerator SleepCoroutine() {
        Debug.Log("ZZZ...");
        yield return new WaitForSeconds(10f);
        Debug.Log("WAKING UP");
        currentState = EnemyState.Patrolling;
    }

    private void OnGamePhaseChanged(GameManager.GamePhase newPhase)
    {
        // Controlla la nuova fase di gioco e abilita o disabilita gli oggetti di conseguenza
        if (newPhase == GameManager.GamePhase.Theft)
        {
            // Abilita gli oggetti per la fase di furto
            //gameObject.SetActive(true);
        }
        else if (newPhase == GameManager.GamePhase.Escape)
        {
            // Abilita gli oggetti per la fase di fuga
            gameObject.SetActive(true);
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        DrawVisionCone(vision.position, transform.forward, VisionConeAngle, VisionConeRange);
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
            if (Physics.Raycast(vision.position, directionToPlayer, out hit, VisionConeRange))
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

        if (Physics.Raycast(vision.position, directionToPlayer, out hit, playerDistance))
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