using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Brightness : MonoBehaviour
{
    public Slider brightSlider;
    public PostProcessProfile brightness;
    public PostProcessLayer layer;

    AutoExposure exposure;

    // Start is called before the first frame update
    void Start()
    {
        brightness.TryGetSettings(out exposure);
        brightSlider.value = exposure.keyValue.value;
        AdjustBrightness(brightSlider.value);
    }

    public void AdjustBrightness(float value){
        if(value != 0){
            exposure.keyValue.value = value;
        }
        else{
            exposure.keyValue.value = .05f;
        }
        //brightness.AddSettings(exposure);
    }
}
