using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetText : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI text;

    public void SetTextValue(string value)
    {
        text.text = value;
    }
}
