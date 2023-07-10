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
    public static event EventHandler OnGameOver;

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
    public int destPoint;
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
    private GameObject sheepObject;
    private AudioSource barkAudioSource;
    private AudioSource sniffAudioSource;
    private AudioSource snoreAudioSource;
    private bool wokeUp;

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
        destPoint = 0;
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;

        GameManager.Instance.OnGamePhaseChanged += OnGamePhaseChanged;

        agent.autoBraking = false;
        currentState = EnemyState.Patrolling;

        GameObject[] detectionObjects = GameObject.FindGameObjectsWithTag("DetectionCamera");

        wokeUp = false;

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
        sheepObject = transform.Find("Status/Sheep").gameObject;
        barkAudioSource = transform.Find("Audio/Bark").gameObject.GetComponent<AudioSource>();
        sniffAudioSource = transform.Find("Audio/Sniff").gameObject.GetComponent<AudioSource>();
        snoreAudioSource = transform.Find("Audio/Snore").gameObject.GetComponent<AudioSource>();
        // exclamationObject.SetActive(false);
    }

    private void DetectionScript_OnPlayerDetected(object sender, Detection.OnSwitchItemEventArgs aaa)
    {
        // Controllo se la telecamera corrente è nella lista detectionCameras
        if (detectionCameras.Contains(aaa.camera)) //
        {
            //barkAudioSource.Play();
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
            //WALKING
            case EnemyState.Patrolling:

                policeAnimator.SetTrigger("patrolling");
                agent.speed = 1.0f;

                Debug.Log("STATE = PATROLLING");
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
                    policeAnimator.SetTrigger("chasing");
                    agent.speed = 1.8f;
                    exclamationObject.SetActive(true);
                    LookAtPlayer();
                    if (playerDistance > 1f)
                    {
                        Chase();
                    }
                    else
                    {
                        Debug.Log("Game Over");
                        OnGameOver?.Invoke(this, EventArgs.Empty);
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
                        //policeAnimator.SetBool("isStop", true);
                        sniffAudioSource.Play();
                        exclamationObject.SetActive(false);
                        questionObject.SetActive(true);
                        currentState = EnemyState.Searching;
                    }
                }
                break;

            case EnemyState.Searching:
                Debug.Log("STATE = SEARCHING");
                searchTimer += Time.deltaTime;
                if (CanSeePlayer)
                {
                    Debug.Log("I AM SEARCHING BUT I JUST SAW YOU");
                    barkAudioSource.Play();
                    //policeAnimator.SetBool("isSleeping", false);
                    //policeAnimator.SetBool("isStop", false);

                    policeAnimator.SetTrigger("chasing");

                    agent.angularSpeed = 120f;
                    currentState = EnemyState.Chasing;
                }
                if (searchTimer >= searchDuration)
                {
                    agent.angularSpeed = 120f;

                 

                    //policeAnimator.SetBool("isStop", false);
                    currentState = EnemyState.Patrolling;
                }
                else
                {
                    policeAnimator.SetTrigger("searching");
                    //policeAnimator.SetBool("isStop", true);
                    transform.localEulerAngles = new Vector3(0, Mathf.PingPong(Time.time * rotateSpeed, 90) - 30, 0);
                    Debug.Log("Forse è qui intorno...");
                }
                break;

            case EnemyState.Distracted:
                Debug.Log("STATE = DISTRACTED");
                if (CanSeePlayer) {
                    Debug.Log("Ehi ti ho visto!");
                    currentState = EnemyState.Chasing;
                    Chase();
                } else if (agent.remainingDistance < 0.2f) {
                    OnDistractionReached?.Invoke(this, EventArgs.Empty);
                    agent.angularSpeed = 0f;
                    searchTimer = 0f;
                    //policeAnimator.SetBool("isStop", true);
                    sniffAudioSource.Play();
                    exclamationObject.SetActive(false);
                    questionObject.SetActive(true);
                    currentState = EnemyState.Searching;
                }
                break;

            case EnemyState.Sleeping:
                Debug.Log("STATE = SLEEPING");
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
            if (wokeUp) {
                destPoint--;
                destPoint = (destPoint + navPoint.Length) % navPoint.Length;
                wokeUp = false;
            }
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
        if (currentState != EnemyState.Chasing && currentState != EnemyState.Distracted && currentState != EnemyState.Sleeping) {
            currentState = EnemyState.Distracted;
            //lastKnownPlayerPosition = distractionPosition;
            agent.destination = distractionPosition;
        }
    }

    public void Sleep() {
        //if (currentState != EnemyState.Sleeping) {
        currentState = EnemyState.Sleeping;
        agent.isStopped = true;
        //agent.destination = transform.position;
        StopCoroutine(SleepCoroutine());
        StartCoroutine(SleepCoroutine());
        agent.isStopped = false;
        //}
    }

    IEnumerator SleepCoroutine() {
        Debug.Log("ZZZ...");
        agent.angularSpeed = 0f;
        agent.destination = transform.position;
        policeAnimator.SetTrigger("sleeping");
        //policeAnimator.SetBool("isStop", true);
        //policeAnimator.SetBool("isSleeping", true);
        questionObject.SetActive(false);
        exclamationObject.SetActive(false);
        sheepObject.SetActive(true);
        snoreAudioSource.Play();
        yield return new WaitForSeconds(10f);
        Debug.Log("WAKING UP");
        if (CanSeePlayer) {
            //policeAnimator.SetBool("isSleeping", false);
            //policeAnimator.SetBool("isStop", false);
            wokeUp = true;
            sheepObject.SetActive(false);
            agent.angularSpeed = 120f;
            barkAudioSource.Play();
            currentState = EnemyState.Chasing;
        } else {
            //policeAnimator.SetBool("isStop", true);
            //policeAnimator.SetBool("isSleeping", false);
            searchTimer = 0f;
            wokeUp = true;
            sheepObject.SetActive(false);
            sniffAudioSource.Play();
            currentState = EnemyState.Searching;
        }
        
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

    private void OnDestroy() {
        GameManager.Instance.OnGamePhaseChanged -= OnGamePhaseChanged;
        foreach(Detection detectionCamera in detectionCameras) {
            detectionCamera.OnPlayerDetected -= DetectionScript_OnPlayerDetected;
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