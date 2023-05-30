using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

[RequireComponent(typeof(AwarenessSystem))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI FeedbackDisplay;
    
    [SerializeField] float _VisionConeAngle = 60f;
    [SerializeField] float _VisionConeRange = 30f;
    [SerializeField] Color _VisionConeColour = new Color(1f, 0f, 0f, 0.25f);

    [SerializeField] float _HearingRange = 20f;
    [SerializeField] Color _HearingRangeColour = new Color(1f, 1f, 0f, 0.25f);

    [SerializeField] float _ProximityDetectionRange = 3f;
    [SerializeField] Color _ProximityRangeColour = new Color(1f, 1f, 1f, 0.25f);

    [SerializeField] float speed = 5f;
    
    public bool isPlayerVisible = false;
    public Vector3 EyeLocation => transform.position;
    public Vector3 EyeDirection => transform.forward;

    public float VisionConeAngle => _VisionConeAngle;
    public float VisionConeRange => _VisionConeRange;
    public Color VisionConeColour => _VisionConeColour;

    public float HearingRange => _HearingRange;
    public Color HearingRangeColour => _HearingRangeColour;

    public float ProximityDetectionRange => _ProximityDetectionRange;
    public Color ProximityDetectionColour => _ProximityRangeColour;

    public float CosVisionConeAngle { get; private set; } = 0f;

    AwarenessSystem Awareness;
    //AGGIUNTA
    private bool isMoving = false;

    void Awake()
    {
        CosVisionConeAngle = Mathf.Cos(VisionConeAngle * Mathf.Deg2Rad);
        Awareness = GetComponent<AwarenessSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* //IN ORIGINE ERA VUOTO
         GameObject player = GameObject.FindGameObjectWithTag("Player");

         if (player != null && isPlayerVisible)
         {
             // Ottenere la direzione verso il giocatore
             Vector3 playerDirection = player.transform.position - transform.position;
             playerDirection.y = 0f; // Assicurarsi che la rotazione sia solo attorno all'asse y

             // Ruotare l'Enemy Character verso il giocatore
             if (playerDirection != Vector3.zero)
             {
                 Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
                 transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
             }
         } */
        if (isMoving)
        {
            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                Vector3 targetPosition = player.transform.position;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                // Orienta l'EnemyCharacter verso il giocatore
                transform.LookAt(targetPosition);

                // Puoi aggiungere ulteriori logiche qui, come l'attacco al giocatore
            }
        }
    }

    public void ReportCanSee(DetectableTarget seen)
    {
        //AGGIUNTA:
        isPlayerVisible = (seen != null);
        
        Awareness.ReportCanSee(seen);
    }

    public void ReportCanHear(GameObject source, Vector3 location, EHeardSoundCategory category, float intensity)
    {
        Awareness.ReportCanHear(source, location, category, intensity);
    }

    public void ReportInProximity(DetectableTarget target)
    {
        Awareness.ReportInProximity(target);
    }

    public void OnSuspicious()
    {
        FeedbackDisplay.text = "I hear you";
    }

    public void OnDetected(GameObject target)
    {
        FeedbackDisplay.text = "I see you " + target.gameObject.name;
    }

    public void OnFullyDetected(GameObject target)
    {
        FeedbackDisplay.text = "Charge! " + target.gameObject.name;
        isMoving = true;

        // Ottieni il componente EnemyPatrolling
        EnemyPatrolling enemyPatrolling = GetComponent<EnemyPatrolling>();
        if (enemyPatrolling != null)
        {
            // Interrompi il patrolling
            enemyPatrolling.StopPatrolling();
        }
    }


    public void OnLostDetect(GameObject target)
    {
        //AGGIUNTA:
        isPlayerVisible = false;

        FeedbackDisplay.text = "Where are you " + target.gameObject.name;
    }

    public void OnLostSuspicion()
    {
        //AGGIUNTA:
        isPlayerVisible = false;
                
        FeedbackDisplay.text = "Where did you go";
    }

    public void OnFullyLost()
    {
        //AGGIUNTA:
        isPlayerVisible = false;

        FeedbackDisplay.text = "Must be nothing";
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyAI))]
public class EnemyAIEditor : Editor
{
    public void OnSceneGUI()
    {
        var ai = target as EnemyAI;

        // draw the detectopm range
        Handles.color = ai.ProximityDetectionColour;
        Handles.DrawSolidDisc(ai.transform.position, Vector3.up, ai.ProximityDetectionRange);

        // draw the hearing range
        Handles.color = ai.HearingRangeColour;
        Handles.DrawSolidDisc(ai.transform.position, Vector3.up, ai.HearingRange);

        // work out the start point of the vision cone
        Vector3 startPoint = Mathf.Cos(-ai.VisionConeAngle * Mathf.Deg2Rad) * ai.transform.forward +
                             Mathf.Sin(-ai.VisionConeAngle * Mathf.Deg2Rad) * ai.transform.right;

        // draw the vision cone
        Handles.color = ai.VisionConeColour;
        Handles.DrawSolidArc(ai.transform.position, Vector3.up, startPoint, ai.VisionConeAngle * 2f, ai.VisionConeRange);        
    }
}
#endif // UNITY_EDITOR