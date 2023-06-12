using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxHeadController : MonoBehaviour
{
    [SerializeField] Transform head;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Debug.Log(Vector3.Distance(new(transform.localPosition.x, transform.position.y, 0), new(mousePosition.x, mousePosition.y, 0)));
        //Debug.Log(Vector3.Distance(mousePosition, mousePosition));
    }
}
