using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLightController : MonoBehaviour
{
    [SerializeField] Light alarmLight;

    // Start is called before the first frame update
    void Start()
    {
        alarmLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.L)) { 
          //  alarmLight.enabled = true;
       // }
       if (GameManager.Instance.currentPhase == GameManager.GamePhase.Escape)
        {
            alarmLight.enabled = true;
            alarmLight.bounceIntensity = 10f;
        }
    }


}
