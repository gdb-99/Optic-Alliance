using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR


public class SurveillanceAI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI FeedbackDisplay;

    [SerializeField] float _VisionConeAngle = 60f;
    [SerializeField] float _VisionConeRange = 10f;
    [SerializeField] Color _VisionConeColour = new Color(1f, 0f, 0f, 0.25f);

    [SerializeField] float _HearingRange = 20f;
    [SerializeField] Color _HearingRangeColour = new Color(1f, 1f, 0f, 0.25f);

    [SerializeField] float _ProximityDetectionRange = 3f;
    [SerializeField] Color _ProximityRangeColour = new Color(1f, 1f, 1f, 0.25f);


    //AGGIUNTA:
    [SerializeField] private float rotationAngle = 45f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] float startingAngle = 0f;


    private Quaternion initialRotation;
    private Quaternion targetRotation;
    //AGGIUNTA:
    public bool isPlayerVisible = false;
    private bool isRotatingClockwise = true;
    private float currentRotation = 0f;



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


    void Awake()
    {
        /* CosVisionConeAngle = Mathf.Cos(VisionConeAngle * Mathf.Deg2Rad);
        Awareness = GetComponent<AwarenessSystem>(); */
        initialRotation = transform.rotation;
        targetRotation = initialRotation * Quaternion.Euler(0f, rotationAngle, 0f);
    }

    // Start is called before the first frame update
    private void Start()
    {
        initialRotation = Quaternion.Euler(0f, startingAngle, 0f);
    }



    // Update is called once per frame
    private void Update()
        {
            float targetRotation = isRotatingClockwise ? rotationAngle : -rotationAngle;
            Quaternion targetRotationQuat = initialRotation * Quaternion.Euler(0f, targetRotation, 0f);
            currentRotation = Mathf.LerpAngle(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = initialRotation * Quaternion.Euler(0f, currentRotation, 0f);

            if (Mathf.Abs(currentRotation - targetRotation) < 1f)
            {
                isRotatingClockwise = !isRotatingClockwise;
            }
    }


    public void ReportCanSee(DetectableTarget seen)
    {
        //AGGIUNTA:
        isPlayerVisible = (seen != null);

        Awareness.ReportCanSee(seen);
    }

    /* public void OnDetected(GameObject target)
    {
        FeedbackDisplay.text = "I see you " + target.gameObject.name;
    } */

    /* public void OnFullyDetected(GameObject target)
    {
        FeedbackDisplay.text = "Charge! " + target.gameObject.name;
    } */

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
[CustomEditor(typeof(SurveillanceAI))]
public class SurveillanceAIEditor : Editor
{
    public void OnSceneGUI()
    {
        var ai = target as SurveillanceAI;


        // work out the start point of the vision cone
        Vector3 startPoint = Mathf.Cos(-ai.VisionConeAngle * Mathf.Deg2Rad) * ai.transform.forward +
                             Mathf.Sin(-ai.VisionConeAngle * Mathf.Deg2Rad) * ai.transform.right;

        // draw the vision cone
        Handles.color = ai.VisionConeColour;
        Handles.DrawSolidArc(ai.transform.position, Vector3.up, startPoint, ai.VisionConeAngle * 2f, ai.VisionConeRange);
    }
}
#endif // UNITY_EDITOR