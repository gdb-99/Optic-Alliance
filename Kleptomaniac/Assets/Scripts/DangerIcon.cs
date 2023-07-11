using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerIcon : MonoBehaviour {
    public GameObject dangIconObj;
    public GameObject safeIconObj;

    // Start is called before the first frame update
    void Start() {
        dangIconObj = gameObject.transform.Find("Danger Icon").gameObject;
        safeIconObj = gameObject.transform.Find("Safe Icon").gameObject;
        dangIconObj.SetActive(false);
        dangIconObj.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        if (GameManager.Instance.isPigInDanger == true) {
            dangIconObj.SetActive(true);
            safeIconObj.SetActive(false);
        } else {
            dangIconObj.SetActive(false);
            safeIconObj.SetActive(true);
        }
    }
}