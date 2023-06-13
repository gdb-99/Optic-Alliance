using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{

    string playerTag;

    Transform lens;

    // Start is called before the first frame update
    void Start()
    {
        lens = transform.parent.GetComponent<Transform>();
        playerTag = GameObject.FindGameObjectWithTag("Player").tag;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }



    private void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == playerTag)
        {
            Vector3 direction = col.transform.position - lens.position;
            RaycastHit hit;

            if(Physics.Raycast(lens.transform.position, direction.normalized, out hit, 1000))
            {
                Debug.Log(hit.collider.name);

                if(hit.collider.gameObject.tag == playerTag)
                {
                    //Debug.Log("Maiale spotted");
                }
            }
        }
    }

}
